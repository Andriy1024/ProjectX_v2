using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.Domain;

public class RoleEntity : IdentityRole<int>
{
    public ICollection<AccountRoleEntity> UserRoles { get; private set; } = new List<AccountRoleEntity>();
}