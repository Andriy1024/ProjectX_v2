namespace ProjectX.Core;

public enum ErrorCode
{
    ServerError = 1,
    BadData = 2,
    InvalidPermission = 3,
    NotFound = 4,
    InvalidOperation = 5,

    NoIdentityIdInAccessToken = 6,
    NoIdentityRoleInAccessToken =7,
}
