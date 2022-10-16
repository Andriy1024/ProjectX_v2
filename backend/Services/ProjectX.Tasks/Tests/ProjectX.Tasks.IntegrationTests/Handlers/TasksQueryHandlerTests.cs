using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
using ProjectX.Tasks.Persistence.Context;
using ProjectX.Tasks.Domain.Entities;

namespace ProjectX.Tasks.IntegrationTests.Handlers;

public class TasksQueryHandlerTests : IClassFixture<TasksApplicationFixture>, IAsyncLifetime
{
    private readonly TasksApplicationFixture _app;
    private IServiceScope _scope;

    public TasksQueryHandlerTests(TasksApplicationFixture app)
    {
        _app = app;
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
    }

    public async Task InitializeAsync()
    {
        _scope = _app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = _scope.ServiceProvider.GetRequiredService<TasksDbContext>();
        dbContext.Tasks.Add(TaskEntity.Create("task_1", "task_1_description"));
        dbContext.SaveChanges();
    }

    [Fact]
    [IntegrationTest]
    public async Task TasksQueryTest() 
    {
        //Arrange
        var handler = _scope.ServiceProvider.GetRequiredService<IRequestHandler<TasksQuery, ResultOf<IEnumerable<TaskContarct>>>>();

        //Act
        var result = await handler.Handle(new TasksQuery(), CancellationToken.None);

        //Assert
        result.Should().MatchSnapshot(nameof(result));
    }
}