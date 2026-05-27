using System.Data;
using Npgsql;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;

public class PostgreSQLConnection : IPostgreSQLConnection
{
    private readonly ConnectionString _connectionString;

    public PostgreSQLConnection(IConfiguration configuration)
    {
        _connectionString = new ConnectionString()
        {
            Cluster = Environment.GetEnvironmentVariable("DATABASE_CLUSTER"),
            Username = Environment.GetEnvironmentVariable("DATABASE_USERNAME"),
            Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD"),
        };
    }
    
    public IDbConnection ConnectCLUST(string banco) => new NpgsqlConnection(_connectionString.GetConnectionString(banco));
}

public class ConnectionString
{
    public string? Cluster { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public string GetConnectionString(string banco)
    {
        var crypt = new CryptSpa();
        string _Password = crypt.DecryptDES(Password!, crypt.Chave);
        
        return $"Host={Cluster};Database={banco};Username={Username};Password={_Password};";
    }
}