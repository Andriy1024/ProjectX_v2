using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.Domain;

public class AccountEntity : IdentityUser<int>
{
    public ICollection<AccountRoleEntity> UserRoles { get; private set; } = new List<AccountRoleEntity>();

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<RefreshToken> Tokens { get; set; }
}