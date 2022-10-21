using FluentAssertions;
using ProjectX.Core.Validation;
using ProjectX.Tasks.Application;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ProjectX.Tasks.UnitTests;

public class RequestValidationTests
{
    [Theory]
    [ClassData(typeof(RequestValidationCases))]
    public void RequestValidatesCorrectly(TestCase<IValidatable> testCase)
    {
        // Act
        var act = () => testCase.Payload.Validate();

        // Assert
        if (testCase.SuccessCase)
        {
            act.Should().NotThrow();
        }
        else
        {
            act.Should().Throw<InvalidDataException>();
        }
    }

    public class RequestValidationCases : TestCaseGenerator<IValidatable> 
    {
        public RequestValidationCases()
        {
            ValidCase(new CreateTaskCommand()
            {
                Name = "Task 1"
            });

            InvalidCase("Empty task name", new CreateTaskCommand() 
            {
                Name = ""
            });

            //TODO: tests for the rest requests
        }
    }
}

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