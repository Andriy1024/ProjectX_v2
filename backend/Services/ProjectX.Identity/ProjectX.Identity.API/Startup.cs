using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.Core.StartupTasks;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Persistence.Abstractions;
using ProjectX.Persistence.Extensions;
using System.Reflection;

namespace ProjectX.Identity.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) => app
        .AddCoreServices(Assembly.GetAssembly(typeof(Startup))!)
        .AddObservabilityServices()
        .ConfigureAspNetCore()
        .AddProjectXSwagger()
        .AddAppAuthentication()
        .AddCurrentUser()
        .Services
        .AddDbServices<IdentityDatabase>((p, o) =>
        {
            o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(), 
                x => x.MigrationsHistoryTable("MigrationsHistory", IdentityDatabase.SchemaName));
        })
        .AddIdentity<AccountEntity, RoleEntity>(o => { o.User.RequireUniqueEmail = true; })
        .AddRoles<RoleEntity>()
        .AddEntityFrameworkStores<IdentityDatabase>()
        .AddUserManager<UserManager<AccountEntity>>()
        .Services
        .AddScoped<IStartupTask, DbStartupTask>()
        .AddTransient<AuthorizationService>();

    public static void Configure(WebApplication app)
    {
        app.UseProjectXCors();
        app.UseProjectXSwagger();
        app.UseErrorHandler();
        app.UseProjectXLogging();
        app.UseHttpsRedirection(); //TODO: Consider to remove this
        app.UseAppAuthentication();
        app.UseCorrelationContext();
        app.MapControllers();
    }
}