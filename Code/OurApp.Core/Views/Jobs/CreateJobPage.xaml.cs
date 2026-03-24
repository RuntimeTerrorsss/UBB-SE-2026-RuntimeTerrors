using iss_project.UI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

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

            var result = await vm.CreateJob();

            if (!result.Success)
            {
                var dialog = new ContentDialog
                {
                    Title = "Validation Errors",
                    Content = result.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
                return;
            }

            // Success → go back to jobs list
            MainWindow.Instance.ShowJobs();
        }

        private async void GoBack(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirm Action",
                Content = "Are you sure you want to cancel the modifications?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

        
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                MainWindow.Instance.ShowJobs();
            }
            
        }

    }
}