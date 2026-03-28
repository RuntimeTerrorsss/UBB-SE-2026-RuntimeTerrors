using OurApp.Core.Models;
using OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using iss_project.UI.Validators;
using OurApp.WinUI;

namespace iss_project.Code.OurApp.Core.ViewModels.Jobs
{
    public class SkillRequirement : INotifyPropertyChanged
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }
        private int _percentage = 100;
        public int Percentage
        {
            get => _percentage;
            set { _percentage = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class EditJobViewModel : INotifyPropertyChanged
    {
        private readonly IJobService _jobService;
        private readonly SkillService _skillService;

        public JobPosting Job { get; set; }
        public ObservableCollection<string> JobTypes { get; set; }
        public ObservableCollection<string> ExperienceLevels { get; set; }
        public ObservableCollection<SkillRequirement> Skills { get; set; } = new();

        public bool IsRepostMode { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public EditJobViewModel(JobPosting job)
        {
            _jobService = MainWindow.Services.GetService<IJobService>();
            _skillService = new SkillService();

            Job = job;

            JobTypes = new ObservableCollection<string>
            {
                "Full-time", "Part-time", "Volunteer", "Internship", "Remote", "Hybrid"
            };

            ExperienceLevels = new ObservableCollection<string>
            {
                "Internship", "Entry", "Mid","Senior", "Director","Junior"
            };
        }

        public string JobType
        {
            get => Job.JobType;
            set { Job.JobType = value; OnPropertyChanged(); }
        }

        public string ExperienceLevel
        {
            get => Job.ExperienceLevel;
            set { Job.ExperienceLevel = value; OnPropertyChanged(); }
        }

        public DateTimeOffset? Deadline
        {
            get => Job.Deadline.HasValue ? new DateTimeOffset(Job.Deadline.Value) : null;
            set { Job.Deadline = value?.DateTime; OnPropertyChanged(); }
        }

        public DateTimeOffset? StartDate
        {
            get => Job.StartDate.HasValue ? new DateTimeOffset(Job.StartDate.Value) : null;
            set { Job.StartDate = value?.DateTime; OnPropertyChanged(); }
        }

        public DateTimeOffset? EndDate
        {
            get => Job.EndDate.HasValue ? new DateTimeOffset(Job.EndDate.Value) : null;
            set { Job.EndDate = value?.DateTime; OnPropertyChanged(); }
        }

        public async Task LoadSkillsAsync()
        {
            var dbSkills = await _skillService.GetAllSkillsAsync();

            Skills.Clear();

            foreach (var skill in dbSkills)
            {
                // Find if this skill is already selected in the job
                var existing = Job.RequiredSkills?.FirstOrDefault(rs => rs.SkillName == skill.SkillName);

                Skills.Add(new SkillRequirement
                {
                    SkillId = skill.SkillId,
                    SkillName = skill.SkillName,
                    IsSelected = existing != null,
                    Percentage = existing?.Percentage ?? 100 // default to 100 if not selected
                });
            }
        }

        public async Task<(bool Success, string Message)> UpdateJob()
        {
            var validator = new JobValidator();
            var errors = validator.Validate(this);

            if (errors.Count > 0)
                return (false, string.Join("\n", errors));

            try
            {
                // Map selected skills
                Job.RequiredSkills = Skills
                    .Where(s => s.IsSelected)
                    .Select(s => new JobSkill { SkillName = s.SkillName, Percentage = s.Percentage })
                    .ToList();

                await _jobService.UpdateJobAsync(Job);

                return (true, "Job updated successfully");
            }
            catch
            {
                return (false, "We’re sorry, an error occurred. The job was not updated.");
            }
        }
    }
}