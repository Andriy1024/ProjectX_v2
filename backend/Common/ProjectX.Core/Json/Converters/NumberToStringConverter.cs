﻿using System.Buffers.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Buffers;

namespace ProjectX.Core.Json.Converters;

public class LongToStringConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number;

            if (long.TryParse(reader.GetString(), out number))
                return number;
        }

        return reader.GetInt64();
    }

    public override void Write(Utf8JsonWriter writer, long longValue, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(longValue);
    }
}

public class IntToStringConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number;

            if (int.TryParse(reader.GetString(), out number))
                return number;
        }

        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int longValue, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(longValue);
    }
}

public class DecimalToStringConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out decimal number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number;

            if (decimal.TryParse(reader.GetString(), out number))
                return number;
        }

        return reader.GetDecimal();
    }

    public override void Write(Utf8JsonWriter writer, decimal decimalValue, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(decimalValue);
    }
}

public class DoubleToStringConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
            if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed) && span.Length == bytesConsumed)
                return number;

            if (double.TryParse(reader.GetString(), out number))
                return number;
        }

        return reader.GetDouble();
    }

    public override void Write(Utf8JsonWriter writer, double longValue, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(longValue);
    }
}