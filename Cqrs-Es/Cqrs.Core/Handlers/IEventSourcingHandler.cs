using Cqrs.Core.Domain;

namespace Cqrs.Core.Handlers;

public interface IEventSourcingHandler<T> // todo consider "where" T is aggregateRoot or what
{
    Task SaveAsync(AggregateRoot root);
    Task<T> GetByIdAsync(Guid id);
}