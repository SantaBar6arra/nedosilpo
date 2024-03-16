namespace Cqrs.Core.Events;

public abstract record BaseEvent
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public int Version { get; set; }

    protected BaseEvent(Guid id, string type)
    {
        Id = id;
        Type = type;
    }
}
