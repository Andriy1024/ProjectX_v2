using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ProjectX.Core.StartupTasks;
using ProjectX.FileStorage.Database.Abstractions;
using ProjectX.FileStorage.Database.Setup;

namespace ProjectX.FileStorage.Database;

public static class MongoSetupExtensions
{
    public static IServiceCollection AddMongoServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services
            .AddOptions<MongoConfig>()
            .BindConfiguration(nameof(MongoConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var options = configuration.GetSection(nameof(MongoConfig)).Get<MongoConfig>()
            ?? throw new ArgumentNullException(nameof(MongoConfig));

        services.AddSingleton<IMongoClient>(provider => 
        {
            return new MongoClient(options.ConnectionString);
        });

        services.AddTransient<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        services.AddScoped<IStartupTask, MongoStartupTask>();

        return services;
    }

    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services)
        where TEntity : IDocumentEntry<TIdentifiable>
        where TIdentifiable : notnull
    {
        return services.AddTransient<IMongoRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}