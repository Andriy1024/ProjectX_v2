using FluentAssertions;
using ProjectX.Core.Validation;
using ProjectX.Tasks.Application;
using ProjectX.Tasks.Tests.Common;
using Xunit;

namespace ProjectX.Tasks.UnitTests;

public class RequestValidationTests
{
    [Theory]
    [ClassData(typeof(RequestValidationCases))]
    public void RequestValidatesCorrectly(TestCase<IValidatable> testCase)
    {
        // Act
        var act = () => testCase.Payload.Validate().ThrowIfInvalid();

        // Assert
        if (testCase.SuccessCase)
        {
            act.Should().NotThrow();
        }
        else
        {
            act.Should().Throw<FluentValidation.ValidationException>();
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

            ValidCase(new UpdateTaskCommand() 
            {
                Id = 1,
                Name = "Updated name"
            });

            InvalidCase("Invalid Id", new UpdateTaskCommand()
            {
                Id = 0,
                Name = "Updated name"
            });

            InvalidCase("Empty Name", new UpdateTaskCommand()
            {
                Id = 1,
                Name = " "
            });

            ValidCase(new DeleteTaskCommand()
            {
                Id = 1
            });

            InvalidCase("Invalid Id", new DeleteTaskCommand()
            {
                Id = 0
            });
        }
    }
}