using ProjectX.Core.Enums;
using System.Text.Json.Serialization;

namespace ProjectX.Core.Tests.StringEnum;

public class TestEnumEnvelope
{
    public TestEnum Enum { get; set; }
}

[JsonConverter(typeof(TestEnumConverter))]
public class TestEnum : StringEnum<TestEnum>
{
    public static readonly TestEnum BadData = new("BadData");

    public static readonly TestEnum InvalidPermission = new("InvalidPermission");

    private TestEnum(string name)
        : base(name)
    {
    }
}