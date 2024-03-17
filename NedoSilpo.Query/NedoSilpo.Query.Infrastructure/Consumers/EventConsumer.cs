using System.Reflection;
using System.Text.Json;
using Confluent.Kafka;
using Cqrs.Core.Consumers;
using Cqrs.Core.Events;
using Microsoft.Extensions.Options;
using NedoSilpo.Common.Handlers;
using NedoSilpo.Query.Infrastructure.Converters;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _config;
    private readonly IList<IEventHandler> _eventHandlers;

    public EventConsumer(IOptions<ConsumerConfig> config, IEnumerable<IEventHandler> eventHandlers)
    {
        _config = config.Value;
        _eventHandlers = eventHandlers.ToList();
    }

    public async Task Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult.Message is null)
                continue;

            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options)
                ?? throw new InvalidOperationException("could not deserialize event message!");

            await HandleEvent(@event);

            consumer.Commit(consumeResult);
        }
    }

    private async Task HandleEvent(BaseEvent @event)
    {
        var handlers = _eventHandlers
            .Select(handler => (handler, handler.GetType().GetMethod("On", [@event.GetType()])))
            .OfType<(IEventHandler, MethodInfo)>()
            .ToArray();

        if (handlers.Length is 0)
            throw new InvalidOperationException("no 'On' method found for event type");

        foreach (var (handler, on) in handlers)
            await (Task)on.Invoke(handler, [@event]);
    }
}
