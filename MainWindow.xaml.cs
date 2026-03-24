using iss_project.Application2.Interfaces.Repositories;
using iss_project.Application2.Interfaces.Services;
using iss_project.Application2.Services;
using iss_project.Infrastructure.Data;
using iss_project.Infrastructure.Repositories;
using iss_project.UI.Views.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace iss_project
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static IServiceProvider Services { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();

            Instance = this;

            var services = new ServiceCollection();

            services.AddSingleton<DbConnectionFactory>();
            services.AddScoped<IJobRepository, JobPostingRepository>();
            services.AddScoped<IJobService, JobService>();

            Services = services.BuildServiceProvider();

            ShowCreateJob();
        }

        public void ShowCreateJob()
        {
            this.Content = new CreateJobPage();
        }

        public void ShowJobs()
        {
            this.Content = new JobsListPage();
        }
    }
}