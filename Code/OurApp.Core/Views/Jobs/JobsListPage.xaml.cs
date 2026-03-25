using iss_project.Code.OurApp.Core.Models;
using iss_project.UI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace iss_project.UI.Views.Jobs
{
    public sealed partial class JobsListPage : Page
    {
        private JobsListViewModel ViewModel => (JobsListViewModel)this.DataContext;

        public JobsListPage()
        {
            this.InitializeComponent();

            this.DataContext = new JobsListViewModel();

            Loaded += async (s, e) =>
            {
                await ViewModel.LoadJobs();
            };
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowMain();
        }

        private void GoToAddJob(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowCreateJob();
        }

        private void EditJob_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            var job = menuItem?.Tag as JobPosting;

            if (job == null) return;

            MainWindow.Instance.ShowEditJob(job);
        }

        private async void DeleteJob_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            var job = menuItem?.Tag as JobPosting;

            if (job == null) return;

            var dialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = "Are you sure you want to delete this job?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await ViewModel.DeleteJob(job.JobId);
                await ViewModel.LoadJobs();
            }
        }
    }
}