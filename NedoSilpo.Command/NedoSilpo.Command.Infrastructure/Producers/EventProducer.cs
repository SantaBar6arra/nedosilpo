using System.Text.Json;
using Confluent.Kafka;
using Cqrs.Core.Events;
using Cqrs.Core.Producers;
using Microsoft.Extensions.Options;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Infrastructure.Producers;

public class EventProducer : IEventProducer
{
    private readonly ProducerConfig _config;

    public EventProducer(IOptions<ProducerConfig> config) => _config = config.Value;

    public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        using var producer = new ProducerBuilder<string, string>(_config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };

        var produceResult = await producer.ProduceAsync(topic, message);

        if (produceResult.Status is PersistenceStatus.NotPersisted)
            throw new Exception(
                $"event {@event.GetType().Name} was not persisted to topic {topic}, reason: {produceResult.Message}");
    }
}
