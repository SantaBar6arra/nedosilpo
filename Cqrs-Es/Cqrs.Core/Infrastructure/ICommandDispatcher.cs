using Cqrs.Core.Commands;

namespace Cqrs.Core.Infrastructure;

public interface ICommandDispatcher
{
    void Register<T>(Func<T, Task> handler) where T : BaseCommand;
    Task SendAsync(BaseCommand command);
}