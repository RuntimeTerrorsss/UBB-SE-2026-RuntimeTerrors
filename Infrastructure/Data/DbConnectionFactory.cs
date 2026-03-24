using Microsoft.Data.SqlClient;

namespace iss_project.Infrastructure.Data
{
    public class DbConnectionFactory
    {
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(DbConfig.ConnectionString);
        }
    }
}