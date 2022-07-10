using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.API.Database.Models;

public class RoleEntity : IdentityRole<int>
{
    public ICollection<AccountRoleEntity> UserRoles { get; private set; } = new List<AccountRoleEntity>();
}