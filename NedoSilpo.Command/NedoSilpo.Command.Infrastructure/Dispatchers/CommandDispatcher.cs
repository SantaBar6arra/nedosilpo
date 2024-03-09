using Cqrs.Core.Commands;
using Cqrs.Core.Infrastructure;

namespace NedoSilpo.Command.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = [];

    public void Register<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T))) 
            throw new InvalidOperationException("cannot register the same handler twice");

        _handlers.Add(typeof(T), command => handler((T)command));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (!_handlers.TryGetValue(command.GetType(), out var handler))
            throw new InvalidOperationException("command is not registered");      
        
        await handler(command);
    }
}