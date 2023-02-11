using System.Text.Json;

namespace ProjectX.Core.Tests.StringEnum;

public class StringEnumTests
{
    [Theory]
    [MemberData(nameof(TestCase))]
    public void StringEnum_Serialization_Works(TestEnum input)
    {
        //Arrange
        var initEnum = new TestEnumEnvelope 
        {
            Enum = input
        };

        //Act
        string json = JsonSerializer.Serialize(initEnum);
        var deserializedEnum = JsonSerializer.Deserialize<TestEnumEnvelope>(json);

        //Assert
        Assert.NotNull(deserializedEnum);
        Assert.True(deserializedEnum!.Enum == initEnum.Enum);
    }

    public static IEnumerable<object[]> TestCase => new List<object[]>
    {
        new object[] { TestEnum.BadData },
        new object[] { TestEnum.InvalidPermission }
    };
}