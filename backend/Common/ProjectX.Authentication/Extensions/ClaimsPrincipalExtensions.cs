using ProjectX.Core;
using ProjectX.Core.Errors.Exceptions;
using System.Security.Claims;

namespace ProjectX.Authentication.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetIdentityId(this ClaimsPrincipal user)
    {
        var stringId = user.Claims.FirstOrDefault(c => c.Type == ApplicationClaimTypes.IdentityId)?.Value;

        if (!string.IsNullOrEmpty(stringId) && int.TryParse(stringId, out int value))
        {
            return value;
        }

        throw new BadDataException(ErrorCode.NoIdentityIdInAccessToken);
    }

    public static string GetIdentityRole(this ClaimsPrincipal user)
        => user.Claims.FirstOrDefault(c => c.Type == ApplicationClaimTypes.IdentityRole)?.Value
               ?? throw new BadDataException(ErrorCode.NoIdentityRoleInAccessToken);
}
