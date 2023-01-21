using System.Text.Json;

namespace ProjectX.Core;

public class EnumerationConverter<T> : System.Text.Json.Serialization.JsonConverter<T>
    where T : Enumeration
{
    public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Enumeration.FindValue<T>(value);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}

public class EnumerationNullableConverter<T> : System.Text.Json.Serialization.JsonConverter<T>
       where T : Enumeration
{
    public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null)
            return null;

        return Enumeration.FindValue<T>(value);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Value);
    }
}
