using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using NedoSilpo.Command.Domain.Aggregates;

namespace NedoSilpo.Command.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public EventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
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

        foreach (var evt in events)
        {
            version++;
            evt.Version = version;
            var eventType = evt.GetType().Name;

            var eventModel = new EventModel 
            { 
                TimeStamp = DateTime.Now,
                Version = version,
                EventType = eventType,
                EventData = evt,
                AggregateId = aggregateId,
                AggregateType = nameof(ProductAggregate) // todo: ??? fix it, its an obvious fail
            };

            await _eventStoreRepository.SaveAsync(eventModel);
        }
    }
}
