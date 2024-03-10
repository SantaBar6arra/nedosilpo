using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public abstract class AggregateRoot
{
    private readonly IList<BaseEvent> _changes = [];

    public Guid Id { get; protected set; } = Guid.NewGuid();
    public int Version { get; set; } = -1;


    public IList<BaseEvent> GetUncommittedChanges() => _changes;
    public void MarkChangesAsCommitted() => _changes.Clear();
    protected void RaiseEvent(BaseEvent ev) => ApplyChange(ev, true);
    public void ReplayEvents(IList<BaseEvent> events)
    {
        foreach (var evt in events)
            ApplyChange(evt, false);
    }

    private void ApplyChange(BaseEvent evt, bool isNew)
    {
        var method = GetType().GetMethod("Apply", [evt.GetType()])
            ?? throw new InvalidOperationException("no 'Apply' method found on aggregate");

        method.Invoke(this, [evt.GetType()]);

        if (isNew)
            _changes.Add(evt);
    }
}
