using System.Reflection;
using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public abstract class AggregateRoot
{
    protected Guid _id;
    private readonly IList<BaseEvent> _changes = [];

    public Guid Id => _id;
    public int Version { get; set; } = -1;


    public IList<BaseEvent> GetUncommittedChanges() => _changes;
    public void MarkChangesAsCommitted() => _changes.Clear();
    protected void RaiseEvent(BaseEvent ev) => ApplyChange(ev, true);
    public void ReplayEvents(IList<BaseEvent> events)
    {
        foreach (var @event in events)
            ApplyChange(@event, false);
    }

    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic, [@event.GetType()])
            ?? throw new InvalidOperationException("no 'Apply' method found on aggregate");

        method.Invoke(this, [@event]);

        if (isNew)
            _changes.Add(@event);
    }
}
