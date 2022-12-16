using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.IntegrationTests.Common;

public sealed class TasksApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IServiceScope _scope;

    public IServiceProvider ServiceProvider => _scope.ServiceProvider;

    public TasksApplicationFactory()
        : base()
    {
        _scope = Services
           .GetRequiredService<IServiceScopeFactory>()
           .CreateScope();
    }

    public override ValueTask DisposeAsync()
    {
        _scope.Dispose();

        return base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(s =>
        {
            s.RemoveAll(typeof(DbContextOptions<TasksDbContext>));

            s.AddDbContext<TasksDbContext>(options =>
                options.UseInMemoryDatabase("Testing", root));

            s.AddEntityFrameworkInMemoryDatabase();
        });

        return base.CreateHost(builder);
    }

    public T GetService<T>()
        where T : class
        => ServiceProvider.GetRequiredService<T>();

    public TasksDbContext GetDbContext()
        => GetService<TasksDbContext>();

    public async Task SeedAsync<T>(IEnumerable<T> items)
        where T : class
    {
        using var scope = Services
           .GetRequiredService<IServiceScopeFactory>()
           .CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TasksDbContext>();

        var entity = dbContext.Set<T>();
        await entity.AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }
}