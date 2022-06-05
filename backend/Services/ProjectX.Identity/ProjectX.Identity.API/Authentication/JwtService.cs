using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.Authentication.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectX.Identity.API.Authentication;

public class JwtService
{
    private readonly JwtConfig _jwtConfig;

    public JwtService(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public string GenerateJwtToken(UserEntity user)
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
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by the refresh token
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtHandler.CreateToken(tokenDescriptor);

        var jwtToken = jwtHandler.WriteToken(token);

        return jwtToken;
    }
}