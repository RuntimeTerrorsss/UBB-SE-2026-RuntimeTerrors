using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iss_project.Application2.Interfaces.Repositories;
using iss_project.Application2.Interfaces.Services;
using iss_project.Domain.Entities;

namespace iss_project.Application2.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repo;

        public JobService(IJobRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateJobAsync(JobPosting job)
        {
            Validate(job);

            job.PostedAt = DateTime.Now;

            await _repo.AddAsync(job);
        }

        public async Task UpdateJobAsync(JobPosting job)
        {
            Validate(job);
            await _repo.UpdateAsync(job);
        }

        public async Task DeleteJobAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<JobPosting>> GetCurrentJobsAsync(int companyId)
        {
            var jobs = await _repo.GetByCompanyAsync(companyId);

            return jobs.Where(j =>
                j.PostedAt != null &&
                (j.Salary == null || j.Salary > 0) // example filter (adjust if needed)
            ).ToList();
        }

        public async Task<List<JobPosting>> GetPastJobsAsync(int companyId)
        {
            var jobs = await _repo.GetByCompanyAsync(companyId);

            return jobs.Where(j =>
                j.PostedAt != null &&
                j.PostedAt < DateTime.Now.AddMonths(-1) // example logic
            ).ToList();
        }

        private void Validate(JobPosting job)
        {
            if (job.CompanyId <= 0)
                throw new Exception("Company is required");

            if (string.IsNullOrWhiteSpace(job.JobTitle))
                throw new Exception("Title is required");

            if (string.IsNullOrWhiteSpace(job.IndustryField))
                throw new Exception("Field is required");

            if (string.IsNullOrWhiteSpace(job.JobDescription))
                throw new Exception("Description is required");

            if (string.IsNullOrWhiteSpace(job.JobLocation))
                throw new Exception("Location is required");

            if (job.AvailablePositions <= 0)
                throw new Exception("Positions must be positive");

            if (string.IsNullOrWhiteSpace(job.JobType))
                throw new Exception("Job type is required");

            if (string.IsNullOrWhiteSpace(job.ExperienceLevel))
                throw new Exception("Experience level is required");
        }
    }
}