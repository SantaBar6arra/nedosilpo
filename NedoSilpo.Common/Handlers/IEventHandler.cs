using NedoSilpo.Common.Events;

namespace NedoSilpo.Common.Handlers;

// todo make it generic, and use accordingly
public interface IEventHandler
{
    Task On(ProductCreated @event);
    Task On(ProductUpdated @event);
    Task On(ProductSold @event);
    Task On(ProductRemoved @event);
}
