namespace NedoSilpo.Command.Domain.Dtos;

// product
public record CreateProductRequest(string Name, string Description, decimal Price, int QuantityAvailable);
public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable);
public record SellProductRequest(Guid Id, int Quantity);
public record RemoveProductRequest(Guid Id);

// client
public record RegisterClientRequest(string FullName, string Email, string Address, string PhoneNumber);
public record UpdateClientRequest(Guid Id, string FullName, string Email, string Address, string PhoneNumber);
public record DeactivateClientRequest(Guid Id);
