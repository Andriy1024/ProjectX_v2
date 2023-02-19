using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Authentication;
using ProjectX.Authentication.Constants;
using ProjectX.Identity.Persistence;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectX.Identity.Application.Services;

public sealed class AuthorizationService
{
    private readonly AuthenticationConfig _jwtConfig;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly IdentityDatabase _dbContext;
    private readonly UserManager<AccountEntity> _userManager;

    public AuthorizationService(
        IOptions<AuthenticationConfig> jwtConfig,
        TokenValidationParameters tokenValidationParams,
        IdentityDatabase dbContext,
        UserManager<AccountEntity> userManager)
    {
        _jwtConfig = jwtConfig.Value;
        _tokenValidationParams = tokenValidationParams;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ResultOf<JwtToken>> GenerateTokenAsync(AccountEntity account)
    {
        var tokenEntity = JwtToken.Builder
            .AddAccount(account)
            .AddSecret(_jwtConfig.Secret)
            .AddExpiryTimeFrame(_jwtConfig.ExpiryTimeFrame)
            .AddIssuer(ProjectXAudience.Identity)
            .AddAudiance(
                ProjectXAudience.Identity,
                ProjectXAudience.Dashboard,
                ProjectXAudience.Realtime,
                ProjectXAudience.FileStorage)
            .Build();

        await _dbContext.RefreshTokens.AddAsync(tokenEntity.RefreshToken);
        await _dbContext.SaveChangesAsync();

        return tokenEntity;
    }

    public async Task<ResultOf<JwtToken>> RefreshTokenAsync(RefreshTokenCommand tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        
        var now = DateTime.UtcNow;

        try
        {
            // Validation 1 - Validation JWT token format
            _tokenValidationParams.ValidateLifetime = false;

            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

            _tokenValidationParams.ValidateLifetime = true;

            // Validation 2 - Validate encryption alg
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    //throw new SecurityTokenValidationException("The alg must be RS256.");
                    return ApplicationError.InvalidData(message: $"Invalid signature algoritm, expected: {SecurityAlgorithms.HmacSha256}, actual: {jwtSecurityToken.Header.Alg}.");
                }
            }

            // Validation 3 - validate expiry date
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > now)
            {
                return ApplicationError.InvalidData(message: "Token has not yet expired");
            }

            // validation 4 - validate existence of the token
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return ApplicationError.InvalidData(message: "Token does not exist");
            }

            // Validation 5 - validate if used
            if (storedToken.IsUsed)
            {
                return ApplicationError.InvalidData(message: "Token has been used");
            }

            // Validation 6 - validate if revoked
            if (storedToken.IsRevorked)
            {
                return ApplicationError.InvalidData(message: "Token has been revoked");
            }

            // Validation 7 - validate the id
            var jti = tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return ApplicationError.InvalidData(message: "Token doesn't match");
            }

            // Validation 8 - validate stored token expiry date
            if (storedToken.ExpiryDate < now)
            {
                return ApplicationError.InvalidData(message: "Refresh token has expired");
            }

            // update current token 

            storedToken.IsUsed = true;

            _dbContext.RefreshTokens.Update(storedToken);

            await _dbContext.SaveChangesAsync();

            // Generate a new token
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

            return await GenerateTokenAsync(dbUser!);
        }
        catch (Exception ex)
        {
            return ApplicationError.ServerError(message: "Something went wrong.");
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTimeVal;
    }
}