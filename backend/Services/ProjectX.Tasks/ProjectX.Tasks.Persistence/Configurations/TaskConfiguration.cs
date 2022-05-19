using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectX.Tasks.Domain.Entities;
using ProjectX.Tasks.Persistence.Context;

namespace ProjectX.Tasks.Persistence.Configurations;

internal class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Task", TasksDbContext.SchemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}