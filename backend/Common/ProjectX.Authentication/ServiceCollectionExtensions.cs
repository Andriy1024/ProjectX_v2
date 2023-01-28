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
        services
           .AddOptions<AuthenticationConfig>()
           .BindConfiguration(nameof(AuthenticationConfig))
           .ValidateDataAnnotations()
           .ValidateOnStart();

        var options = configuration.GetSection(nameof(AuthenticationConfig)).Get<AuthenticationConfig>()
            ?? throw new ArgumentNullException(nameof(AuthenticationConfig));

        var secret = Encoding.UTF8.GetBytes(options.Secret);

        var validationParametersFactory = () => new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = true,
            ValidIssuer = options.Issuer,
            ValidateAudience = true,
            ValidAudience = options.Audience,
            RequireExpirationTime = false, //TODO later true
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        return services
            .AddTransient((provider) => validationParametersFactory())
            .AddAuthentication(o =>
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
            })
            .Services;
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