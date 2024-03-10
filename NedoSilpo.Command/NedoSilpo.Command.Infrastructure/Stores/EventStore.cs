using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using Cqrs.Core.Producers;
using NedoSilpo.Command.Domain.Aggregates;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }

    public async Task<IList<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

        if (eventStream is null or { Count: 0 })
            throw new AggregateNotFoundException($"incorrect {nameof(aggregateId)} provided!");

        return eventStream
            .OrderBy(evt => evt.Version)
            .Select(evt => evt.EventData)
            .ToList();
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

        if (expectedVersion is not -1 && eventStream[^1].Version != expectedVersion)
            throw new ConcurrencyException();

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;

            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                Version = version,
                EventType = eventType,
                EventData = @event,
                AggregateId = aggregateId,
                AggregateType = nameof(ProductAggregate) // todo: ??? fix it, its an obvious fail
            };

            await _eventStoreRepository.SaveAsync(eventModel);

            // todo look into the mongodb transactions and how to use them
            // we may have success in saving the event to the event store but fail to produce it to the kafka
            // so we may have to rollback the event store
            // so it is better to consider using transactions in mongodb
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? throw new Exception("KAFKA_TOPIC is not set");
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
}
