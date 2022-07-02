using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.API.Database.Models;

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