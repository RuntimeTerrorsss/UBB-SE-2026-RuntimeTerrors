using iss_project.Code.OurApp.Core.Data;
using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Repositories;
using iss_project.Code.OurApp.Core.Services;
using iss_project.UI.Views;
using iss_project.UI.Views.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;

namespace iss_project
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static IServiceProvider Services { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();

            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(1200, 800));

            Instance = this;

            var services = new ServiceCollection();

            services.AddSingleton<DbConnectionFactory>();
            services.AddScoped<IJobRepository, JobPostingRepository>();
            services.AddScoped<IJobService, JobService>();

            Services = services.BuildServiceProvider();

            ShowMain();
        }

        public void ShowCreateJob()
        {
            this.Content = new CreateJobPage();
        }

        public void ShowJobs()
        {
            this.Content = new JobsListPage();
        }

        public void ShowMain()
        {
            this.Content = new MainPage();
        }

        public void ShowEditJob(JobPosting job, bool isRepost = false)
        {
            this.Content = new EditJobPage(job, isRepost);
        }

        public void ShowPastJobs()
        {
            this.Content = new PastJobsPage();
        }
    }
}