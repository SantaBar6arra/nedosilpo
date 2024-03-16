using System.Text.Json;
using System.Text.Json.Serialization;
using Cqrs.Core.Events;
using NedoSilpo.Common.Events;

namespace NedoSilpo.Query.Infrastructure.Converters;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
    }

    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var document))
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");

        if (!document.RootElement.TryGetProperty("Type", out var type))
            throw new JsonException("Could not detect the Type discriminator property!");

        var typeDiscriminator = type.GetString();
        var json = document.RootElement.GetRawText();

        // todo redo, its disgusting
        return typeDiscriminator switch
        {
            nameof(ProductCreated) => JsonSerializer.Deserialize<ProductCreated>(json, options),
            nameof(ProductUpdated) => JsonSerializer.Deserialize<ProductUpdated>(json, options),
            nameof(ProductSold) => JsonSerializer.Deserialize<ProductSold>(json, options),
            nameof(ProductRemoved) => JsonSerializer.Deserialize<ProductRemoved>(json, options),
            _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
