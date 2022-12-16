using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence.Configurations;

internal class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Task", DashboardDbContext.SchemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}



