using Microsoft.Data.SqlClient;

namespace OurApp.Core.Data;

public class DbConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(DbConfig.ConnectionString);
    }
}