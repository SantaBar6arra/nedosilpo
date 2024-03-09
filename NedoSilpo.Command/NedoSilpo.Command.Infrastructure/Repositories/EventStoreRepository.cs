using System.Text;
using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NedoSilpo.Command.Infrastructure.Config;

namespace NedoSilpo.Command.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        // todo redo when doing a real work
        var (connectionString, databaseName, collection) = config.Value;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _eventStoreCollection = database.GetCollection<EventModel>(collection);
    }

    public async Task<IList<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return await _eventStoreCollection
            .Find(item => item.AggregateId == aggregateId)
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
