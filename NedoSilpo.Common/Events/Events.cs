using Cqrs.Core.Events;

namespace NedoSilpo.Common.Events;

public record ProductCreated(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(nameof(ProductCreated));

public record ProductUpdated(Guid Id, string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(nameof(ProductUpdated));

public record ProductSold(Guid Id, int Quantity) : BaseEvent(nameof(ProductSold));

public record ProductRemoved(Guid Id) : BaseEvent(nameof(ProductRemoved));
