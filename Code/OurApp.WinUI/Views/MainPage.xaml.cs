using OurApp.Core.Models;
using OurApp.WinUI.Views.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace OurApp.WinUI.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GoToAddJob(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowCreateJob();
        }

        private void GoToJobsList(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowJobs();
        }

        public void ShowEditJob(JobPosting job)
        {
            MainWindow.Instance.ShowEditJob(job);
        }

        private void GoToPastJobs(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowPastJobs();
        }
    }
}