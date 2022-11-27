using Dapper;
using System.Data;

namespace ProjectX.Persistence.Dapper;

internal class DbCreator
{
    public void CreateDatabase(IDbConnection connection, string dbName)
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";
        
        var parameters = new DynamicParameters();
        
        parameters.Add("name", dbName);

        var records = connection.Query(query, parameters);
        
        if (!records.Any())
        {
            connection.Execute($"CREATE DATABASE {dbName}");
        }
    }
}