using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using ProjectX.Dashboard.Persistence.Context;
using System.Reflection;
using ProjectX.Dashboard.Application.Mapper;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Observability;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Core.Context;
using ProjectX.Persistence.Abstractions;
using ProjectX.Core.StartupTasks;
using ProjectX.Dashboard.Persistence;
using ProjectX.RabbitMq.Configuration;
using ProjectX.Realtime;
using ProjectX.AspNetCore.Extensions;
using ProjectX.Dashboard.Application;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProjectX.Dashboard.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) => app
        .AddCoreServices(Assembly.GetAssembly(typeof(DashboardProfile))!)
        .AddObservabilityServices()
        .AddRabbitMqMessageBus()
        .AddRealtimeServices()
        .AddAppAuthentication()
        .AddCurrentUser()
        .ConfigureAspNetCore()
        .AddProjectXSwagger()
        .Services
        .AddCoreHealthChecks()
        .Services
        .AddDbServices<DashboardDbContext>((p, o) =>
        {
            o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(),
                x => x.MigrationsHistoryTable("MigrationsHistory", DashboardDbContext.SchemaName));
        })
        .AddTransactinBehaviour()
        .AddScoped<IStartupTask, DashboardDatabaseStartup>()
        .AddDomainEventsHandlers();
    
    public static void Configure(WebApplication app) 
    {
        app.UseProjectXCors();
        app.UseProjectXSwagger();
        app.UseErrorHandler();
        app.UseCoreHeathChecks();
        app.UseProjectXLogging();
        app.UseAppAuthentication();
        app.UseCorrelationContext();
        app.MapControllers();
    }
}