using Microsoft.Data.SqlClient;
using System;

namespace OurApp.Core.Database
{
    public static class DbConnectionHelper
    {
        // Derived from your provided Server instance name and the MyDb database
        //Cip DB
<<<<<<< HEAD
        private const string connectionString = @"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        //private const string ConnectionString = "Data Source=TEA\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        //private const string ConnectionString =@"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        //private const string ConnectionString = @"Data Source=Aron\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        //private const string ConnectionString = "Data Source=.;Initial Catalog=iss_project;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
=======
        //private const string connectionString = @"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        //private const string ConnectionString = "Data Source=TEA\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        //private const string ConnectionString =@"Server=(localdb)\ProjectModels;Database=MyDb;Trusted_Connection=True;TrustServerCertificate=True;";
        //private const string ConnectionString = @"Data Source=Aron\\SQLEXPRESS;Initial Catalog=iss_project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private const string ConnectionString = "Data Source=.;Initial Catalog=iss_project;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
>>>>>>> 6cc806786879ca58285409a45fcf6f483414bfc3

        public static SqlConnection GetConnection()
        {
            return new SqlConnection();
        }
    }
}
