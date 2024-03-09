using Cqrs.Core.Handlers;
using NedoSilpo.Command.Domain.Aggregates;

namespace NedoSilpo.Command.Api.Commands;

public class CommandHandler(IEventSourcingHandler<ProductAggregate> eventSourcingHandler)
    : ICommandHandler
{
    // we cant have a private readonly field here cause primary ctor does not support readonly fields
    private IEventSourcingHandler<ProductAggregate> EventSourcingHandler { get; } = eventSourcingHandler;

    public Task HandleAsync(CreateProduct command)
    {
        var product = new ProductAggregate(
            command.Id,
            command.Name,
            command.Description,
            command.Price,
            command.QuantityAvailable);
        return EventSourcingHandler.SaveAsync(product);
    }

    public async Task HandleAsync(UpdateProduct command)
    {
        var product = await EventSourcingHandler.GetByIdAsync(command.Id);
        product.Update(command.Name, command.Description, command.Price, command.QuantityAvailable);
        await EventSourcingHandler.SaveAsync(product);
    }

    public async Task HandleAsync(SellProduct command)
    {
        var product = await EventSourcingHandler.GetByIdAsync(command.Id);
        product.Sell(command.Quantity);
        await EventSourcingHandler.SaveAsync(product);
    }

    public async Task HandleAsync(RemoveProduct command)
    {
        var product = await EventSourcingHandler.GetByIdAsync(command.Id);
        product.Remove();
        await EventSourcingHandler.SaveAsync(product);
    }
}
