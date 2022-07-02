﻿using Microsoft.AspNetCore.Identity;

namespace ProjectX.Identity.API.Database.Models;

public class UserEntity : IdentityUser<int>
{
    public ICollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<RefreshToken> Tokens { get; set; }
}