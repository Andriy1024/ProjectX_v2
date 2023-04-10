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
        var token = JwtToken.Builder
            .AddAccount(account)
            .AddSecret(_jwtConfig.Secret)
            .AddExpiryTimeFrame(_jwtConfig.ExpiryTimeFrame)
            .AddIssuer(ProjectXAudience.Identity)
            .AddAudiance(
                ProjectXAudience.Identity,
                ProjectXAudience.Dashboard,
                ProjectXAudience.Realtime,
                ProjectXAudience.FileStorage,
                ProjectXAudience.Messenger)
            .Build();

        await _dbContext.RefreshTokens.AddAsync(token.RefreshToken);
        await _dbContext.SaveChangesAsync();

        return token;
    }

    public async Task<ResultOf<JwtToken>> RefreshTokenAsync(RefreshTokenCommand tokenRequest)
    {
        try
        {
            var now = DateTime.UtcNow;

            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return ApplicationError.InvalidData(message: "Token does not exist.");
            }

            var jwtHandler = new JwtSecurityTokenHandler();

            _tokenValidationParams.ValidateLifetime = false;

            var tokenInVerification = jwtHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

            _tokenValidationParams.ValidateLifetime = true;

            var useResult = storedToken.Use(now, tokenInVerification, validatedToken);

            if (useResult.IsFailed) 
            {
                return useResult.Error!;
            }

            _dbContext.RefreshTokens.Update(storedToken);

            await _dbContext.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

            return await GenerateTokenAsync(dbUser!);
        }
        catch (Exception ex)
        {
            return ApplicationError.ServerError(message: "Something went wrong.");
        }
    }
}