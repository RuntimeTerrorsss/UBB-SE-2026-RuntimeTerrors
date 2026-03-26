using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
using OurApp.Core.Services;
using System;
using System.Collections.ObjectModel;

namespace OurApp.Core.ViewModels;

/// <summary>Rows for the "Posted jobs" / "Events" preview lists on the company view profile page.</summary>
public sealed class CompanyProfileListRow
{
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
}

/// <summary>One trending skill row for the statistics sidebar (frontend display; fill from analytics later).</summary>
public sealed class CompanyTrendingSkillRow
{
    public string Rank { get; set; } = "";
    public string SkillName { get; set; } = "";
    public string Detail { get; set; } = "";
}

public partial class CompanyProfileViewModel : ObservableObject
{
    private readonly ICompanyService _companyService;
    private readonly ProfileCompletionCalculator _calculator;

    [ObservableProperty]
    private Company? _company;

    [ObservableProperty]
    private string _loadMessage = "";

    [ObservableProperty]
    private int _completionPercentage;

    [ObservableProperty]
    private int _completedTasksCount;

    [ObservableProperty]
    private ObservableCollection<string> _remainingTasks = new();

    [ObservableProperty]
    private string _applicantSummary = "Applicant trends (vs last month) will show here when connected to reporting.";

    [ObservableProperty]
    private ObservableCollection<CompanyTrendingSkillRow> _trendingSkills = new();

    [ObservableProperty]
    private ObservableCollection<CompanyProfileListRow> _jobPreview = new();

    [ObservableProperty]
    private ObservableCollection<CompanyProfileListRow> _eventPreview = new();

    [ObservableProperty]
    private ObservableCollection<string> _collaboratorLines = new();

    /// <summary>Fired when the user chooses Edit profile (page should navigate and pass <see cref="CompanyId"/>).</summary>
    public event EventHandler? NavigateEditProfileRequested;

    public int CompanyId { get; private set; }

    public CompanyProfileViewModel(ICompanyService companyService, ProfileCompletionCalculator calculator)
    {
        _companyService = companyService;
        _calculator = calculator;
    }

    public void Load(int companyId)
    {
        CompanyId = companyId;
        Company = _companyService.GetCompanyById(companyId);
        if (Company is null)
        {
            LoadMessage = "We could not load this company profile.";
            CompletionPercentage = 0;
            CompletedTasksCount = 0;
            RemainingTasks.Clear();
            return;
        }

        LoadMessage = "";
        RefreshProfileStatistics();
        FillPreviewSections();
    }

    public void RefreshProfileStatistics()
    {
        if (Company is null)
            return;

        var (percentage, tasks) = _calculator.Calculate(Company);
        CompletionPercentage = percentage;
        CompletedTasksCount = 5 - tasks.Count;
        if (CompletedTasksCount < 0)
            CompletedTasksCount = 0;

        RemainingTasks.Clear();
        foreach (var task in tasks)
            RemainingTasks.Add(task);
    }

    private void FillPreviewSections()
    {
        if (Company is null)
            return;

        CollaboratorLines.Clear();
        if (Company.CollaboratorsCount > 0 || Company.Collaborators.Count > 0)
        {
            var n = Math.Max(Company.CollaboratorsCount, Company.Collaborators.Count);
            CollaboratorLines.Add($"{n} collaborator compan{(n == 1 ? "y" : "ies")} (see Events to manage invitations).");
        }
        else
            CollaboratorLines.Add("No collaborators yet — collaborate on an event to connect with other companies.");

        JobPreview.Clear();
        JobPreview.Add(new CompanyProfileListRow
        {
            Title = "Job listings",
            Subtitle = Company.PostedJobsCount > 0
                ? $"{Company.PostedJobsCount} job post(s) on file. Open “Our jobs” when that page is wired."
                : "No jobs yet. Use Create Post from your jobs page."
        });

        EventPreview.Clear();
        EventPreview.Add(new CompanyProfileListRow
        {
            Title = "Events",
            Subtitle = "Open “Our events” for the full list. This block is a preview placeholder until events are bound here."
        });

        TrendingSkills.Clear();
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "1", SkillName = "—", Detail = "Hook job_skills + skills API" });
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "2", SkillName = "—", Detail = "Top skills across all postings" });
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "3", SkillName = "—", Detail = "Will rank by frequency" });
    }

    [RelayCommand]
    private void EditProfile()
    {
        NavigateEditProfileRequested?.Invoke(this, EventArgs.Empty);
    }
}
