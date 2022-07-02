using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Swagger;

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

        services.AddDbContext<ProjectXIdentityDbContext>(o =>
        {
            o.UseInMemoryDatabase(databaseName: "ProjectX.Identity");
        });

        services.AddIdentity<UserEntity, RoleEntity>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<RoleEntity>()
        .AddEntityFrameworkStores<ProjectXIdentityDbContext>()
        .AddUserManager<UserManager<UserEntity>>();

        services.AddTransient<JwtService>();

        services.AddCors(options =>
        {
            options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });
    }

    public static void Configure(WebApplication app)
    {
        app.UseProjectXSwagger();

        app.UseHttpsRedirection();

        app.UseProjectXAuthentication();

        app.UseCors("Open");

        app.MapControllers();
    }
}
