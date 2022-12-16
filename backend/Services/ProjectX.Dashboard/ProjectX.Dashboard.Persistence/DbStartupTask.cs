using Microsoft.EntityFrameworkCore;
using ProjectX.Core.StartupTasks;
using ProjectX.Dashboard.Domain.Entities;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence;

public sealed class DbStartupTask : IStartupTask
{
    private readonly TasksDbContext _dbContext;

    public DbStartupTask(TasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync();

        if (!await _dbContext.Tasks.AnyAsync())
        {
            _dbContext.Tasks.Add(new TaskEntity("Default task", "Default task created by system"));

            await _dbContext.SaveChangesAsync();
        }
    }
}
