using Cqrs.Core.Handlers;
using NedoSilpo.Command.Domain.Aggregates;
// ReSharper disable once ConvertToPrimaryConstructor

namespace NedoSilpo.Command.Api.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<ProductAggregate> _eventSourcingHandler;
    public CommandHandler(IEventSourcingHandler<ProductAggregate> eventSourcingHandler) =>
        _eventSourcingHandler = eventSourcingHandler;

    public Task HandleAsync(CreateProduct command)
    {
        var product = new ProductAggregate(
            command.Name,
            command.Description,
            command.Price,
            command.QuantityAvailable);
        return _eventSourcingHandler.SaveAsync(product);
    }

    public async Task HandleAsync(UpdateProduct command)
    {
        var product = await _eventSourcingHandler.GetByIdAsync(command.Id);
        product.Update(command.Name, command.Description, command.Price, command.QuantityAvailable);
        await _eventSourcingHandler.SaveAsync(product);
    }

    public async Task HandleAsync(SellProduct command)
    {
        var product = await _eventSourcingHandler.GetByIdAsync(command.Id);
        product.Sell(command.Quantity);
        await _eventSourcingHandler.SaveAsync(product);

    }
    public async Task HandleAsync(RemoveProduct command)
    {
        var product = await _eventSourcingHandler.GetByIdAsync(command.Id);
        product.Remove();
        await _eventSourcingHandler.SaveAsync(product);
    }
}
