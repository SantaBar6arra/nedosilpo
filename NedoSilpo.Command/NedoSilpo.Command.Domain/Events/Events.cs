using Cqrs.Core.Events;

namespace NedoSilpo.Command.Domain.Events;

public record ProductCreated(string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(nameof(ProductCreated));

public record ProductUpdated(string Name, string Description, decimal Price, int QuantityAvailable)
    : BaseEvent(nameof(ProductUpdated));

public record ProductSold(int Quantity) : BaseEvent(nameof(ProductSold));

public record ProductRemoved(Guid Id) : BaseEvent(nameof(ProductRemoved));
