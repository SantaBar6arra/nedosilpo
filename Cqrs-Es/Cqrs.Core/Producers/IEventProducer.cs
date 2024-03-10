using Cqrs.Core.Events;

namespace Cqrs.Core.Producers;

public interface IEventProducer
{
    Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}
