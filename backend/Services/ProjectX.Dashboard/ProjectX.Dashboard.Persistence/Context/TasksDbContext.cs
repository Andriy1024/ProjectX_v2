using Microsoft.EntityFrameworkCore;
using ProjectX.Dashboard.Domain.Entities;
using System.Reflection;

namespace ProjectX.Dashboard.Persistence.Context;

public class TasksDbContext : DbContext
{
    public const string SchemaName = "ProjectX.Tasks";

    public DbSet<TaskEntity> Tasks { get; set; }

    public TasksDbContext(DbContextOptions<TasksDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(SchemaName);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}