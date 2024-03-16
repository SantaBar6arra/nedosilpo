namespace NedoSilpo.Command.Domain.Dtos;

public record CreateProductRequest(string Name, string Description, decimal Price, int QuantityAvailable);
public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable);
public record SellProductRequest(Guid Id, int Quantity);
public record RemoveProductRequest(Guid Id);
