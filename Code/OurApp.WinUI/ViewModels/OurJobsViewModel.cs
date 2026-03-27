using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using OurApp.Core.Database;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;

namespace OurApp.WinUI.ViewModels
{
    public class OurJobsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IApplicantService _applicantService;
        // Note: uses JobsRepository (→ DbConnectionHelper → MyDb) as that is the live DB

        // ── Stage navigation ──────────────────────────────────────────
        public enum PageStage { Jobs, Applicants, Details }

        private PageStage _currentStage = PageStage.Jobs;
        public PageStage CurrentStage
        {
            get => _currentStage;
            set
            {
                if (_currentStage != value)
                {
                    _currentStage = value;
                    OnPropertyChanged(nameof(CurrentStage));
                    OnPropertyChanged(nameof(JobsVisibility));
                    OnPropertyChanged(nameof(ApplicantsVisibility));
                    OnPropertyChanged(nameof(DetailsVisibility));
                    OnPropertyChanged(nameof(BackButtonVisibility));
                }
            }
        }

        public Visibility JobsVisibility       => CurrentStage == PageStage.Jobs       ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ApplicantsVisibility => CurrentStage == PageStage.Applicants ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DetailsVisibility    => CurrentStage == PageStage.Details    ? Visibility.Visible : Visibility.Collapsed;
        public Visibility BackButtonVisibility => CurrentStage == PageStage.Jobs       ? Visibility.Collapsed : Visibility.Visible;

        // ── Collections ───────────────────────────────────────────────
        public ObservableCollection<JobPosting> Jobs       { get; } = new();
        public ObservableCollection<Applicant>  Applicants { get; } = new();
        public ObservableCollection<string> ApplicationStatusOptions { get; } = new()
        {
            "Accepted", "Rejected", "On Hold", "Recommandation"
        };

        // ── Selection ────────────────────────────────────────────────
        private JobPosting _selectedJob;
        public JobPosting SelectedJob
        {
            get => _selectedJob;
            set { if (_selectedJob != value) { _selectedJob = value; OnPropertyChanged(nameof(SelectedJob)); } }
        }

        private Applicant _selectedApplicant;
        public Applicant SelectedApplicant
        {
            get => _selectedApplicant;
            set
            {
                if (_selectedApplicant != value)
                {
                    _selectedApplicant = value;
                    if (_selectedApplicant != null) LoadDraft(_selectedApplicant);
                    OnPropertyChanged(nameof(SelectedApplicant));
                }
            }
        }

        // ── Draft grade fields ────────────────────────────────────────
        private string _draftStatus;
        public string DraftStatus { get => _draftStatus; set { if (_draftStatus != value) { _draftStatus = value; OnPropertyChanged(nameof(DraftStatus)); } } }

        private string _draftAppTestGrade;
        public string DraftAppTestGrade { get => _draftAppTestGrade; set { if (_draftAppTestGrade != value) { _draftAppTestGrade = value; OnPropertyChanged(nameof(DraftAppTestGrade)); } } }

        private string _draftCvGrade;
        public string DraftCvGrade { get => _draftCvGrade; set { if (_draftCvGrade != value) { _draftCvGrade = value; OnPropertyChanged(nameof(DraftCvGrade)); } } }

        private string _draftCompanyTestGrade;
        public string DraftCompanyTestGrade { get => _draftCompanyTestGrade; set { if (_draftCompanyTestGrade != value) { _draftCompanyTestGrade = value; OnPropertyChanged(nameof(DraftCompanyTestGrade)); } } }

        private string _draftInterviewGrade;
        public string DraftInterviewGrade { get => _draftInterviewGrade; set { if (_draftInterviewGrade != value) { _draftInterviewGrade = value; OnPropertyChanged(nameof(DraftInterviewGrade)); } } }

        // ── Constructor ───────────────────────────────────────────────
        public OurJobsViewModel()
        {
            _applicantService = new ApplicantService(new ApplicantRepository());
            _ = LoadJobsAsync();
        }

        // ── Job loading ───────────────────────────────────────────────
        /// <summary>Loads all jobs using JobsRepository (correct DB connection).</summary>
        public async Task LoadJobsAsync()
        {
            try
            {
                IJobsRepository jobsRepo = new JobsRepository();
                var jobsFromDb = jobsRepo.GetAllJobs();
                Jobs.Clear();
                foreach (var job in jobsFromDb)
                    Jobs.Add(job);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error loading jobs: {ex.Message}");
            }

            await Task.CompletedTask;
        }

        // ── Job deletion ──────────────────────────────────────────────
        public async Task<(bool Success, string Message)> DeleteJob(int jobId)
        {
            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                conn.Open();
                // Delete skills first (FK), then the job
                using (var cmd = new SqlCommand("DELETE FROM job_skills WHERE job_id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", jobId);
                    await cmd.ExecuteNonQueryAsync();
                }
                using (var cmd = new SqlCommand("DELETE FROM jobs WHERE job_id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", jobId);
                    await cmd.ExecuteNonQueryAsync();
                }

                var job = Jobs.FirstOrDefault(j => j.JobId == jobId);
                if (job != null) Jobs.Remove(job);
                return (true, "Job deleted successfully");
            }
            catch
            {
                return (false, "We're sorry, an error occurred. The job was not deleted. Please try again.");
            }
        }

        // ── Applicant management ──────────────────────────────────────
        public void ViewApplicantsForJob(JobPosting job)
        {
            SelectedJob = job;
            Applicants.Clear();
            if (SelectedJob != null)
            {
                try
                {
                    var applicants = _applicantService.GetApplicantsForJob(SelectedJob);
                    foreach (var a in applicants) Applicants.Add(a);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                    Applicants.Add(new Applicant
                    {
                        ApplicantId = 999,
                        User = new OurApp.Core.Models.User(1, "Mock Applicant (DB Error)", ""),
                        ApplicationStatus = "On Hold",
                        Job = SelectedJob
                    });
                }
            }
            CurrentStage = PageStage.Applicants;
        }

        public void GoBack()
        {
            if (CurrentStage == PageStage.Details)
            {
                CurrentStage = PageStage.Applicants;
                SelectedApplicant = null;
            }
            else if (CurrentStage == PageStage.Applicants)
            {
                CurrentStage = PageStage.Jobs;
                SelectedJob = null;
                Applicants.Clear();
            }
        }

        private void LoadDraft(Applicant applicant)
        {
            DraftStatus           = applicant.ApplicationStatus;
            DraftAppTestGrade     = applicant.AppTestGrade?.ToString()     ?? "";
            DraftCvGrade          = applicant.CvGrade?.ToString()          ?? "";
            DraftCompanyTestGrade = applicant.CompanyTestGrade?.ToString() ?? "";
            DraftInterviewGrade   = applicant.InterviewGrade?.ToString()   ?? "";
        }

        public void SaveChanges()
        {
            if (SelectedApplicant == null) return;

            SelectedApplicant.ApplicationStatus = DraftStatus;
            if (decimal.TryParse(DraftAppTestGrade,     out decimal t1)) SelectedApplicant.AppTestGrade     = t1;
            else if (string.IsNullOrWhiteSpace(DraftAppTestGrade))       SelectedApplicant.AppTestGrade     = null;
            if (decimal.TryParse(DraftCvGrade,          out decimal t2)) SelectedApplicant.CvGrade          = t2;
            else if (string.IsNullOrWhiteSpace(DraftCvGrade))            SelectedApplicant.CvGrade          = null;
            if (decimal.TryParse(DraftCompanyTestGrade, out decimal t3)) SelectedApplicant.CompanyTestGrade = t3;
            else if (string.IsNullOrWhiteSpace(DraftCompanyTestGrade))   SelectedApplicant.CompanyTestGrade = null;
            if (decimal.TryParse(DraftInterviewGrade,   out decimal t4)) SelectedApplicant.InterviewGrade   = t4;
            else if (string.IsNullOrWhiteSpace(DraftInterviewGrade))     SelectedApplicant.InterviewGrade   = null;

            _applicantService.UpdateApplicant(SelectedApplicant);

            int index = Applicants.IndexOf(SelectedApplicant);
            if (index >= 0) Applicants[index] = SelectedApplicant;

            GoBack();
        }

        public void RemoveApplicant(Applicant applicant)
        {
            if (applicant == null) return;
            _applicantService.RemoveApplicant(applicant.ApplicantId);
            Applicants.Remove(applicant);
            if (SelectedApplicant == applicant) GoBack();
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
