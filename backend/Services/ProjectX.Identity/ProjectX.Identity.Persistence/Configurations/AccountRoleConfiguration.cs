namespace ProjectX.Identity.Persistence.Configurations;

internal class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRoleEntity>
{
    public void Configure(EntityTypeBuilder<AccountRoleEntity> builder)
    {
        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.HasOne(ur => ur.User)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}