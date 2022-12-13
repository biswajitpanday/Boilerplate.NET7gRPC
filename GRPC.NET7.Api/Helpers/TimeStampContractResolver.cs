using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GRPC.NET7.Api.Helpers;

public class TimeStampContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.PropertyType == typeof(Google.Protobuf.WellKnownTypes.Timestamp))
        {
            property.Converter = new TimeStampConverter();
        }
        return property;
    }
}

public class TimeStampConverter : DateTimeConverterBase
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        var date = DateTime.Parse(reader.Value?.ToString());
        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        return Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(date);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((Google.Protobuf.WellKnownTypes.Timestamp)value).ToString());
    }
}