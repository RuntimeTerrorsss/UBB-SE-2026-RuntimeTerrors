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
        private void GoToJobsList(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowJobs();
        }
        private void GoToPastJobs(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowPastJobs();
        }
    }
}