using System.Collections;

namespace ProjectX.Tasks.Tests.Common;

public record TestCase<TPayload>(string Description, bool SuccessCase, TPayload Payload);

public class TestCaseGenerator<TPayload> : IEnumerable<object[]>
    where TPayload : notnull
{
    private readonly List<TestCase<TPayload>> _testCases = new();

    protected void ValidCase(TPayload payload, string? description = null)
    {
        description ??= $"Valid: {payload.GetType().Name}";

        _testCases.Add(new(description, true, payload));
    }

    protected void InvalidCase(string description, TPayload payload)
    {
        _testCases.Add(new(description, false, payload));
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        return _testCases.Select(x => new object[] { x }).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}