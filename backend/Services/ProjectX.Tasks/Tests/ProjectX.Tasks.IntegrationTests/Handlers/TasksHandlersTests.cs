using FluentAssertions;
using ProjectX.Tasks.Application;
using ProjectX.Tasks.IntegrationTests.Common;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;
using MediatR;
using System.Collections.Generic;
using ProjectX.Tasks.Application.Contracts;
using ProjectX.Core;
using ProjectX.Tasks.Domain.Entities;
using ProjectX.Tasks.Tests.Common;

namespace ProjectX.Tasks.IntegrationTests.Handlers;

public class TaskIntegrationTests : IAsyncLifetime
{
    private readonly TasksApplicationFactory _app = new();

    public async Task InitializeAsync()
    {
        await _app.SeedAsync(new[]
        {
            TaskEntity.Create("task_1", "task_1_description"),
            TaskEntity.Create("task_2", "task_2_description")
        });
    }

    public async Task DisposeAsync()
    {
        await _app.DisposeAsync();
    }

    [Fact]
    [IntegrationTest]
    public async Task TasksQueryWorksCorrectly() 
    {
        //Arrange
        var handler = _app.GetService<IRequestHandler<TasksQuery, ResultOf<IEnumerable<TaskContarct>>>>();

        //Act
        var result = await handler.Handle(new TasksQuery(), CancellationToken.None);

        //Assert
        result.Should().MatchSnapshot(nameof(result));
    }

    [Fact]
    [IntegrationTest]
    public async Task CreateTaskWorksCorrectly()
    {
        //Arrange
        var handler = _app.GetService<IRequestHandler<CreateTaskCommand, ResultOf<TaskContarct>>>();
        var command = new CreateTaskCommand()
        {
            Name = "task 3",
            Description = "description 3"
        };

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().MatchSnapshot(nameof(result));
    }

    [Fact]
    [IntegrationTest]
    public async Task UpdateTaskWorksCorrectly()
    {
        //Arrange
        var handler = _app.GetService<IRequestHandler<UpdateTaskCommand, ResultOf<TaskContarct>>>();
        
        var command = new UpdateTaskCommand()
        {
            Id = 2,
            Name = "task updated",
            Description = "task_2_description"
        };

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().MatchSnapshot(nameof(result));
    }
}