using Microsoft.Extensions.Configuration;
using ProjectX.Persistence.Abstractions;

namespace ProjectX.Persistence.Implementations;

internal class DbConnectionStringAccessor : IDbConnectionStringAccessor
{
    private readonly string _connectionString;

    public DbConnectionStringAccessor(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DbConnection").ThrowIfNullOrEmpty("Empty db connection string.");
    }

    public string GetConnectionString() => _connectionString;
}