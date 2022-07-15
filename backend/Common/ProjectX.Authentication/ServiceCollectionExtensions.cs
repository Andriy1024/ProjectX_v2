﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProjectX.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectXAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationConfig>(configuration.GetSection(nameof(AuthenticationConfig)));

        var secret = Encoding.UTF8.GetBytes(configuration["AuthenticationConfig:Secret"]);

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = true,
            ValidIssuer = configuration["AuthenticationConfig:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["AuthenticationConfig:Audience"],
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