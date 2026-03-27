using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OurApp.Core.Models;
using OurApp.Core.Database;

namespace OurApp.Core.Repositories
{
    public class JobsRepository : IJobsRepository
    {
        public IEnumerable<JobPosting> GetAllJobs()
        {
            var list = new List<JobPosting>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT j.job_id, j.job_title, j.industry_field, j.job_type, j.experience_level, 
                           j.start_date, j.end_date, j.job_description, j.job_location, j.available_positions,
                           j.posted_at, j.salary, j.amount_payed, j.deadline, j.photo,
                           c.company_id, c.company_name
                    FROM jobs j
                    LEFT JOIN companies c ON j.company_id = c.company_id";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var job = new JobPosting
                        {
                            JobId = reader.GetInt32(reader.GetOrdinal("job_id")),
                            JobTitle = reader.GetString(reader.GetOrdinal("job_title")),
                            IndustryField = reader.GetString(reader.GetOrdinal("industry_field")),
                            JobType = reader.GetString(reader.GetOrdinal("job_type")),
                            ExperienceLevel = reader.GetString(reader.GetOrdinal("experience_level"))
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("company_id")))
                        {
                            job.Company = new Company 
                            { 
                                CompanyId = reader.GetInt32(reader.GetOrdinal("company_id")),
                                CompanyName = reader.GetString(reader.GetOrdinal("company_name"))
                            };
                        }

                        list.Add(job);
                    }
                }
            }

            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT js.job_id, js.skill_id, js.required_percentage, s.skill_name
                    FROM job_skills js
                    JOIN skills s ON js.skill_id = s.skill_id";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int jobId = reader.GetInt32(reader.GetOrdinal("job_id"));
                        var targetJob = list.Find(j => j.JobId == jobId);
                        if (targetJob != null)
                        {
                            var js = new JobSkill
                            {
                                Job = targetJob,
                                RequiredPercentage = reader.GetInt32(reader.GetOrdinal("required_percentage")),
                                Skill = new Skill
                                {
                                    SkillId = reader.GetInt32(reader.GetOrdinal("skill_id")),
                                    SkillName = reader.GetString(reader.GetOrdinal("skill_name"))
                                }
                            };
                            targetJob.JobSkills.Add(js);
                        }
                    }
                }
            }

            return list;
        }
    }
}
