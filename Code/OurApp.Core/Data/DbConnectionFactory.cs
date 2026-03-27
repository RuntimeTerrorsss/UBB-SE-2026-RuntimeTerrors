using Microsoft.Data.SqlClient;

namespace iss_project.Code.OurApp.Core.Data;

public class DbConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(DbConfig.ConnectionString);
    }
}