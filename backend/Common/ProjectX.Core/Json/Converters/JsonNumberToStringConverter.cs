using System.Buffers.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Buffers;

namespace ProjectX.Core.Json.Converters;

public class JsonNumberToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number.ToString();

            if (long.TryParse(reader.GetString(), out number))
                return number.ToString();
        }

        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string longValue, JsonSerializerOptions options)
    {
        writer.WriteStringValue(longValue);
    }
}

public class JsonDoubleToStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number.ToString();

            if (double.TryParse(reader.GetString(), out number))
                return number.ToString();
        }

        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string longValue, JsonSerializerOptions options)
    {
        writer.WriteStringValue(longValue);
    }
}