using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Models;
using OurApp.WinUI.ViewModels;

namespace OurApp.WinUI
{
    public sealed partial class JobApplicantsPage : Page
    {
        public JobApplicantsViewModel ViewModel { get; private set; }

        public JobApplicantsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is JobPosting job)
            {
                ViewModel = new JobApplicantsViewModel(job);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TableVisibility == Visibility.Collapsed)
            {
                ViewModel.GoBackFromDetails();
            }
            else if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void ApplicantsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // The ViewModel's SelectedApplicant property update already handles showing details grid,
            // but we can ensure it is synced here if needed.
            var listView = sender as ListView;
            if (listView?.SelectedItem is Applicant applicant)
            {
                ViewModel.SelectedApplicant = applicant;
            }
        }

        private void RemoveApplicant_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Applicant applicant)
            {
                ViewModel.RemoveApplicant(applicant);
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveChanges();
        }
    }

    public class EmptyStringToPendingConverter : Microsoft.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            return string.IsNullOrWhiteSpace(str) ? "Pending" : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
