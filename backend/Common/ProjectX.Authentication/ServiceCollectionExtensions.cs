using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Authentication.Services;
using ProjectX.Core.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProjectX.Authentication;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCurrentUser(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IUserContext, UserContext>();

        return app;
    }

    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder app)
    {
        app.Services.AddAppAuthentication(app.Configuration);
        return app;
    }

    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationConfig>(configuration.GetSection(nameof(AuthenticationConfig)));

        var secret = Encoding.UTF8.GetBytes(configuration["AuthenticationConfig:Secret"]);

        var validationParametersFactory = () => new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = true,
            ValidIssuer = configuration["AuthenticationConfig:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["AuthenticationConfig:Audience"],
            RequireExpirationTime = false, //TODO later true
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        services.AddTransient((provider) => validationParametersFactory());

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(
            //JwtBearerDefaults.AuthenticationScheme, TODO: need to test
            jwt =>
            {
                jwt.SaveToken = true;
                jwt.RequireHttpsMetadata = false;
                jwt.TokenValidationParameters = validationParametersFactory();
            });

        return services;
    }

    public static IApplicationBuilder UseAppAuthentication(this IApplicationBuilder app)
    {
        // https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
        // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier -  sub
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}