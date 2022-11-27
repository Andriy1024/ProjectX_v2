using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using ProjectX.Tasks.Application.Handlers.Tasks;
using ProjectX.Tasks.Persistence.Context;
using System.Reflection;
using ProjectX.Tasks.Application.Mapper;
using ProjectX.AspNetCore.Http;
using ProjectX.Authentication;
using Serilog;
using ProjectX.Core;
using ProjectX.Core.Observability;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Core.Validation;
using ProjectX.Core.Context;

namespace ProjectX.Tasks.API;

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
        
        services.AddDbServices<TasksDbContext>(o => o.UseInMemoryDatabase(databaseName: "ProjectX.Tasks"));
        
        services.AddMediatR(Assembly.GetAssembly(typeof(TasksHandlers))!);
        
        services.AddAutoMapper(Assembly.GetAssembly(typeof(TaskProfile))!);

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

        app.UseSerilogRequestLogging();

        app.UseProjectXAuthentication();

        app.UseContexts();

        app.MapControllers();
    }
}