using System.Collections.Generic;
using System.Threading.Tasks;
using OurApp.Core.Models;

namespace OurApp.Core.Repositories
{
    public interface IJobRepository
    {
        Task<List<JobPosting>> GetByCompanyAsync(int companyId);
        Task<JobPosting> GetByIdAsync(int id);
        Task AddAsync(JobPosting job, List<(int SkillId, int Percentage)> skills);
        Task UpdateAsync(JobPosting job);
        Task DeleteAsync(int id);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);
        Task<List<SkillUsage>> GetSkillUsageAsync();
        Task<List<(string SkillName, int Percentage)>> GetSkillsForJobAsync(int jobId);
    }
}