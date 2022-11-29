using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectX.Persistence.Implementations;

public class DbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
{
    public T CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            //.AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DbConnection").ThrowIfNullOrEmpty("ConnectionString is empty.");

        var optionsBuilder = new DbContextOptionsBuilder<T>();

        optionsBuilder.UseNpgsql(connectionString);

        return Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
    }
}