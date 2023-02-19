using MediatR;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProjectX.Identity.Domain;

public class RefreshToken
{
    #region Public Properties

    public int Id { get; private set; }

    public required string Token { get; init; }

    public required string JwtId { get; init; }

    public bool IsUsed { get; private set; }

    public bool IsRevoked { get; private set; }

    public required DateTime AddedDate { get; init; }

    public required DateTime ExpiryDate { get; init; }

    public required int UserId { get; init; }

    public required AccountEntity User { get; init; }

    #endregion

    #region Public Methods

    public static RefreshToken Create(AccountEntity account, string jwtId, DateTime issuedAt)
    {
        return new RefreshToken()
        {
            JwtId = jwtId,
            IsUsed = false,
            IsRevoked = false,
            UserId = account.Id,
            User = account,
            AddedDate = issuedAt,
            ExpiryDate = issuedAt.AddMonths(6),
            Token = RandomString(35)
        };
    }

    public ResultOf<Unit> Use(DateTime now, ClaimsPrincipal? tokenInVerification, SecurityToken? token)
    {
        tokenInVerification.ThrowIfNull();
        token.ThrowIfNull();

        if (token is JwtSecurityToken jwtSecurityToken)
        {
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

            if (result == false)
            {
                //throw new SecurityTokenValidationException("The alg must be RS256.");
                return ApplicationError.InvalidData(message: $"Invalid signature algoritm, expected: {SecurityAlgorithms.HmacSha256}, actual: {jwtSecurityToken.Header.Alg}.");
            }
        }

        var utcExpiryDate = long.Parse(tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

        if (expiryDate > now)
        {
            return ApplicationError.InvalidData(message: "Token has not yet expired");
        }

        if (IsUsed)
        {
            return ApplicationError.InvalidData(message: "Token has been used");
        }

        if (IsRevoked)
        {
            return ApplicationError.InvalidData(message: "Token has been revoked");
        }

        var jti = tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        if (JwtId != jti)
        {
            return ApplicationError.InvalidData(message: "Token doesn't match");
        }

        if (ExpiryDate < now)
        {
            return ApplicationError.InvalidData(message: "Refresh token has expired");
        }

        IsUsed = true;

        return ResultOf<Unit>.Unit;
    }

    #endregion

    #region Private Methods

    private static string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTimeVal;
    }

    #endregion
}