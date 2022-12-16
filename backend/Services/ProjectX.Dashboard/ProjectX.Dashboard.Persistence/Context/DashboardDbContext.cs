using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProjectX.Dashboard.Persistence.Context;

public class DashboardDbContext : DbContext
{
    public const string SchemaName = "ProjectX.Dashboard";

    public DbSet<TaskEntity> Tasks { get; set; }

    public DbSet<NoteEntity> Notes { get; set; }
    
    public DbSet<BookmarkEntity> Bookmarks { get; set; }

    public DashboardDbContext(DbContextOptions<DashboardDbContext> options) 
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