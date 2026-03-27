using OurApp.Core.Models;
using OurApp.WinUI.ViewModels.Jobs;
using OurApp.WinUI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace OurApp.WinUI.Views.Jobs
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

        private async void SeeApplicants_Click(object sender, RoutedEventArgs e)
        {
            // something here
        }
    }
}