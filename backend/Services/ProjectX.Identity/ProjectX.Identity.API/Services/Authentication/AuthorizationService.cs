using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Authentication;
using ProjectX.Authentication.Constants;
using ProjectX.Core;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;
using ProjectX.Identity.API.Results;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectX.Identity.API.Authentication;

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

    public async Task<ResultOf<AuthResult>> GenerateTokenAsync(AccountEntity user)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        /*
         * At Auth0 we allow signing of tokens using either a symmetric algorithm (HS256), 
         * or an asymmetric algorithm (RS256).RS256: 
         * <see href="https://www.jerriepelser.com/blog/manually-validating-rs256-jwt-dotnet/"/>
         * <see href="https://developer.okta.com/code/dotnet/jwt-validation/"/>
         * HS256 tokens are signed and verified using a simple secret, 
         * where as RS256 use a private and public key for signing and verifying the token signatures.
         * SHA-256 it's Hashing function. This means that if we take our Header and Payload and run it through this function,
         * no one will be able to get the data back again just by looking at the output.
         * Hashing is not encryption: encryption by definition is a reversible action - we do need to get back the original input from the encrypted output.
        */
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = ProjectXAudience.Identity,
            //Audience = _jwtConfig.Audience,
            Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Aud, ProjectXAudience.Identity),
                new(JwtRegisteredClaimNames.Aud, ProjectXAudience.Dashboard),
                new(JwtRegisteredClaimNames.Aud, ProjectXAudience.Realtime),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by the refresh token
            }
            //,JwtBearerDefaults.AuthenticationScheme //TODO: test it
            )
        };

        var token = jwtHandler.CreateToken(tokenDescriptor);

        var jwtToken = jwtHandler.WriteToken(token);

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevorked = false,
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            Token = RandomString(35) + Guid.NewGuid()
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return AuthResult.Success(jwtToken, refreshToken.Token);
    }

    public async Task<ResultOf<AuthResult>> RefreshTokenAsync(RefreshTokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

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

            if (expiryDate > DateTime.UtcNow)
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
            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return ApplicationError.InvalidData(message: "Refresh token has expired");
            }

            // update current token 

            storedToken.IsUsed = true;
            
            _dbContext.RefreshTokens.Update(storedToken);
            
            await _dbContext.SaveChangesAsync();

            // Generate a new token
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            
            return await GenerateTokenAsync(dbUser);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
            {
                return ApplicationError.ServerError(message: "Token has expired please re-login");
            }
            else
            {
                return ApplicationError.ServerError(message: "Something went wrong.");
            }
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTimeVal;
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
}