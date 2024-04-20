using Cqrs.Core.Events;
using Cqrs.Core.Infrastructure;
using MongoDB.Bson.Serialization;
using NedoSilpo.Command.Api.Commands;
using NedoSilpo.Command.Infrastructure.Dispatchers;
using NedoSilpo.Common.Events;

namespace NedoSilpo.Command.Api.Helpers;

public static class StartupHelper
{
    public static void RegisterBsonClassMap()
    {
        // todo replace this obvious shit
        BsonClassMap.RegisterClassMap<BaseEvent>();
        BsonClassMap.RegisterClassMap<ProductCreated>();
        BsonClassMap.RegisterClassMap<ProductUpdated>();
        BsonClassMap.RegisterClassMap<ProductSold>();
        BsonClassMap.RegisterClassMap<ProductRemoved>();
        BsonClassMap.RegisterClassMap<ClientRegistered>();
        BsonClassMap.RegisterClassMap<ClientUpdated>();
        BsonClassMap.RegisterClassMap<ClientDeactivated>();
    }

    public static void RegisterCommandHandlers(this WebApplicationBuilder builder)
    {
        var (dispatcher, serviceProvider) = (new CommandDispatcher(), builder.Services.BuildServiceProvider());

        var productCommandHandler = serviceProvider.GetRequiredService<IProductCommandHandler>();
        dispatcher.Register<CreateProduct>(productCommandHandler.HandleAsync);
        dispatcher.Register<UpdateProduct>(productCommandHandler.HandleAsync);
        dispatcher.Register<SellProduct>(productCommandHandler.HandleAsync);
        dispatcher.Register<RemoveProduct>(productCommandHandler.HandleAsync);

        var clientCommandHandler = serviceProvider.GetRequiredService<IClientCommandHandler>();
        dispatcher.Register<RegisterClient>(clientCommandHandler.HandleAsync);
        dispatcher.Register<UpdateClient>(clientCommandHandler.HandleAsync);
        dispatcher.Register<DeactivateClient>(clientCommandHandler.HandleAsync);

        builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);
    }
}
