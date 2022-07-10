using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.API.Database.Models;

public class AccountRoleEntity : IdentityUserRole<int>
{
    public AccountEntity User { get; private set; }
    public RoleEntity Role { get; private set; }

    public AccountRoleEntity()
    {
    }

    public AccountRoleEntity(AccountEntity user, RoleEntity role)
    {
        User = user;
        Role = role;
        User.UserRoles.Add(this);
        Role.UserRoles.Add(this);
    }
}