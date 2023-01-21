using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.StartupTasks;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.Core.StartupTasks;
using ProjectX.RabbitMq.Configuration;
using ProjectX.Realtime.API.Controllers;
using ProjectX.Realtime.API.WebSockets;
using ProjectX.Realtime.IntegrationEvent;
using ProjectX.Realtime.PublicContract;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .AddCoreServices(Assembly.GetAssembly(typeof(RealtimeController))!)
    .AddObservabilityServices()
    .AddAppAuthentication()
    .AddCurrentUser()
    .ConfigureAspNetCore()
    .AddProjectXSwagger()
    .Services
    .AddSingleton<ApplicationWebSocketManager>()
    .AddSingleton<WebSocketAuthenticationManager>()
    .AddScoped<IStartupTask, MessageBusStartupTask>()
    .AddRabbitMqMessageBus<RealtimeMessageDispatcher>(builder.Configuration);

builder.Host.UseSerilog();

var app = builder.Build();

app.UseProjectXSwagger();

app.UseAppAuthentication();

app.Map("/contracts", o => o.UseMiddleware<RealtimeContractsMiddleware>());

app.UseCorrelationContext();

app.MapControllers();

app.UseWebSockets();

app.Map("/ws", o => o.UseMiddleware<WebSocketMiddleware>());

try
{
    Log.Information("Starting web host");

    await app.RunWithTasksAsync();

    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Program terminated unexpectedly!");

    return 1;
}
finally
{
    Log.CloseAndFlush();
}