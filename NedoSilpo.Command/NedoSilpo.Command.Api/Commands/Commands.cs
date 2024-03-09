using Cqrs.Core.Commands;

namespace NedoSilpo.Command.Api.Commands;

public record CreateProduct(string Name, string Description, decimal Price, int QuantityAvailable) : BaseCommand;

public record UpdateProduct(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable) : BaseCommand;

public record SellProduct(Guid Id, int Quantity) : BaseCommand;

public record RemoveProduct(Guid Id) : BaseCommand;
