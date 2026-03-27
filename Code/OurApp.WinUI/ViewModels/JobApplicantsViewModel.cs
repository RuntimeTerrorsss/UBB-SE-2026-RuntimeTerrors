using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using OurApp.Core.Models;
using OurApp.Core.Services;
using OurApp.Core.Repositories;

namespace OurApp.WinUI.ViewModels
{
    public class JobApplicantsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IApplicantService _applicantService;

        public JobPosting SelectedJob { get; private set; }

        public ObservableCollection<Applicant> Applicants { get; } = new ObservableCollection<Applicant>();
        public ObservableCollection<string> ApplicationStatusOptions { get; } = new ObservableCollection<string> 
        { 
            "Accepted", "Rejected", "On Hold", "Recommandation" 
        };

        private string _draftStatus;
        public string DraftStatus { get { return _draftStatus; } set { if (_draftStatus != value) { _draftStatus = value; OnPropertyChanged(nameof(DraftStatus)); } } }

        private string _draftAppTestGrade;
        public string DraftAppTestGrade { get { return _draftAppTestGrade; } set { if (_draftAppTestGrade != value) { _draftAppTestGrade = value; OnPropertyChanged(nameof(DraftAppTestGrade)); } } }

        private string _draftCvGrade;
        public string DraftCvGrade { get { return _draftCvGrade; } set { if (_draftCvGrade != value) { _draftCvGrade = value; OnPropertyChanged(nameof(DraftCvGrade)); } } }

        private string _draftCompanyTestGrade;
        public string DraftCompanyTestGrade { get { return _draftCompanyTestGrade; } set { if (_draftCompanyTestGrade != value) { _draftCompanyTestGrade = value; OnPropertyChanged(nameof(DraftCompanyTestGrade)); } } }

        private string _draftInterviewGrade;
        public string DraftInterviewGrade { get { return _draftInterviewGrade; } set { if (_draftInterviewGrade != value) { _draftInterviewGrade = value; OnPropertyChanged(nameof(DraftInterviewGrade)); } } }

        private Applicant _selectedApplicant;
        public Applicant SelectedApplicant
        {
            get { return _selectedApplicant; }
            set
            {
                if (_selectedApplicant != value)
                {
                    _selectedApplicant = value;
                    if (_selectedApplicant != null)
                    {
                        LoadDraft(_selectedApplicant);
                        DetailsVisibility = Visibility.Visible;
                        TableVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        DetailsVisibility = Visibility.Collapsed;
                        TableVisibility = Visibility.Visible;
                    }
                    OnPropertyChanged(nameof(SelectedApplicant));
                }
            }
        }

        private Visibility _tableVisibility = Visibility.Visible;
        public Visibility TableVisibility
        {
            get { return _tableVisibility; }
            set
            {
                if (_tableVisibility != value)
                {
                    _tableVisibility = value;
                    OnPropertyChanged(nameof(TableVisibility));
                }
            }
        }

        private Visibility _detailsVisibility = Visibility.Collapsed;
        public Visibility DetailsVisibility
        {
            get { return _detailsVisibility; }
            set
            {
                if (_detailsVisibility != value)
                {
                    _detailsVisibility = value;
                    OnPropertyChanged(nameof(DetailsVisibility));
                }
            }
        }

        public JobApplicantsViewModel(JobPosting job)
        {
            SelectedJob = job;
            IApplicantRepository repo = new ApplicantRepository();
            _applicantService = new ApplicantService(repo);

            LoadApplicants();
        }

        private void LoadApplicants()
        {
            Applicants.Clear();
            if (SelectedJob != null)
            {
                try
                {
                    var applicants = _applicantService.GetApplicantsForJob(SelectedJob);
                    foreach (var applicant in applicants)
                    {
                        Applicants.Add(applicant);
                    }
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                    Applicants.Add(new Applicant { ApplicantId = 999, User = new OurApp.Core.Models.User(1, "Mock Applicant (DB Error)", ""), ApplicationStatus = "On Hold", Job = SelectedJob });
                }
            }
        }

        private void LoadDraft(Applicant applicant)
        {
            DraftStatus = applicant.ApplicationStatus;
            DraftAppTestGrade = applicant.AppTestGrade?.ToString() ?? "";
            DraftCvGrade = applicant.CvGrade?.ToString() ?? "";
            DraftCompanyTestGrade = applicant.CompanyTestGrade?.ToString() ?? "";
            DraftInterviewGrade = applicant.InterviewGrade?.ToString() ?? "";
        }

        public void SaveChanges()
        {
            if (SelectedApplicant == null) return;

            SelectedApplicant.ApplicationStatus = DraftStatus;
            
            if (decimal.TryParse(DraftAppTestGrade, out decimal t1)) SelectedApplicant.AppTestGrade = t1;
            else if (string.IsNullOrWhiteSpace(DraftAppTestGrade)) SelectedApplicant.AppTestGrade = null;

            if (decimal.TryParse(DraftCvGrade, out decimal t2)) SelectedApplicant.CvGrade = t2;
            else if (string.IsNullOrWhiteSpace(DraftCvGrade)) SelectedApplicant.CvGrade = null;

            if (decimal.TryParse(DraftCompanyTestGrade, out decimal t3)) SelectedApplicant.CompanyTestGrade = t3;
            else if (string.IsNullOrWhiteSpace(DraftCompanyTestGrade)) SelectedApplicant.CompanyTestGrade = null;

            if (decimal.TryParse(DraftInterviewGrade, out decimal t4)) SelectedApplicant.InterviewGrade = t4;
            else if (string.IsNullOrWhiteSpace(DraftInterviewGrade)) SelectedApplicant.InterviewGrade = null;

            _applicantService.UpdateApplicant(SelectedApplicant);
            
            int index = Applicants.IndexOf(SelectedApplicant);
            if (index >= 0)
            {
                Applicants[index] = SelectedApplicant;
            }

            GoBackFromDetails();
        }

        public void GoBackFromDetails()
        {
            SelectedApplicant = null; // This will trigger the setter to collapse details and show table
        }

        public void RemoveApplicant(Applicant applicant)
        {
            if (applicant != null)
            {
                _applicantService.RemoveApplicant(applicant.ApplicantId);
                Applicants.Remove(applicant);
                if (SelectedApplicant == applicant)
                {
                    GoBackFromDetails();
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
