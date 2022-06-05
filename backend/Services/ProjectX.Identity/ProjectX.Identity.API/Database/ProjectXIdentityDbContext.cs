using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Identity.API.Database;

public class ProjectXIdentityDbContext : IdentityDbContext
    <UserEntity, RoleEntity, int, IdentityUserClaim<int>, 
     UserRoleEntity, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public const string SchemaName = "ProjectX.Identity";

    public override DbSet<UserEntity> Users { get; set; }
    public override DbSet<RoleEntity> Roles { get; set; }
    public override DbSet<UserRoleEntity> UserRoles { get; set; }

    public ProjectXIdentityDbContext(DbContextOptions<ProjectXIdentityDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(SchemaName);
    }
}

public class UserEntity : IdentityUser<int>
{
    public ICollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}

public class RoleEntity : IdentityRole<int>
{
    public ICollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();
}

public class UserRoleEntity : IdentityUserRole<int>
{
    public UserEntity User { get; private set; }
    public RoleEntity Role { get; private set; }

    public UserRoleEntity()
    {
    }

    public UserRoleEntity(UserEntity user, RoleEntity role)
    {
        User = user;
        Role = role;
        User.UserRoles.Add(this);
        Role.UserRoles.Add(this);
    }
}