using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using NedoSilpo.Command.Domain.Aggregates;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Infrastructure.Handlers;

public class EventSourcingHandler : IEventSourcingHandler<ProductAggregate> // todo specify name
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore) => _eventStore = eventStore;

    public async Task<ProductAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new ProductAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId);

        if (events is null or { Count: 0 })
            return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(evt => evt.Version).Max();

        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
