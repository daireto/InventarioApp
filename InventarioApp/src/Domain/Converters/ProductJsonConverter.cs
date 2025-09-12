using System.Text.Json;
using System.Text.Json.Serialization;
using InventarioApp.src.Domain.Entities;

public class ProductJsonConverter : JsonConverter<Product>
{
    public override Product? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("Type", out var typeProp))
            throw new JsonException("No se encontró la propiedad 'Type' para deserialización polimórfica.");

        var type = typeProp.GetString();
        return type switch
        {
            nameof(PerishableProduct) => root.Deserialize<PerishableProduct>(options),
            nameof(NonPerishableProduct) => root.Deserialize<NonPerishableProduct>(options),
            _ => throw new NotSupportedException($"Tipo de producto no soportado: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Product value, JsonSerializerOptions options)
    {
        var type = value.GetType().Name;
        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));
        writer.WriteStartObject();
        writer.WriteString("Type", type);
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }
        writer.WriteEndObject();
    }
}
