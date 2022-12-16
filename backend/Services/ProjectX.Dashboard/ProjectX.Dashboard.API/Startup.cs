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

namespace ProjectX.Dashboard.API;

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
        
        services.AddTransient<IEventDispatcher, EventDispatcher>();
        
        services.AddDbServices<DashboardDbContext>((p, o) =>    
        {
            o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(),
                x => x.MigrationsHistoryTable("MigrationsHistory", DashboardDbContext.SchemaName));
        });

        services.AddScoped<IStartupTask, DashboardDatabaseStartup>();

        services.AddMediatR(Assembly.GetAssembly(typeof(TasksHandlers))!);
        
        services.AddAutoMapper(Assembly.GetAssembly(typeof(DashboardProfile))!);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        app.UseSerilogRequestLogging(o => 
        {
            // o.EnrichDiagnosticContext = TODO: enrich with IContext
        });

        app.UseProjectXAuthentication();

        app.UseContexts();

        app.MapControllers();
    }
}