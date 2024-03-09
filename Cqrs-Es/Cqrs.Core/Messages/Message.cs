namespace Cqrs.Core.Messages;

public abstract record Message
{
    public Guid Id { get; set; }
}
