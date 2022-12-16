using Microsoft.EntityFrameworkCore;
using ProjectX.Core.StartupTasks;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence;

public sealed class DashboardDatabaseStartup : IStartupTask
{
    private readonly DashboardDbContext _dbContext;

    public DashboardDatabaseStartup(DashboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync();

        if (!await _dbContext.Tasks.AnyAsync())
        {
            _dbContext.Tasks.Add(new TaskEntity("Default task", "Default task created by system"));
        }

        if (!await _dbContext.Notes.AnyAsync())
        {
            _dbContext.Notes.Add(NoteEntity.Create("Default note", "Default note content"));
        }

        if (!await _dbContext.Bookmarks.AnyAsync())
        {
            _dbContext.Bookmarks.Add(BookmarkEntity.Create("Google", "https://www.google.com/"));
        }

        await _dbContext.SaveChangesAsync();
    }
}
