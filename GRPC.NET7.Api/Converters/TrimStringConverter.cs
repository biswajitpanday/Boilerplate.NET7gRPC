using System.Text.Json;

namespace GRPC.NET7.Api.Converters;

public class TrimStringConverter : System.Text.Json.Serialization.JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.Trim();

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}