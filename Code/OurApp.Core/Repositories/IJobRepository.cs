using System.Collections.Generic;
using System.Threading.Tasks;
using iss_project.Code.OurApp.Core.Models;

namespace iss_project.Code.OurApp.Core.Repositories
{
    public interface IJobRepository
    {
        Task<List<JobPosting>> GetByCompanyAsync(int companyId);
        Task<JobPosting> GetByIdAsync(int id);
        Task AddAsync(JobPosting job, List<(int SkillId, int Percentage)> skills);
        Task UpdateAsync(JobPosting job);
        Task DeleteAsync(int id);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);

        Task<List<(string SkillName, int Percentage)>> GetSkillsForJobAsync(int jobId);
    }
}