using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public interface IJobService
    {
        Task CreateJobAsync(JobPosting job, List<SkillRequirement> skills);
        Task<List<SkillUsage>> GetSkillUsageAsync();
        Task<List<SkillRequirement>> GetSkillsForJobAsync(int jobId);
        Task UpdateJobAsync(JobPosting job);
        Task DeleteJobAsync(int id);

        Task<List<JobPosting>> GetCurrentJobsAsync(int companyId);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);
        Task RepostJobAsync(JobPosting job);
    }
}