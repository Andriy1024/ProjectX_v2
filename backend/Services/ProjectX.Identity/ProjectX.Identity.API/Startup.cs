using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.AspNetCore.Http;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using Serilog;

namespace ProjectX.Identity.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) 
    {
        var services = app.Services;
        
        var configuration = app.Configuration;

        app.AddProjectXSwagger();

        app.AddObservabilityServices();

        services.AddContexts();

        services.AddControllers();

        services.AddProjectXAuthentication(configuration);

        services.AddDbContext<IdentityXDbContext>(o =>
        {
            o.UseInMemoryDatabase(databaseName: "ProjectX.Identity");
        });

        services
            .AddIdentity<AccountEntity, RoleEntity>(o => { o.User.RequireUniqueEmail = true; })
            .AddRoles<RoleEntity>()
            .AddEntityFrameworkStores<IdentityXDbContext>()
            .AddUserManager<UserManager<AccountEntity>>();

        services.AddTransient<AuthorizationService>();

        services.AddCors(options =>
        {
            options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    public static void Configure(WebApplication app)
    {
        app.UseCors("Open");

        app.UseProjectXSwagger();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseProjectXAuthentication();

        app.UseContexts();

        app.MapControllers();
    }
}