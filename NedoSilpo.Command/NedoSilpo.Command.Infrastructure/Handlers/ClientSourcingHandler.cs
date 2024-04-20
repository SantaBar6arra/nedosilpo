using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using NedoSilpo.Command.Domain.Aggregates;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Infrastructure.Handlers;

public class ClientSourcingHandler : IEventSourcingHandler<ClientAggregate>
{
    private readonly IEventStore _eventStore;

    public ClientSourcingHandler(IEventStore eventStore) => _eventStore = eventStore;

    public async Task<ClientAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new ClientAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId, typeof(ClientAggregate));

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
