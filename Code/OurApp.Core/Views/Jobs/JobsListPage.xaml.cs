using iss_project.UI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
    }
}