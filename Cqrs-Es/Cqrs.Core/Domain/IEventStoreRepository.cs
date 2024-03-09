using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel eventModel);
    Task<IList<EventModel>> FindByAggregateId(Guid aggregateId);
}