using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectX.Dashboard.Domain.Entities;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence.Configurations;

internal class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Task", TasksDbContext.SchemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}