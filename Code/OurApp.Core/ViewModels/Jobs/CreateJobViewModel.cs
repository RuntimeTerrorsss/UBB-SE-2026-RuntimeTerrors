using iss_project.Code.OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
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

            JobTypes = new ObservableCollection<string>
            {
                "part-time", "full-time", "volunteer", "internship", "remote", "hybrid"
            };

            ExperienceLevels = new ObservableCollection<string>
            {
                "internship", "entry-level", "mid-senior level", "director", "executive"
            };
        }

        public int CompanyId { get; set; } = 1;

        // Fields
        public string JobTitle { get; set; }
        public string IndustryField { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }

        public string JobDescription { get; set; }
        public string JobLocation { get; set; }

        public int AvailablePositions { get; set; }

        // Dates
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? Deadline { get; set; }

        // Financials
        public int? Salary { get; set; }
        public int? AmountPayed { get; set; }

        // Dropdown data
        public ObservableCollection<string> JobTypes { get; set; }
        public ObservableCollection<string> ExperienceLevels { get; set; }

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
                PostedAt = DateTime.Now,
                StartDate = StartDate?.DateTime,
                EndDate = EndDate?.DateTime,
                Deadline = Deadline?.DateTime,
                Salary = Salary,
                AmountPayed = AmountPayed
            };

            await _jobService.CreateJobAsync(job);
        }
    }
}