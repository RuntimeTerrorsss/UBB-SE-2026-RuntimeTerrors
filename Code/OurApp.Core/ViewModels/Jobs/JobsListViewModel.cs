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
            var jobs = await _jobService.GetCurrentJobsAsync(1); // CompanyId = 1

            Jobs.Clear();

            foreach (var job in jobs)
            {
                // Load skills from DB
                var skills = await _jobService.GetSkillsForJobAsync(job.JobId);

                // Map to JobSkill objects
                job.RequiredSkills = skills
                    .Select(s => new JobSkill
                    {
                        SkillName = s.SkillName,
                        Percentage = s.Percentage
                    })
                    .ToList();

                Jobs.Add(job);
            }
        }

        public async Task<(bool Success, string Message)> DeleteJob(int jobId)
        {
            try
            {
                await _jobService.DeleteJobAsync(jobId);

                var job = Jobs.FirstOrDefault(j => j.JobId == jobId);
                if (job != null)
                    Jobs.Remove(job);

                return (true, "Job deleted successfully");
            }
            catch
            {
                return (false, "We’re sorry, an error occurred. The job was not deleted. Please try again.");
            }
        }
    }
}