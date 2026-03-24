using System.Collections.Generic;
using System.Threading.Tasks;
using iss_project.Domain.Entities;

namespace iss_project.Application2.Interfaces.Services
{
    public interface IJobService
    {
        Task CreateJobAsync(JobPosting job);
        Task UpdateJobAsync(JobPosting job);
        Task DeleteJobAsync(int id);

        Task<List<JobPosting>> GetCurrentJobsAsync(int companyId);
        Task<List<JobPosting>> GetPastJobsAsync(int companyId);
    }
}