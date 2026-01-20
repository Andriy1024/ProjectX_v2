using System.Reflection;

using ProjectX.Core;
using ProjectX.Core.Observability;
using ProjectX.AspNetCore.Extensions;
using ProjectX.Core.Context;
using ProjectX.Search.API.Models;
using Nest;

namespace ProjectX.Search.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder
           .AddCoreServices(Assembly.GetAssembly(typeof(Startup))!)
           .AddObservabilityServices()
           //.AddAppAuthentication()
           //.AddCurrentUser()
           .ConfigureAspNetCore()
           .Services
           .AddCoreHealthChecks()
           .Services
           .AddControllers()
           .Services
           .AddElasticSearch(builder.Configuration);
    }

    public static void Configure(WebApplication app)
    {
        app.UseProjectXCors();
        
        app.UseErrorHandler();

        app.UseCoreHeathChecks();

        app.UseProjectXLogging();

        //app.UseAppAuthentication();

        app.UseCorrelationContext();

        app.MapControllers();
    }


    public static void AddElasticSearch(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var uri = configuration["ELKConfiguration:Uri"]!;
        
        var index = configuration["ELKConfiguration:Index"]!;

        var settings = new Nest.ConnectionSettings(new Uri(uri))
            .PrettyJson()
            .DefaultIndex(index);

        // Ignore means we don't search by these fields
        settings.DefaultMappingFor<Product>(p => 
            p.Ignore(x => x.Id)
             .Ignore(x => x.Quantity));

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        client.Indices.Create(index, i => i.Map<Product>(x => x.AutoMap()));
    }
}