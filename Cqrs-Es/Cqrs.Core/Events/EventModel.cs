using MongoDB.Bson.Serialization.Attributes;

namespace Cqrs.Core.Events;

public class EventModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }    
    public DateTime TimeStamp { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; }
    public int Version { get; set; }
    public string EventType { get; set; }
    public BaseEvent EventData { get; set; }
}