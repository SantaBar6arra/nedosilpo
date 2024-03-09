using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using NedoSilpo.Command.Domain.Aggregates;

namespace NedoSilpo.Command.Infrastructure.Handlers;

public class EventSourcingHandler(IEventStore eventStore) : IEventSourcingHandler<ProductAggregate> // todo specify name
{
    private IEventStore EventStore { get; } = eventStore;

    public async Task<ProductAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new ProductAggregate();
        var events = await EventStore.GetEventsAsync(aggregateId);

        if (events is null or { Count: 0 })
            return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(evt => evt.Version).Max();

        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await EventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
