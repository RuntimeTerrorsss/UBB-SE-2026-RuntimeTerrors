using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace iss_project.UI.ViewModels.Jobs
{
    public class JobsListViewModel
    {
        private readonly IJobService _jobService;

        public ObservableCollection<JobPosting> Jobs { get; set; }

        public JobsListViewModel()
        {
            _jobService = MainWindow.Services.GetService<IJobService>();
            Jobs = new ObservableCollection<JobPosting>();
        }

        public async Task LoadJobs()
        {
            var jobs = await _jobService.GetCurrentJobsAsync(1); // hardcoded companyId

            Jobs.Clear();

            foreach (var job in jobs)
            {
                Jobs.Add(job);
            }
        }
        public async Task DeleteJob(int jobId)
        {
            await _jobService.DeleteJobAsync(jobId);

            var job = Jobs.FirstOrDefault(j => j.JobId == jobId);
            if (job != null)
            {
                Jobs.Remove(job);
            }
        }
    }
}