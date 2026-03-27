using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iss_project.Code.OurApp.Core.Repositories
{
    using global::iss_project.Code.OurApp.Core.Data;
    using global::iss_project.Code.OurApp.Core.Models;
    using Microsoft.Data.SqlClient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace iss_project.Code.OurApp.Core.Repositories
    {
        public class SkillRepository
        {
            private readonly string _connectionString;

            public SkillRepository()
            {
                _connectionString = DbConfig.ConnectionString;
            }

            public async Task<List<Skill>> GetAllAsync()
            {
                var skills = new List<Skill>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT skill_id, skill_name FROM skills";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            skills.Add(new Skill
                            {
                                SkillId = (int)reader["skill_id"],
                                SkillName = reader["skill_name"].ToString()
                            });
                        }
                    }
                }

                return skills;
            }
        }
    }
}
