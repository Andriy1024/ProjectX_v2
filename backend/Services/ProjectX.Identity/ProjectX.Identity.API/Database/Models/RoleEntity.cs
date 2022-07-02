using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.API.Database.Models;

public class RoleEntity : IdentityRole<int>
{
    public ICollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();
}