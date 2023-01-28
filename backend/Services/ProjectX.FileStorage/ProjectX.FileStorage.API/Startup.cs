using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.FileStorage.Application;
using ProjectX.FileStorage.Database;
using ProjectX.FileStorage.Persistence.Database.Documents;
using ProjectX.FileStorage.Persistence.FileStorage;
using System.Reflection;

namespace ProjectX.FileStorage.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app)  => app
        .AddCoreServices(Assembly.GetAssembly(typeof(MapperProfile))!)
        .AddObservabilityServices()
        .AddAppAuthentication()
        .AddCurrentUser()
        .ConfigureAspNetCore()
        .AddProjectXSwagger()
        .Services
        .AddFileStorage()
        .AddMongoServices(app.Configuration)
        .AddMongoRepository<FileDocument, Guid>();
    
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