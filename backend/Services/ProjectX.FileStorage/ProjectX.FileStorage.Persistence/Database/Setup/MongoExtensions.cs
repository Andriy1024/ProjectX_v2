﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ProjectX.Core.Setup;
using ProjectX.Core.StartupTasks;
using ProjectX.FileStorage.Persistence.Database.Abstractions;
using ProjectX.FileStorage.Persistence.Database.Setup;

namespace ProjectX.FileStorage.Persistence.Database;

public static class MongoExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString(nameof(ConnectionStrings.DbConnection))
            ?? throw new ArgumentNullException(nameof(ConnectionStrings.DbConnection));

        var options = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>()
            ?? throw new ArgumentNullException(nameof(MongoOptions));

        if (string.IsNullOrWhiteSpace(options.DatabaseName))
        {
            throw new ArgumentNullException("MongoOptions.DatabaseName");
        }

        services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

        services.AddSingleton<IMongoClient>(provider => 
        {
            return new MongoClient(connectionString);
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
    {
        return services.AddTransient<IMongoRepository<TEntity, TIdentifiable>, MongoRepository<TEntity, TIdentifiable>>();
    }
}
