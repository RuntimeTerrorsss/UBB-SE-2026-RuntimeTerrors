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
INSERT INTO jobs
(company_id, photo, job_title, industry_field, job_type, experience_level,
 start_date, end_date, job_description, job_location, available_positions,
 posted_at, salary, amount_payed, deadline, scheduled_at)
VALUES
(@CompanyId, @Photo, @JobTitle, @IndustryField, @JobType, @ExperienceLevel,
 @StartDate, @EndDate, @JobDescription, @JobLocation, @AvailablePositions,
 @PostedAt, @Salary, @AmountPayed, @Deadline, @ScheduledAt)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", job.CompanyId);
                    command.Parameters.AddWithValue("@Photo", (object?)job.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle ?? "");
                    command.Parameters.AddWithValue("@IndustryField", job.IndustryField ?? "");
                    command.Parameters.AddWithValue("@JobType", job.JobType ?? "");
                    command.Parameters.AddWithValue("@ExperienceLevel", job.ExperienceLevel ?? "");
                    command.Parameters.AddWithValue("@StartDate", (object?)job.StartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object?)job.EndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobDescription", job.JobDescription ?? "");
                    command.Parameters.AddWithValue("@JobLocation", job.JobLocation ?? "");
                    command.Parameters.AddWithValue("@AvailablePositions", job.AvailablePositions);
                    command.Parameters.AddWithValue("@PostedAt", job.PostedAt ?? DateTime.Now);
                    command.Parameters.AddWithValue("@Salary", (object?)job.Salary ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AmountPayed", (object?)job.AmountPayed ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Deadline", (object?)job.Deadline ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ScheduledAt", (object?)job.ScheduledAt ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<JobPosting?> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM jobs WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapJob(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task<List<JobPosting>> GetPastJobsAsync(int companyId)
        {
            var jobs = new List<JobPosting>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
SELECT * FROM jobs
WHERE company_id = @CompanyId
AND deadline IS NOT NULL
AND deadline < GETDATE()";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            jobs.Add(MapJob(reader));
                        }
                    }
                }
            }

            return jobs;
        }

        public async Task<List<JobPosting>> GetByCompanyAsync(int companyId)
        {
            var jobs = new List<JobPosting>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
SELECT * FROM jobs
WHERE company_id = @CompanyId
AND (
    scheduled_at IS NULL
    OR scheduled_at <= GETDATE()
)
AND (
    deadline IS NULL
    OR deadline > GETDATE()
)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            jobs.Add(MapJob(reader));
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
UPDATE jobs
SET photo = @Photo,
    job_title = @JobTitle,
    industry_field = @IndustryField,
    job_type = @JobType,
    experience_level = @ExperienceLevel,
    start_date = @StartDate,
    end_date = @EndDate,
    job_description = @JobDescription,
    job_location = @JobLocation,
    available_positions = @AvailablePositions,
    salary = @Salary,
    amount_payed = @AmountPayed,
    deadline = @Deadline,
    posted_at = @PostedAt,
    scheduled_at = @ScheduledAt
WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", job.JobId);
                    command.Parameters.AddWithValue("@Photo", (object?)job.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                    command.Parameters.AddWithValue("@IndustryField", job.IndustryField);
                    command.Parameters.AddWithValue("@JobType", job.JobType);
                    command.Parameters.AddWithValue("@ExperienceLevel", job.ExperienceLevel);
                    command.Parameters.AddWithValue("@StartDate", (object?)job.StartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object?)job.EndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@JobDescription", job.JobDescription);
                    command.Parameters.AddWithValue("@JobLocation", job.JobLocation);
                    command.Parameters.AddWithValue("@AvailablePositions", job.AvailablePositions);
                    command.Parameters.AddWithValue("@Salary", (object?)job.Salary ?? DBNull.Value);
                    command.Parameters.AddWithValue("@AmountPayed", (object?)job.AmountPayed ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Deadline", (object?)job.Deadline ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PostedAt", (object?)job.PostedAt ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ScheduledAt", (object?)job.ScheduledAt ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM jobs WHERE job_id = @JobId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobId", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private JobPosting MapJob(SqlDataReader reader)
        {
            return new JobPosting
            {
                JobId = Convert.ToInt32(reader["job_id"]),
                CompanyId = Convert.ToInt32(reader["company_id"]),
                Photo = reader["photo"]?.ToString(),
                JobTitle = reader["job_title"].ToString(),
                IndustryField = reader["industry_field"].ToString(),
                JobType = reader["job_type"].ToString(),
                ExperienceLevel = reader["experience_level"].ToString(),
                StartDate = reader["start_date"] == DBNull.Value ? null : (DateTime?)reader["start_date"],
                EndDate = reader["end_date"] == DBNull.Value ? null : (DateTime?)reader["end_date"],
                JobDescription = reader["job_description"].ToString(),
                JobLocation = reader["job_location"].ToString(),
                AvailablePositions = Convert.ToInt32(reader["available_positions"]),
                PostedAt = reader["posted_at"] == DBNull.Value ? null : (DateTime?)reader["posted_at"],
                Salary = reader["salary"] == DBNull.Value ? null : (int?)reader["salary"],
                AmountPayed = reader["amount_payed"] == DBNull.Value ? null : (int?)reader["amount_payed"],
                Deadline = reader["deadline"] == DBNull.Value ? null : (DateTime?)reader["deadline"],
                ScheduledAt = reader["scheduled_at"] == DBNull.Value ? null : (DateTime?)reader["scheduled_at"]
            };
        }
    }
}