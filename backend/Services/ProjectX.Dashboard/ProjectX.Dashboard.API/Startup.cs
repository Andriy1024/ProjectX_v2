using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using ProjectX.Dashboard.Application.Handlers.Tasks;
using ProjectX.Dashboard.Persistence.Context;
using System.Reflection;
using ProjectX.Dashboard.Application.Mapper;
using ProjectX.AspNetCore.Http;
using ProjectX.Authentication;
using Serilog;
using ProjectX.Core;
using ProjectX.Core.Observability;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Core.Validation;
using ProjectX.Core.Context;
using ProjectX.Persistence.Abstractions;
using ProjectX.Core.StartupTasks;
using ProjectX.Dashboard.Persistence;
using Microsoft.AspNetCore.Mvc;
using ProjectX.RabbitMq.Configuration;
using ProjectX.Realtime;
using ProjectX.AspNetCore.Extensions;

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
        .AddDbServices<DashboardDbContext>((p, o) =>
        {
            o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(),
                x => x.MigrationsHistoryTable("MigrationsHistory", DashboardDbContext.SchemaName));
        })
        .AddTransactinBehaviour()
        .AddScoped<IStartupTask, DashboardDatabaseStartup>();
    
    public static void Configure(WebApplication app) 
    {
        app.UseProjectXCors();
        app.UseProjectXSwagger();
        app.UseErrorHandler();
        app.UseProjectXLogging();
        app.UseAppAuthentication();
        app.UseCorrelationContext();
        app.MapControllers();
    }
}