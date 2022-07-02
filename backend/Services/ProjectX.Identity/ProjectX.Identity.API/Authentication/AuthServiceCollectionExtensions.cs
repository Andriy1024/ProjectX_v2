using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProjectX.Identity.API.Authentication;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddProjectXAuthentication(this IServiceCollection services, IConfiguration configuration) 
    {
        services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)));

        var secret = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = false,   //TODO later true
            ValidateAudience = false, //TODO later true
            RequireExpirationTime = false, //TODO later true
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }

    public static void UseProjectXAuthentication(this WebApplication app) 
    {
        app.UseAuthentication();

        app.UseAuthorization();
    }
}
