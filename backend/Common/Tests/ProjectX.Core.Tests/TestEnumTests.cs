using ProjectX.Core.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectX.Core.Tests;

public class TestEnumTests
{
    [Fact]
    public void TestEnumSerialization()
    {
        var error = new TestClass(TestEnum.InvalidPermission);

        string jsonString = JsonSerializer.Serialize(error);

        var deserialisedError = JsonSerializer.Deserialize<TestClass>(jsonString);

        Assert.True(deserialisedError.Type == TestEnum.InvalidPermission);
    }
}

public class TestClass
{
    public TestEnum Type { get; set; }

    public TestClass(TestEnum type)
    {
        Type = type;
    }
}

[JsonConverter(typeof(TestEnumConverter))]
public class TestEnum : StringEnum<TestEnum>
{
    public static readonly TestEnum BadData  = new TestEnum("BadData");
    
    public static readonly TestEnum InvalidPermission = new TestEnum("InvalidPermission");

    private TestEnum(string name) 
        : base(name)
    {
    }
}

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