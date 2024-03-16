using Cqrs.Core.Domain;
using Cqrs.Core.Events;

namespace Cqrs.Core.Infrastructure;

public interface IEventStore
{
    Task SaveEventsAsync(AggregateRoot aggregate, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<IList<BaseEvent>> GetEventsAsync(Guid aggregateId, Type aggregateType);
}
