using System.Text.Json;
using System.Text.Json.Serialization;
using Cqrs.Core.Events;
using JsonException = ThirdParty.Json.LitJson.JsonException;

namespace NedoSilpo.Query.Infrastructure.Converters;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(BaseEvent).IsAssignableFrom(typeToConvert);
    }

    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var document))
            throw new JsonException($"Could not parse event of type {typeToConvert.Name} from JSON");

        if (document.RootElement.GetProperty("Type").GetString() is null)
            throw new JsonException($"Could not find 'Type' property in event of type {typeToConvert.Name}");

        var json = document.RootElement.GetRawText();
        var @event = JsonSerializer.Deserialize(json, typeToConvert, options) as BaseEvent // ?
            ?? throw new JsonException($"Could not deserialize event of type {typeToConvert.Name} from JSON");

        return @event;
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
