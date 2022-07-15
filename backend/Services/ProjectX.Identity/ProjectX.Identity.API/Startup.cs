using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.AspNetCore.Http;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;

namespace ProjectX.Identity.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) 
    {
        var services = app.Services;
        var configuration = app.Configuration;

        services.AddControllers();
        
        services.AddProjectXSwagger();

        services.AddProjectXAuthentication(configuration);

        services.AddDbContext<IdentityXDbContext>(o =>
        {
            o.UseInMemoryDatabase(databaseName: "ProjectX.Identity");
        });

        services.AddIdentity<AccountEntity, RoleEntity>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
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
        app.UseProjectXSwagger();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseProjectXAuthentication();

        app.UseCors("Open");

        app.MapControllers();
    }
}
