using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.AspNetCore.Http;
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
using Serilog;
using System.Reflection;

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

        services.AddDbServices<IdentityDatabase>((p, o) =>
        {
            o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(), 
                x => x.MigrationsHistoryTable("MigrationsHistory", IdentityDatabase.SchemaName));
        });

        services.AddMediatR(Assembly.GetAssembly(typeof(Startup))!);

        services.AddTransient<IEventDispatcher, EventDispatcher>();

        services.AddScoped<IStartupTask, DbStartupTask>();

        services
            .AddIdentity<AccountEntity, RoleEntity>(o => { o.User.RequireUniqueEmail = true; })
            .AddRoles<RoleEntity>()
            .AddEntityFrameworkStores<IdentityDatabase>()
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