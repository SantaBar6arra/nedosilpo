using System.Reflection;
using NedoSilpo.Common.Handlers;

namespace NedoSilpo.Query.Api.Extensions;

public static class StartupExtensions
{
    public static void RegisterEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var eventHandlers = assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(interfaceType => interfaceType.IsGenericType
                                      && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        foreach (var eventHandler in eventHandlers)
        {
            services.Add(new ServiceDescriptor(
                typeof(IEventHandler),
                eventHandler,
                ServiceLifetime.Scoped));
        }
    }
}
