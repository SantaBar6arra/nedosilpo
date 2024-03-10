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
    private readonly IEventHandler _eventHandler;

    public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
    {
        _config = config.Value;
        _eventHandler = eventHandler;
    }

    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetKeyDeserializer(Deserializers.Utf8)
            .Build();

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        consumer.Subscribe(topic);


        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult.Message is null)
                continue;

            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

            var handlerMethod = _eventHandler.GetType().GetMethod("On", [@event?.GetType()])
                ?? throw new InvalidOperationException("no 'On' method found for event type");

            handlerMethod.Invoke(_eventHandler, [@event]);
            consumer.Commit(consumeResult);
        }
    }
}
