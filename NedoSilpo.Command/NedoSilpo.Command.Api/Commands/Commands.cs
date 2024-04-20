using Cqrs.Core.Commands;

namespace NedoSilpo.Command.Api.Commands;

// product
public record CreateProduct(string Name, string Description, decimal Price, int QuantityAvailable) : BaseCommand;

public record UpdateProduct(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable) : BaseCommand;

public record SellProduct(Guid Id, int Quantity) : BaseCommand;

public record RemoveProduct(Guid Id) : BaseCommand;

// client
public record RegisterClient(string FullName, string Email, string Address, string PhoneNumber) : BaseCommand;

public record UpdateClient(Guid Id, string FullName, string Email, string Address, string PhoneNumber) : BaseCommand;

public record DeactivateClient(Guid Id) : BaseCommand;
