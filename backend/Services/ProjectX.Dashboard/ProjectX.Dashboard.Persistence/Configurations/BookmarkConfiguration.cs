using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectX.Dashboard.Persistence.Context;

namespace ProjectX.Dashboard.Persistence.Configurations;

internal class BookmarkConfiguration : IEntityTypeConfiguration<BookmarkEntity>
{
    public void Configure(EntityTypeBuilder<BookmarkEntity> builder)
    {
        builder.ToTable("Bookmark", DashboardDbContext.SchemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.URL).IsRequired();
    }
}
