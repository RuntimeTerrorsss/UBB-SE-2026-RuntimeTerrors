using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.ViewModels.Jobs;
using iss_project.UI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace iss_project.UI.Views.Jobs
{
    public sealed partial class PastJobsPage : Page
    {
        private PastJobsViewModel ViewModel => (PastJobsViewModel)this.DataContext;

        public PastJobsPage()
        {
            this.InitializeComponent();

            this.DataContext = new PastJobsViewModel();

            Loaded += async (s, e) =>
            {
                await ViewModel.LoadJobs();
            };
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowMain();
        }

        private async void RepostJob_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            var job = (JobPosting)menuItem?.Tag;

            MainWindow.Instance.ShowEditJob(job, true);
        }
    }
}