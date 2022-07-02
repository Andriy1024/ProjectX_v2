using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectX.Identity.API.Database.Configurations;
using ProjectX.Identity.API.Database.Models;

namespace ProjectX.Identity.API.Database;

public class ProjectXIdentityDbContext : IdentityDbContext
    <UserEntity, RoleEntity, int, IdentityUserClaim<int>, 
     UserRoleEntity, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public const string SchemaName = "ProjectX.Identity";

    public override DbSet<UserEntity> Users { get; set; }
    public override DbSet<RoleEntity> Roles { get; set; }
    public override DbSet<UserRoleEntity> UserRoles { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public ProjectXIdentityDbContext(DbContextOptions<ProjectXIdentityDbContext> options) 
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



