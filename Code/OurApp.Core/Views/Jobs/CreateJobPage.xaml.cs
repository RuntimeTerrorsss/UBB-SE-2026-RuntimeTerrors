using iss_project.UI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace iss_project.UI.Views.Jobs
{
    public sealed partial class CreateJobPage : Page
    {
        public CreateJobPage()
        {
            this.InitializeComponent();
            this.DataContext = new CreateJobViewModel();
        }

        private async void CreateJob_Click(object sender, RoutedEventArgs e)
        {
            var vm = (CreateJobViewModel)this.DataContext;
            await vm.CreateJob();
        }

        private void GoToJobs(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowJobs();
        }


    }
}