using ProjectX.Identity.Persistence.Configurations;

namespace ProjectX.Identity.Persistence;

public class IdentityDatabase : IdentityDbContext
    <AccountEntity, RoleEntity, int, IdentityUserClaim<int>, 
     AccountRoleEntity, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public const string SchemaName = "ProjectX.Identity";

    public override DbSet<AccountEntity> Users { get; set; }
    public override DbSet<RoleEntity> Roles { get; set; }
    public override DbSet<AccountRoleEntity> UserRoles { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityDatabase(DbContextOptions<IdentityDatabase> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(SchemaName);

        builder.ApplyConfigurationsFromAssembly(typeof(RefreshTokenConfiguration).Assembly);
    }
}