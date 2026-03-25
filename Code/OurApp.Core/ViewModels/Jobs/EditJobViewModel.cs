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
            System.Diagnostics.Debug.WriteLine($"JobType from DB: {job.JobType}");

            Job = job;

            JobTypes = new ObservableCollection<string>
            {
                "Full-time", "Part-time", "Volunteer", "Internship", "Remote", "Hybrid"
            };

            ExperienceLevels = new ObservableCollection<string>
            {
                "Internship", "Entry", "Mid","Senior", "Director","Junior"
            };

            // Normalize values to match dropdown EXACTLY
            Job.JobType = Job.JobType?.Trim().ToLower();
            Job.ExperienceLevel = Job.ExperienceLevel?.Trim().ToLower();
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

        public async Task UpdateJob()
        {
            await _jobService.UpdateJobAsync(Job);
        }
    }
}
