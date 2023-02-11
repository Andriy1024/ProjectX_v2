using ProjectX.Core.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProjectX.Core.Tests.StringEnum;

public class TestEnumConverter : JsonConverter<TestEnum>
{
    public override TestEnum Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
            StringEnum<TestEnum>.FromName(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        TestEnum enumValue,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(enumValue.Name);
}