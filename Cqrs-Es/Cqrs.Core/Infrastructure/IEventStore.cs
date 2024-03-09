using Cqrs.Core.Events;

namespace Cqrs.Core.Infrastructure;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<IList<BaseEvent>> GetEventsAsync(Guid aggregateId);
}
