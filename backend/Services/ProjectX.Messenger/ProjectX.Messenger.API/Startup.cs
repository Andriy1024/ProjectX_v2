using ProjectX.AspNetCore.Extensions;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.Core.StartupTasks;
using ProjectX.Messenger.Application.Setup;
using ProjectX.Messenger.Application.UseCases;
using ProjectX.RabbitMq.Configuration;
using ProjectX.Realtime;
using System.Reflection;

namespace ProjectX.Messenger.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app)
    {
        app
        .AddCoreServices(Assembly.GetAssembly(typeof(SendMessageCommand))!)
        .AddObservabilityServices()
        .AddRabbitMqMessageBus()
        .AddRealtimeServices()
        .AddAppAuthentication()
        .AddCurrentUser()
        .ConfigureAspNetCore()
        .AddMarten()
        .Services
        .AddScoped<IStartupTask, MessageBusStartupTask>();
    }

    public static void Configure(WebApplication app)
    {
        app.UseProjectXCors();
        app.UseErrorHandler();
        app.UseProjectXLogging();
        app.UseAppAuthentication();
        app.UseCorrelationContext();
        app.MapControllers();
    }
}