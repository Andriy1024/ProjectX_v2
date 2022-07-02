using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;
using ProjectX.Identity.API.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectX.Identity.API.Authentication;

public class JwtService
{
    private readonly JwtConfig _jwtConfig;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly ProjectXIdentityDbContext _dbContext;
    private readonly UserManager<UserEntity> _userManager;

    public JwtService(
        IOptions<JwtConfig> jwtConfig,
        TokenValidationParameters tokenValidationParams,
        ProjectXIdentityDbContext dbContext,
        UserManager<UserEntity> userManager)
    {
        _jwtConfig = jwtConfig.Value;
        _tokenValidationParams = tokenValidationParams;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<AuthResult> GenerateJwtToken(UserEntity user)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("Id", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by the refresh token
            }),
            Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
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

    public async Task<AuthResult> VerifyAndGenerateToken(RefreshTokenRequest tokenRequest)
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
                    return AuthResult.Failed("Invalid signature algoritm");
                }
            }

            // Validation 3 - validate expiry date
            var utcExpiryDate = long.Parse(tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return AuthResult.Failed("Token has not yet expired");
            }

            // validation 4 - validate existence of the token
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken == null)
            {
                return AuthResult.Failed("Token does not exist");
            }

            // Validation 5 - validate if used
            if (storedToken.IsUsed)
            {
                return AuthResult.Failed("Token has been used");
            }

            // Validation 6 - validate if revoked
            if (storedToken.IsRevorked)
            {
                return AuthResult.Failed("Token has been revoked");
            }

            // Validation 7 - validate the id
            var jti = tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return AuthResult.Failed("Token doesn't match");
            }

            // Validation 8 - validate stored token expiry date
            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return AuthResult.Failed("Refresh token has expired");
            }

            // update current token 

            storedToken.IsUsed = true;
            
            _dbContext.RefreshTokens.Update(storedToken);
            
            await _dbContext.SaveChangesAsync();

            // Generate a new token
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            
            return await GenerateJwtToken(dbUser);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
            {
                return AuthResult.Failed("Token has expired please re-login");
            }
            else
            {
                return AuthResult.Failed("Something went wrong.");
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