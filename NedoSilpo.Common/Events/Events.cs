using Cqrs.Core.Events;

namespace NedoSilpo.Common.Events;

public record ProductCreated(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(Id, nameof(ProductCreated));

public record ProductUpdated(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(Id, nameof(ProductUpdated));

public record ProductSold(Guid Id, int Quantity) : BaseEvent(Id, nameof(ProductSold));

public record ProductRemoved(Guid Id) : BaseEvent(Id, nameof(ProductRemoved));

public record ClientRegistered(Guid Id, string FullName, string Email, string Address, string PhoneNumber)
    : BaseEvent(Id, nameof(ClientRegistered));

public record ClientUpdated(Guid Id, string FullName, string Email, string Address, string PhoneNumber)
    : BaseEvent(Id, nameof(ClientUpdated));

public record ClientDeactivated(Guid Id) : BaseEvent(Id, nameof(ClientDeactivated));
