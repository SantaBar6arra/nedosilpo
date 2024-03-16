using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NedoSilpo.Command.Infrastructure.Config;

namespace NedoSilpo.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    // consider registering MongoDbConfig as a singleton service itself and pass it as a parameter
    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        var (connectionString, databaseName, collection) = config.Value;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _eventStoreCollection = database.GetCollection<EventModel>(collection);
    }

    public async Task<IList<EventModel>> FindEvents(Guid id, Type type)
    {
        return await _eventStoreCollection
            .Find(eventModel => eventModel.AggregateId == id
                                && eventModel.AggregateType == type.Name)
            .ToListAsync()
            .ConfigureAwait(false); // todo find out why the fuck
    }

    public async Task SaveAsync(EventModel eventModel)
    {
        await _eventStoreCollection
            .InsertOneAsync(eventModel)
            .ConfigureAwait(false);
    }
}
