using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using iss_project.UI.Views.Jobs;

namespace iss_project.UI.Views
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
    }
}