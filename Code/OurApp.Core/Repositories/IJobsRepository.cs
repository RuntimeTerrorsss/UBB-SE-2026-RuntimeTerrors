using System.Collections.Generic;
using OurApp.Core.Models;

namespace OurApp.Core.Repositories
{
    public interface IJobsRepository
    {
        IEnumerable<JobPosting> GetAllJobs();
    }
}
