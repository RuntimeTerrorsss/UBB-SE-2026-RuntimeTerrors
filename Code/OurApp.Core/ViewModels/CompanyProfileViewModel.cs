using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using OurApp.Core.Models;     
using OurApp.Core.Services;   

namespace App.ViewModels
{
    public partial class CompanyProfileViewModel : ObservableObject
    {
        private readonly ProfileCompletionCalculator _calculator;

        [ObservableProperty]
        private Company _company;

        [ObservableProperty]
        private int _completionPercentage;

        // ObservableCollection automatically notifies the UI when items are added/removed
        [ObservableProperty]
        private ObservableCollection<string> _remainingTasks = new();

        [ObservableProperty]
        private ObservableCollection<Collaborator> _collaboratorPreview = new();

        // 1. Inject your service here
        public CompanyProfileViewModel(ProfileCompletionCalculator calculator)
        {
            _calculator = calculator;
            LoadCompanyData();
        }

        private void LoadCompanyData()
        {
            // TODO: Load your Company from the database here
            // Company = ...

            // Update stats immediately after loading
            RefreshProfileStatistics();
        }

        public void RefreshProfileStatistics()
        {
            if (_company == null) return;

            var (percentage, tasks) = _calculator.Calculate(_company);

            CompletionPercentage = percentage;

            RemainingTasks.Clear();
            foreach (var task in tasks)
            {
                RemainingTasks.Add(task);
            }
        }

        [RelayCommand]
        private void NavigateToEditProfile()
        {
            // Navigation logic...
        }
    }
}