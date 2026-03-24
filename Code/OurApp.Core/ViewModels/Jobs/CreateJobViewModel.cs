using iss_project.Code.OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using iss_project.Code.OurApp.Core.Models;

namespace iss_project.UI.ViewModels.Jobs
{
    public class CreateJobViewModel
    {
        private readonly IJobService _jobService;

        public CreateJobViewModel()
        {
            _jobService = MainWindow.Services.GetService<IJobService>();
        }

        public int CompanyId { get; set; } = 1; // temporary

        public string JobTitle { get; set; }
        public string IndustryField { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }

        public string JobDescription { get; set; }
        public string JobLocation { get; set; }

        public int AvailablePositions { get; set; }

        public async Task CreateJob()
        {
            var job = new JobPosting
            {
                CompanyId = CompanyId,
                JobTitle = JobTitle,
                IndustryField = IndustryField,
                JobType = JobType,
                ExperienceLevel = ExperienceLevel,
                JobDescription = JobDescription,
                JobLocation = JobLocation,
                AvailablePositions = AvailablePositions,
                PostedAt = DateTime.Now
            };

            await _jobService.CreateJobAsync(job);
        }
    }
}