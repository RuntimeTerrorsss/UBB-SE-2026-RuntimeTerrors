using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iss_project.Code.OurApp.Core.Services
{
    public interface IJobService
    {
        Task CreateJobAsync(JobPosting job, List<(int SkillId, int Percentage)> skills);

        Task<List<(string SkillName, int Percentage)>> GetSkillsForJobAsync(int jobId);
        Task UpdateJobAsync(JobPosting job);
        Task DeleteJobAsync(int id);

        Task<List<JobPosting>> GetCurrentJobsAsync(int companyId);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);
        Task RepostJobAsync(JobPosting job);
    }
}