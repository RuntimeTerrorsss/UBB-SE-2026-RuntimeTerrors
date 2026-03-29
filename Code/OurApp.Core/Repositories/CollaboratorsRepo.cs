using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OurApp.Core.Database;
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
    public class CollaboratorsRepo : ICollaboratorsRepo
    {

        /// <summary>
        /// Function that adds a collaborator to the collaborators table
        /// </summary>
        /// <param name="eventOfCollaboration"> the event that the invited company is collaborating on </param>
        /// <param name="collaboratorToBeAdded"> the company that has been invited to collaborate </param>
        public void AddCollaboratorToRepo(Event eventOfCollaboration, Company collaboratorToBeAdded)
        {
            using (SqlConnection sqlConnection = DbConnectionHelper.GetConnection())
            {
                sqlConnection.Open();

                string queryToBeRun = @"
                    INSERT INTO collaborators 
                    VALUES (@EventId, @CompanyId)";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@EventId", eventOfCollaboration.Id);
                sqlCommand.Parameters.AddWithValue("@CompanyId", collaboratorToBeAdded.CompanyId);

                sqlCommand.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Function that returns a list of all the collaborators of the user company
        /// </summary>
        /// <param name="loggedInCompanyId">  </param>
        /// <returns></returns>
        public List<Company> GetAllCollaborators(int loggedInCompanyId)
        {
            var usersCollaborators = new List<Company>();

            using (SqlConnection sqlConnection = DbConnectionHelper.GetConnection())
            {
                sqlConnection.Open();

                string queryToBeRun = @"
                    SELECT *
                    FROM companies
                    WHERE company_id IN (
                        SELECT c2.company_id
                        FROM collaborators c2
                        INNER JOIN events e ON e.event_id = c2.event_id
                        WHERE e.host_company_id = @HostID
                    )
                    ";

                SqlCommand sqlCommand = new SqlCommand(queryToBeRun, sqlConnection);


                sqlCommand.Parameters.AddWithValue("@HostID", loggedInCompanyId);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    usersCollaborators.Add(new Company(
                        reader["company_name"].ToString(),
                        "",
                        "",
                        reader["logo_picture_url"].ToString(),
                        "",
                        ""
                    ));
                    
                }
            }

            return usersCollaborators;
        }
    }
}
