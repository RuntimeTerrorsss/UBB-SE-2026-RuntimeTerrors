using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace iss_project.UI.Views.Jobs
{
    public sealed partial class EditJobPage : Page
    {
        private EditJobViewModel ViewModel => this.DataContext as EditJobViewModel;

        public EditJobPage(JobPosting job)
        {
            this.InitializeComponent();
            this.DataContext = new EditJobViewModel(job);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            try
            {
                await ViewModel.UpdateJob();

                var dialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Job updated successfully",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();

                MainWindow.Instance.ShowJobs();
            }
            catch
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "We’re sorry, an error occurred. The job was not updated. Please try again.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.ShowJobs();
        }
    }
}