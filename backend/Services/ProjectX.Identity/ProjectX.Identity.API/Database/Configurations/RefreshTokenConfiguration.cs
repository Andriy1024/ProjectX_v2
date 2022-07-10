using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectX.Identity.API.Database.Models;

namespace ProjectX.Identity.API.Database.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken", IdentityXDbContext.SchemaName);

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Tokens)
            .HasForeignKey(e => e.UserId);
    }
}
