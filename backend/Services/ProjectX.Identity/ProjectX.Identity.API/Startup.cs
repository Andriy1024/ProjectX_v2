using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.Identity.Application.Services;
using System.Reflection;
using ProjectX.Identity.Persistence;
using ProjectX.Identity.Application;

namespace ProjectX.Identity.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) => app
        .AddCoreServices(Assembly.GetAssembly(typeof(SignInCommand))!)
        .AddObservabilityServices()
        .ConfigureAspNetCore()
        .AddProjectXSwagger()
        .AddAppAuthentication()
        .AddCurrentUser()
        .Services
        .AddTransient<AuthorizationService>()
        .AddPersistence();

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