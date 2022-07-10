using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Core;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;
using ProjectX.Identity.API.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectX.Identity.API.Authentication;

public class AuthorizationService
{
    private readonly JwtConfig _jwtConfig;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly IdentityXDbContext _dbContext;
    private readonly UserManager<AccountEntity> _userManager;

    public AuthorizationService(
        IOptions<JwtConfig> jwtConfig,
        TokenValidationParameters tokenValidationParams,
        IdentityXDbContext dbContext,
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

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret)),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by the refresh token
            }),
            Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
            SigningCredentials = credentials
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
                    return Error.InvalidData(message: "Invalid signature algoritm");
                }
            }

            // Validation 3 - validate expiry date
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return Error.InvalidData(message: "Token has not yet expired");
            }

            // validation 4 - validate existence of the token
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return Error.InvalidData(message: "Token does not exist");
            }

            // Validation 5 - validate if used
            if (storedToken.IsUsed)
            {
                return Error.InvalidData(message: "Token has been used");
            }

            // Validation 6 - validate if revoked
            if (storedToken.IsRevorked)
            {
                return Error.InvalidData(message: "Token has been revoked");
            }

            // Validation 7 - validate the id
            var jti = tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return Error.InvalidData(message: "Token doesn't match");
            }

            // Validation 8 - validate stored token expiry date
            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Error.InvalidData(message: "Refresh token has expired");
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
                return Error.ServerError(message: "Token has expired please re-login");
            }
            else
            {
                return Error.ServerError(message: "Something went wrong.");
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