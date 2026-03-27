using Microsoft.Data.SqlClient;
using System;

namespace OurApp.Core.Database
{
    public static class DbConnectionHelper
    {
        // Derived from your provided Server instance name and the MyDb database
        private const string ConnectionString = @"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
