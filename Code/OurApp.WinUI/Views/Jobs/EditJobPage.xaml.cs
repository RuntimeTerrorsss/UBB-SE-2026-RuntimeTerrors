using OurApp.Core.Models;
using OurApp.WinUI.ViewModels.Jobs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using iss_project.UI.Views;

namespace OurApp.WinUI.Views.Jobs
{
    public sealed partial class EditJobPage : Page
    {
        private EditJobViewModel ViewModel => this.DataContext as EditJobViewModel;

        public EditJobPage(JobPosting job, bool isRepost = false)
        {
            this.InitializeComponent();

            var vm = new EditJobViewModel(job)
            {
                IsRepostMode = isRepost
            };

            this.DataContext = vm;

            // Load skills from DB
            _ = vm.LoadSkillsAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            try
            {
                if (ViewModel.IsRepostMode)
                {
                    ViewModel.Job.PostedAt = DateTime.Now;
                }

                var result = await ViewModel.UpdateJob();

                var dialog = new ContentDialog
                {
                    Title = result.Success ? "Success" : "Error",
                    Content = result.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();

                if (result.Success)
                {
                    MainWindow.Instance.ShowJobs();
                }
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

        private void PayToPromote_Click(object sender, RoutedEventArgs e)
        {
            // TODO: implement later
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
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