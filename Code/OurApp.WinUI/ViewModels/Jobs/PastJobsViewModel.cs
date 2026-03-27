using OurApp.Core.Models;
using OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.WinUI.ViewModels.Jobs
{
    public class PastJobsViewModel
    {
        private readonly IJobService _jobService;

        public ObservableCollection<JobPosting> Jobs { get; set; }

        public PastJobsViewModel()
        {
            _jobService = MainWindow.Services.GetService<IJobService>();
            Jobs = new ObservableCollection<JobPosting>();
        }

        public async Task LoadJobs()
        {
            var jobs = await _jobService.GetPastJobsAsync(1);

            Jobs.Clear();
            foreach (var job in jobs)
                Jobs.Add(job);
        }

        public async Task<(bool Success, string Message)> RepostJob(JobPosting job)
        {
            try
            {
                await _jobService.RepostJobAsync(job);
                Jobs.Remove(job);

                return (true, "Job reposted successfully");
            }
            catch
            {
                return (false, "We’re sorry, an error occurred. The job was not reposted.");
            }
        }
    }
}
