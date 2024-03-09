using Cqrs.Core.Messages;

namespace Cqrs.Core.Events;

public record BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        Type = type;
    }

    public string Type { get; set; }
    public int Version { get; set; }
}
