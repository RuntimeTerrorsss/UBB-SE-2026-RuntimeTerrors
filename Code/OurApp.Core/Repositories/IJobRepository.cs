using System.Collections.Generic;
using System.Threading.Tasks;
using OurApp.Core.Models;

namespace OurApp.Core.Repositories
{
    public interface IJobRepository
    {
        Task<List<JobPosting>> GetByCompanyAsync(int companyId);
        Task<JobPosting> GetByIdAsync(int id);
        Task AddAsync(JobPosting job, List<SkillRequirement> skills);
        Task UpdateAsync(JobPosting job);
        Task DeleteAsync(int id);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);
        Task<List<SkillUsage>> GetSkillUsageAsync();
        Task<List<SkillRequirement>> GetSkillsForJobAsync(int jobId);
    }
}