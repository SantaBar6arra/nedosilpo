using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using NedoSilpo.Command.Domain.Aggregates;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Infrastructure.Handlers;

public class ProductSourcingHandler : IEventSourcingHandler<ProductAggregate>
{
    private readonly IEventStore _eventStore;

    public ProductSourcingHandler(IEventStore eventStore) => _eventStore = eventStore;

    public async Task<ProductAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new ProductAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId, typeof(ProductAggregate));

        if (events is null or { Count: 0 })
            return aggregate;

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(evt => evt.Version).Max();

        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
