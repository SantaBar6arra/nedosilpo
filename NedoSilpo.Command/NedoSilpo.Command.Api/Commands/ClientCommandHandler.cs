using Cqrs.Core.Handlers;
using NedoSilpo.Command.Domain.Aggregates;

namespace NedoSilpo.Command.Api.Commands;

// todo consider creating a base command handler which will have a method to handle all commands
public interface IClientCommandHandler
{
    Task HandleAsync(RegisterClient command);
    Task HandleAsync(UpdateClient command);
    Task HandleAsync(DeactivateClient command);
}

public class ClientCommandHandler : IClientCommandHandler
{
    private readonly IEventSourcingHandler<ClientAggregate> _eventSourcingHandler;

    public ClientCommandHandler(IEventSourcingHandler<ClientAggregate> eventSourcingHandler) =>
        _eventSourcingHandler = eventSourcingHandler;

    public Task HandleAsync(RegisterClient command)
    {
        var client = new ClientAggregate(
            command.FullName,
            command.Email,
            command.Address,
            command.PhoneNumber);
        return _eventSourcingHandler.SaveAsync(client);
    }

    public async Task HandleAsync(UpdateClient command)
    {
        var client = await _eventSourcingHandler.GetByIdAsync(command.Id);
        client.Update(command.FullName, command.Email, command.Address, command.PhoneNumber);
        await _eventSourcingHandler.SaveAsync(client);
    }

    public async Task HandleAsync(DeactivateClient command)
    {
        var client = await _eventSourcingHandler.GetByIdAsync(command.Id);
        client.Deactivate();
        await _eventSourcingHandler.SaveAsync(client);
    }
}
