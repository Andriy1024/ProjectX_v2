namespace ProjectX.Identity.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken", IdentityDatabase.SchemaName);

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Tokens)
            .HasForeignKey(e => e.UserId);
    }
}