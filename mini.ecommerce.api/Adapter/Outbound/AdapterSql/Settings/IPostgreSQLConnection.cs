using System.Data;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;

public interface IPostgreSQLConnection
{
    public IDbConnection ConnectCLUST(string banco);
}