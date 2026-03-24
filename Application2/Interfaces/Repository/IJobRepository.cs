using System.Collections.Generic;
using System.Threading.Tasks;
using iss_project.Domain.Entities;

namespace iss_project.Application2.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<List<JobPosting>> GetByCompanyAsync(int companyId);
        Task<JobPosting> GetByIdAsync(int id);
        Task AddAsync(JobPosting job);
        Task UpdateAsync(JobPosting job);
        Task DeleteAsync(int id);
    }
}