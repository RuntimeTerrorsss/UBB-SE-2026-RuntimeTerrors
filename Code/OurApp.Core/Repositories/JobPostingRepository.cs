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

        public async Task<List<(string SkillName, int Percentage)>> GetSkillsForJobAsync(int jobId)
        {
            var skills = new List<(string SkillName, int Percentage)>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
SELECT s.skill_name, js.required_percentage
FROM job_skills js
JOIN skills s ON js.skill_id = s.skill_id
WHERE js.job_id = @JobId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@JobId", jobId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                skills.Add((reader.GetString(0), reader.GetInt32(1)));
            }

            return skills;
        }

        public async Task AddAsync(JobPosting job, List<(int SkillId, int Percentage)> skills)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // 🔥 Get inserted job_id
                string query = @"
INSERT INTO jobs
(company_id, photo, job_title, industry_field, job_type, experience_level,
 start_date, end_date, job_description, job_location, available_positions,
 posted_at, salary, amount_payed, deadline, scheduled_at)
OUTPUT INSERTED.job_id
VALUES
(@CompanyId, @Photo, @JobTitle, @IndustryField, @JobType, @ExperienceLevel,
 @StartDate, @EndDate, @JobDescription, @JobLocation, @AvailablePositions,
 @PostedAt, @Salary, @AmountPayed, @Deadline, @ScheduledAt)";

                int jobId;

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

                    jobId = (int)await command.ExecuteScalarAsync();
                }

                // 🔥 Insert into job_skills
                if (skills != null)
                {
                    foreach (var skill in skills)
                    {
                        string skillQuery = @"
INSERT INTO job_skills (job_id, skill_id, required_percentage)
VALUES (@JobId, @SkillId, @Percentage)";

                        using (SqlCommand cmd = new SqlCommand(skillQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@JobId", jobId);
                            cmd.Parameters.AddWithValue("@SkillId", skill.SkillId);
                            cmd.Parameters.AddWithValue("@Percentage", skill.Percentage);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
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
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // 1️⃣ Update jobs table
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

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@JobId", job.JobId);
                cmd.Parameters.AddWithValue("@Photo", (object?)job.Photo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                cmd.Parameters.AddWithValue("@IndustryField", job.IndustryField);
                cmd.Parameters.AddWithValue("@JobType", job.JobType);
                cmd.Parameters.AddWithValue("@ExperienceLevel", job.ExperienceLevel);
                cmd.Parameters.AddWithValue("@StartDate", (object?)job.StartDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object?)job.EndDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@JobDescription", job.JobDescription);
                cmd.Parameters.AddWithValue("@JobLocation", job.JobLocation);
                cmd.Parameters.AddWithValue("@AvailablePositions", job.AvailablePositions);
                cmd.Parameters.AddWithValue("@Salary", (object?)job.Salary ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AmountPayed", (object?)job.AmountPayed ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Deadline", (object?)job.Deadline ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedAt", (object?)job.PostedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ScheduledAt", (object?)job.ScheduledAt ?? DBNull.Value);

                await cmd.ExecuteNonQueryAsync();
            }

            // 2️⃣ Delete old skills
            string deleteSkills = "DELETE FROM job_skills WHERE job_id = @JobId";
            using (var cmd = new SqlCommand(deleteSkills, connection))
            {
                cmd.Parameters.AddWithValue("@JobId", job.JobId);
                await cmd.ExecuteNonQueryAsync();
            }

            // 3️⃣ Insert new skills
            foreach (var skill in job.RequiredSkills)
            {
                string insertSkill = @"
INSERT INTO job_skills (job_id, skill_id, required_percentage)
SELECT @JobId, skill_id, @Percentage
FROM skills
WHERE skill_name = @SkillName";

                using (var cmd = new SqlCommand(insertSkill, connection))
                {
                    cmd.Parameters.AddWithValue("@JobId", job.JobId);
                    cmd.Parameters.AddWithValue("@SkillName", skill.SkillName);
                    cmd.Parameters.AddWithValue("@Percentage", skill.Percentage);

                    await cmd.ExecuteNonQueryAsync();
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
                // RequiredSkills will be loaded separately via GetSkillsForJobAsync
            };
        }
    }
}