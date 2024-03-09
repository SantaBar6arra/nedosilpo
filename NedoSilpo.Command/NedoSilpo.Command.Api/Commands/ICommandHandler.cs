namespace NedoSilpo.Command.Api.Commands;

public interface ICommandHandler
{
    Task HandleAsync(CreateProduct command);
    Task HandleAsync(UpdateProduct command);
    Task HandleAsync(SellProduct command);
    Task HandleAsync(RemoveProduct command);
}
