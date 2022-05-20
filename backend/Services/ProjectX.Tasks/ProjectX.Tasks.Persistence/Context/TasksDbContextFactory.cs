using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Tasks.Persistence.Context;

internal class TasksDbContextFactory : IDesignTimeDbContextFactory<TasksDbContext>
{
    public TasksDbContext CreateDbContext(string[] args)
    {
        throw new NotImplementedException();
    }
}
