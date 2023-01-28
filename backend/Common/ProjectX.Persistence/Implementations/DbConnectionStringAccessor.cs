using Microsoft.Extensions.Options;
using ProjectX.Core.Setup;
using ProjectX.Persistence.Abstractions;

namespace ProjectX.Persistence.Implementations;

internal class DbConnectionStringAccessor : IDbConnectionStringAccessor
{
    private readonly string _connectionString;

    public DbConnectionStringAccessor(IOptions<ConnectionStrings> options)
    {
        _connectionString = options.Value.DbConnection;
    }

    public string GetConnectionString() => _connectionString;
}