using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using iss_project.UI.Validators;

namespace iss_project.Code.OurApp.Core.ViewModels.Jobs
{
    public class EditJobViewModel : INotifyPropertyChanged
    {
        private readonly IJobService _jobService;

        public JobPosting Job { get; set; }
        public ObservableCollection<string> JobTypes { get; set; }
        public ObservableCollection<string> ExperienceLevels { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public EditJobViewModel(JobPosting job)
        {
            _jobService = MainWindow.Services.GetService<IJobService>();

            Job = job;

            JobTypes = new ObservableCollection<string>
            {
                "Full-time", "Part-time", "Volunteer", "Internship", "Remote", "Hybrid"
            };

            ExperienceLevels = new ObservableCollection<string>
            {
                "Internship", "Entry", "Mid","Senior", "Director","Junior"
            };

            
            //Job.JobType = Job.JobType?.Trim().ToLower();
            //Job.ExperienceLevel = Job.ExperienceLevel?.Trim().ToLower();
        }

        public string JobType
        {
            get => Job.JobType;
            set
            {
                Job.JobType = value;
                OnPropertyChanged();
            }
        }

        public string ExperienceLevel
        {
            get => Job.ExperienceLevel;
            set
            {
                Job.ExperienceLevel = value;
                OnPropertyChanged();
            }
        }

        public async Task<(bool Success, string Message)> UpdateJob()
        {
            var validator = new JobValidator();
            var errors = validator.Validate(this);

            if (errors.Count > 0)
            {
                return (false, string.Join("\n", errors));
            }

            try
            {
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
