using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Services;
using iss_project.UI.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iss_project.UI.ViewModels.Jobs
{
    public class CreateJobViewModel : INotifyPropertyChanged
    {
        private readonly IJobService _jobService;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public CreateJobViewModel()
        {
            _jobService = MainWindow.Services.GetService<IJobService>();

            JobTypes = new ObservableCollection<string>
            {
                "Full-time", "Part-time", "Volunteer", "Internship", "Remote", "Hybrid"
            };

            ExperienceLevels = new ObservableCollection<string>
            {
                "Internship", "Entry", "Mid","Senior", "Director","Junior"
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

        public async Task<(bool Success, string Message)> CreateJob()
        {
            var validator = new JobValidator();
            var errors = validator.Validate(this);

            if (errors.Any())
            {
                var message = string.Join("\n", errors);
                return (false, message);
            }

            try
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

                    Deadline = UseAutomaticExpiration && ExpirationDays.HasValue
        ? DateTime.Now.AddDays(ExpirationDays.Value)
        : Deadline?.DateTime,

                    Salary = Salary,
                    AmountPayed = AmountPayed
                };

                await _jobService.CreateJobAsync(job);

                return (true, "Job created successfully");
            }
            catch
            {
                return (false, "We're sorry, an error occurred. The job was not created. Please try again.");
            }
        }

        private bool _useAutomaticExpiration;
        public bool UseAutomaticExpiration
        {
            get => _useAutomaticExpiration;
            set
            {
                _useAutomaticExpiration = value;
                OnPropertyChanged(); // 🔥 this notifies UI
            }
        }

        private int? _expirationDays;
        public int? ExpirationDays
        {
            get => _expirationDays;
            set
            {
                _expirationDays = value;
                OnPropertyChanged(); // 🔥 this notifies UI
            }
        }
    }
}