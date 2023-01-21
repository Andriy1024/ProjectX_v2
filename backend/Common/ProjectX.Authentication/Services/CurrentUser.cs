using Microsoft.AspNetCore.Http;
using ProjectX.Authentication.Extensions;
using ProjectX.Core.Auth;

namespace ProjectX.Authentication.Services;

public class CurrentUser : ICurrentUser
{
    private int? _identityId;
    private string _identityRole;

    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUser(IHttpContextAccessor contextAccessor) 
        => _contextAccessor = contextAccessor;

    public int IdentityId
    {
        get
        {
            if (!_identityId.HasValue)
                _identityId = _contextAccessor.HttpContext.User.GetIdentityId();

            return _identityId.Value;
        }
    }

    public string IdentityRole
    {
        get
        {
            if (string.IsNullOrEmpty(_identityRole))
                _identityRole = _contextAccessor.HttpContext.User.GetIdentityRole();

            return _identityRole;
        }
    }
}
