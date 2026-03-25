using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class CollaboratorsRepo
    {
        private string connectionString { get; set; }

        public CollaboratorsRepo(string connection)
        {
            this.connectionString = connection;
        }

        public void AddCollaboratorToRepo(Event eventOfCollaboration, Company collaboratorToBeAdded)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = @"
                    INSERT INTO collaborators 
                    VALUES (@EventId, @CompanyId)";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@EventId", eventOfCollaboration.Id);
                sqlCommand.Parameters.AddWithValue("@CompanyId", collaboratorToBeAdded.Id);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public List<Company> GetAllCollaborators(int e)
        {
            var collabs = new List<Company>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string queryToBeRun = "SELECT * FROM companies c inner join collaborators c2 on c.company_id = c2.company_id inner join events e on e.event_id = c2.event_id WHERE e.host_company_id = @HostID";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);


                sqlCommand.Parameters.AddWithValue("@HostID", e);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    collabs.Add(new Company(
                        (int)reader["company_id"],
                        reader["company_name"].ToString(),
                        "",
                        "",
                        reader["logo_picture_url"].ToString(),
                        "",
                        ""
                    ));
                    
                }
            }

            return collabs;
        }
    }
}
