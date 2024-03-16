using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel eventModel);
    Task<IList<EventModel>> FindEvents(Guid id, Type type);
}
