using iss_project.Code.OurApp.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iss_project.Code.OurApp.Core.Data;

namespace iss_project.Code.OurApp.Core.Repositories
{
    public class JobPostingRepository : IJobRepository
    {
        private readonly string _connectionString;

        public JobPostingRepository()
        {
            _connectionString = DbConfig.ConnectionString;
        }

        public async Task AddAsync(JobPosting job)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                INSERT INTO Job_Postings
                (company_id, job_title, industry_field, job_type, experience_level,
                 period_of_time, job_description, job_location, available_positions,
                 posted_at, salary, amount_payed)
                VALUES
                (@CompanyId, @JobTitle, @IndustryField, @JobType, @ExperienceLevel,
                 @PeriodOfTime, @JobDescription, @JobLocation, @AvailablePositions,
                 @PostedAt, @Salary, @AmountPayed)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", job.CompanyId);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle ?? "");
                    command.Parameters.AddWithValue("@IndustryField", job.IndustryField ?? "");
                    command.Parameters.AddWithValue("@JobType", job.JobType ?? "");
                    command.Parameters.AddWithValue("@ExperienceLevel", job.ExperienceLevel ?? "");
                    command.Parameters.AddWithValue("@PeriodOfTime", (object?)job.PeriodOfTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobDescription", job.JobDescription ?? "");
                    command.Parameters.AddWithValue("@JobLocation", job.JobLocation ?? "");
                    command.Parameters.AddWithValue("@AvailablePositions", job.AvailablePositions);
                    command.Parameters.AddWithValue("@PostedAt", job.PostedAt ?? DateTime.Now);
                    command.Parameters.AddWithValue("@Salary", (object?)job.Salary ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AmountPayed", (object?)job.AmountPayed ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<JobPosting> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Job_Postings WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new JobPosting
                            {
                                JobId = Convert.ToInt32(reader["job_id"]),
                                CompanyId = Convert.ToInt32(reader["company_id"]),
                                JobTitle = reader["job_title"].ToString(),
                                IndustryField = reader["industry_field"].ToString(),
                                JobType = reader["job_type"].ToString(),
                                ExperienceLevel = reader["experience_level"].ToString(),
                                PeriodOfTime = reader["period_of_time"]?.ToString(),
                                JobDescription = reader["job_description"].ToString(),
                                JobLocation = reader["job_location"].ToString(),
                                AvailablePositions = Convert.ToInt32(reader["available_positions"]),
                                PostedAt = reader["posted_at"] == DBNull.Value ? null : (DateTime?)reader["posted_at"],
                                Salary = reader["salary"] == DBNull.Value ? null : (int?)reader["salary"],
                                AmountPayed = reader["amount_payed"] == DBNull.Value ? null : (int?)reader["amount_payed"]
                            };
                        }
                    }
                }
            }

            return null; // not found
        }

        public async Task<List<JobPosting>> GetByCompanyAsync(int companyId)
        {
            var jobs = new List<JobPosting>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Job_Postings WHERE company_id = @CompanyId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            jobs.Add(new JobPosting
                            {
                                JobId = Convert.ToInt32(reader["job_id"]),
                                CompanyId = Convert.ToInt32(reader["company_id"]),
                                JobTitle = reader["job_title"].ToString(),
                                IndustryField = reader["industry_field"].ToString(),
                                JobType = reader["job_type"].ToString(),
                                ExperienceLevel = reader["experience_level"].ToString(),
                                PeriodOfTime = reader["period_of_time"]?.ToString(),
                                JobDescription = reader["job_description"].ToString(),
                                JobLocation = reader["job_location"].ToString(),
                                AvailablePositions = Convert.ToInt32(reader["available_positions"]),
                                PostedAt = reader["posted_at"] == DBNull.Value ? null : (DateTime?)reader["posted_at"],
                                Salary = reader["salary"] == DBNull.Value ? null : (int?)reader["salary"],
                                AmountPayed = reader["amount_payed"] == DBNull.Value ? null : (int?)reader["amount_payed"]
                            });
                        }
                    }
                }
            }

            return jobs;
        }

        public async Task UpdateAsync(JobPosting job)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                UPDATE Job_Postings
                SET job_title = @JobTitle,
                    industry_field = @IndustryField,
                    job_type = @JobType,
                    experience_level = @ExperienceLevel,
                    job_description = @JobDescription,
                    job_location = @JobLocation,
                    available_positions = @AvailablePositions
                WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", job.JobId);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                    command.Parameters.AddWithValue("@IndustryField", job.IndustryField);
                    command.Parameters.AddWithValue("@JobType", job.JobType);
                    command.Parameters.AddWithValue("@ExperienceLevel", job.ExperienceLevel);
                    command.Parameters.AddWithValue("@JobDescription", job.JobDescription);
                    command.Parameters.AddWithValue("@JobLocation", job.JobLocation);
                    command.Parameters.AddWithValue("@AvailablePositions", job.AvailablePositions);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM Job_Postings WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}