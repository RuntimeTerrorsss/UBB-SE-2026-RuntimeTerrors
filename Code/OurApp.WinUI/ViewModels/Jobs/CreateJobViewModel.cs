using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.WinUI.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OurApp.WinUI.ViewModels.Jobs
{
    public class CreateJobViewModel : INotifyPropertyChanged
    {
        private readonly IJobService _jobService;
        private readonly SkillService _skillService = new SkillService();

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

            Skills = new ObservableCollection<SkillRequirement>();
            _ = LoadSkills(); // async fire-and-forget

        }

        private async Task LoadSkills()
        {
            var dbSkills = await _skillService.GetAllSkillsAsync();

            Skills.Clear();

            foreach (var skill in dbSkills)
            {
                Skills.Add(new SkillRequirement
                {
                    SkillId = skill.SkillId,
                    SkillName = skill.SkillName
                });
            }
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

        public ObservableCollection<SkillRequirement> Skills { get; set; } = new();

        private bool _useScheduledPosting;


public bool UseScheduledPosting
        {
            get => _useScheduledPosting;
            set
            {
                _useScheduledPosting = value;
                OnPropertyChanged();
            }
        }

        private DateTimeOffset? _scheduledAt;

        public DateTimeOffset? ScheduledAt
        {
            get => _scheduledAt;
            set
            {
                _scheduledAt = value;
                OnPropertyChanged();
            }
        }

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
                    AmountPayed = AmountPayed,
                    ScheduledAt = UseScheduledPosting ? ScheduledAt?.DateTime : null,
                };

                var selectedSkills = Skills
                .Where(s => s.IsSelected)
                .Select(s => new SkillRequirement() { SkillId = s.SkillId, Percentage = s.Percentage })
                .ToList();

                await _jobService.CreateJobAsync(job, selectedSkills);

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