using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OurApp.Core.Models;
using OurApp.Core.Repositories;

namespace OurApp.Core.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository repo)
        {
            _jobRepository = repo;
        }
        public async Task<List<SkillUsage>> GetSkillUsageAsync()
        {
            return await _jobRepository.GetSkillUsageAsync();
        }
        public async Task CreateJobAsync(JobPosting job, List<(int SkillId, int Percentage)> skills)
        {
            Validate(job);

            job.PostedAt = DateTime.Now;

            await _jobRepository.AddAsync(job, skills);
        }

        public async Task<List<(string SkillName, int Percentage)>> GetSkillsForJobAsync(int jobId)
        {
            return await _jobRepository.GetSkillsForJobAsync(jobId);
        }

        public async Task UpdateJobAsync(JobPosting job)
        {
            Validate(job);
            await _jobRepository.UpdateAsync(job);
        }

        public async Task DeleteJobAsync(int id)
        {
            await _jobRepository.DeleteAsync(id);
        }

        public async Task<List<JobPosting>> GetCurrentJobsAsync(int companyId)
        {
            var jobs = await _jobRepository.GetByCompanyAsync(companyId);

            return jobs.Where(j =>
                j.PostedAt != null &&
                (j.Salary == null || j.Salary > 0)
            ).ToList();
        }

        public async Task<List<JobPosting>> GetPastJobsAsync(int companyId)
        {
            return await _jobRepository.GetPastJobsAsync(companyId);
        }

        public async Task RepostJobAsync(JobPosting job)
        {
            
            job.PostedAt = DateTime.Now;

            
            if (job.Deadline.HasValue)
            {
                var duration = job.Deadline.Value - job.PostedAt.Value;
                job.Deadline = DateTime.Now.Add(duration);
            }

            await _jobRepository.UpdateAsync(job);
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