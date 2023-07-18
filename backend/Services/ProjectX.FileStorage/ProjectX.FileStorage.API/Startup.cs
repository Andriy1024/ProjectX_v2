using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectX.AspNetCore.Extensions;
using ProjectX.AspNetCore.Swagger;
using ProjectX.Authentication;
using ProjectX.Core;
using ProjectX.Core.Context;
using ProjectX.Core.Observability;
using ProjectX.FileStorage.Application;
using ProjectX.FileStorage.Database;
using ProjectX.FileStorage.Database.Setup;
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
        .AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy())
        .AddMongoDb(mongodbConnectionString: 
            app.Configuration
            .GetSection(nameof(MongoConfig))
            .Get<MongoConfig>()?
            .ConnectionString!,
            name: "mongo")
        .Services
        .AddHealthChecksUI()
        .AddInMemoryStorage()
        .Services
        .AddFileStorage()
        .AddMongoServices(app.Configuration)
        .AddMongoRepository<FileDocument, Guid>();
    
    public static void Configure(WebApplication app) 
    {
        app.UseProjectXCors();
        app.UseProjectXSwagger();
        app.UseErrorHandler();

        app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });

        app.MapHealthChecksUI();

        app.UseProjectXLogging();
        app.UseAppAuthentication();
        app.UseCorrelationContext();
        app.MapControllers();
    }
}