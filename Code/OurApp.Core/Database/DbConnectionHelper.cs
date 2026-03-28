using Microsoft.Data.SqlClient;
using System;

namespace OurApp.Core.Database
{
    public static class DbConnectionHelper
    {
        // Derived from your provided Server instance name and the MyDb database
        //Cip DB
        //private const string connectionString = @"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        private const string ConnectionString = "Data Source=TEA\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        //private const string ConnectionString =@"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        //private const string ConnectionString = @"Data Source=Aron\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
