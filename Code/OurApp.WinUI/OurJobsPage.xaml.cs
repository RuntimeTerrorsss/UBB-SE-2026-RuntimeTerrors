using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using OurApp.Core.Models;
using OurApp.WinUI.ViewModels;

namespace OurApp.WinUI
{
    public sealed partial class OurJobsPage : Page
    {
        public OurJobsViewModel ViewModel { get; }

        public OurJobsPage()
        {
            this.InitializeComponent();
            ViewModel = new OurJobsViewModel();
        }

        private void ViewApplicantsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is JobPosting job)
            {
                Frame.Navigate(typeof(JobApplicantsPage), job);
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentStage == OurJobsViewModel.PageStage.Jobs)
                MainWindow.Instance.ShowMain();
            else
                ViewModel.GoBack();
        }
    }
}
