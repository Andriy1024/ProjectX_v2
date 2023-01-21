using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.StartupTasks;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.StartupTasks;
using ProjectX.RabbitMq.Configuration;
using ProjectX.Realtime.API.WebSockets;
using ProjectX.Realtime.IntegrationEvent;
using ProjectX.Realtime.PublicContract;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .AddCoreServices()
    .AddAppAuthentication()
    .AddCurrentUser()
    .ConfigureAspNetCore()
    .AddProjectXSwagger()
    .Services
    .AddSingleton<ApplicationWebSocketManager>()
    .AddSingleton<WebSocketAuthenticationManager>()
    .AddScoped<IStartupTask, MessageBusStartupTask>()
    .AddRabbitMqMessageBus<RealtimeMessageDispatcher>(builder.Configuration);

var app = builder.Build();

app.UseProjectXSwagger();

// https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier -  sub
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseAppAuthentication();

app.Map("/contracts", o => o.UseMiddleware<RealtimeContractsMiddleware>());

app.UseCorrelationContext();

app.MapControllers();

app.UseWebSockets();

app.Map("/ws", o => o.UseMiddleware<WebSocketMiddleware>());

await app.RunWithTasksAsync();