using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence.Configurations;

internal class NoteConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        builder.ToTable("Note", DashboardDbContext.SchemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired();
    }
}
