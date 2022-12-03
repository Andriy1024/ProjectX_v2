using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Tasks.Persistence.Context;

public class TasksDbContextFactory : IDesignTimeDbContextFactory<TasksDbContext>
{
    public TasksDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TasksDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectX.Tasks;Username=postgres;Password=root");

        return new TasksDbContext(optionsBuilder.Options);
    }
}