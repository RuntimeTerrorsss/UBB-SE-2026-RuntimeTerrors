using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
using OurApp.Core.Services;
using System;
using System.Collections.ObjectModel;

namespace OurApp.Core.ViewModels;

public sealed class CompanyCollabListRow
{
    public string Name { get; set; } = "";
}

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
    private readonly GameService _gameService;
    IEventsService eventService;
    SessionService sessionService;
    ICollaboratorsService collabService;
    private readonly ProfileCompletionCalculator _calculator;
    private int _currentScenarioIndex;

    [ObservableProperty]
    private string _currentQuestion = string.Empty;

    [ObservableProperty]
    private List<string>? _currentChoices;

    [ObservableProperty]
    private string _feedback = string.Empty;

    public string BuddyImagePath => BuddyImageProvider.GetImagePathById(_gameService.getBuddyId());

    [ObservableProperty]
    private string _welcomeMessage = string.Empty;

    [ObservableProperty]
    private GameState _currentState = GameState.NotCompleted;

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

    public IEnumerable<CompanyProfileListRow> Top3JobPreviews =>
        eventService
            .GetCurrentEvents(sessionService.loggedInUser.CompanyId)
            .Take(3)
            .Select(e => new CompanyProfileListRow
            {
                Title = e.Title,
                Subtitle = e.Description
            });
    public IEnumerable<CompanyProfileListRow> Top3EventPreviews => eventService
            .GetCurrentEvents(sessionService.loggedInUser.CompanyId)
            .Take(3)
            .Select(e => new CompanyProfileListRow
            {
                Title = e.Title,
                Subtitle = e.Description
            });

    public IEnumerable<CompanyCollabListRow> Top3CollabsPreviews => collabService
            .GetAllCollaborators(sessionService.loggedInUser.CompanyId)
            .Take(3)
            .Select(e => new CompanyCollabListRow
            {
                Name = e.Name
            });
   
    public event EventHandler? NavigateAllCollaboratorRequested;
    public event EventHandler? NavigateEditProfileRequested;
    public event EventHandler? NavigateAllEventsRequested;
    public event EventHandler? NavigateAllJobsRequested;

    public int CompanyId { get; private set; }

    public CompanyProfileViewModel(ICompanyService companyService, ProfileCompletionCalculator calculator, GameService gameService, IEventsService eventService, SessionService sessionService, ICollaboratorsService collaboratorsService)
    {
        _gameService = gameService;
        _companyService = companyService;
        _calculator = calculator;
        gamePreview();
        this.eventService = eventService;
        this.sessionService = sessionService;
        this.collabService = collaboratorsService;

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

        TrendingSkills.Clear();
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "1", SkillName = "—", Detail = "percent" });
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "2", SkillName = "—", Detail = "percent" });
        TrendingSkills.Add(new CompanyTrendingSkillRow { Rank = "3", SkillName = "—", Detail = "percent" });

        OnPropertyChanged(nameof(Top3JobPreviews));
        OnPropertyChanged(nameof(Top3EventPreviews));
        OnPropertyChanged(nameof(Top3CollabsPreviews));
    }

    [RelayCommand]
    private void SeeAllCollaborators()
    {
        NavigateAllCollaboratorRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void EditProfile()
    {
        NavigateEditProfileRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void SeeAllEvents()
    {
        NavigateAllEventsRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void SeeAllJobs()
    {
        NavigateAllJobsRequested?.Invoke(this, EventArgs.Empty);
    }


    //Game

    partial void OnCurrentStateChanged(GameState value)
    {
        OnPropertyChanged(nameof(IncompleteGame));
        OnPropertyChanged(nameof(IsStartVisible));
        OnPropertyChanged(nameof(IsChoice1Visible));
        OnPropertyChanged(nameof(IsReaction1Visible));
        OnPropertyChanged(nameof(IsChoice2Visible));
        OnPropertyChanged(nameof(IsReaction2Visible));
        OnPropertyChanged(nameof(IsConclusionVisible));
        OnPropertyChanged(nameof(IsChoiceActive));
        OnPropertyChanged(nameof(IsReactionActive));
    }

    public bool IncompleteGame => CurrentState == GameState.NotCompleted;
    public bool IsStartVisible => CurrentState == GameState.Start;
    public bool IsChoice1Visible => CurrentState == GameState.Choices1;
    public bool IsReaction1Visible => CurrentState == GameState.Reaction1;
    public bool IsChoice2Visible => CurrentState == GameState.Choices2;
    public bool IsReaction2Visible => CurrentState == GameState.Reaction2;
    public bool IsConclusionVisible => CurrentState == GameState.Conclusion;

    public bool IsChoiceActive => IsChoice1Visible || IsChoice2Visible;
    public bool IsReactionActive => IsReaction1Visible || IsReaction2Visible;

    private void UpdateScenario()
    {

        if (_currentScenarioIndex < 2)
        {
            CurrentQuestion = _gameService.ShowScenarioText(_currentScenarioIndex);
            CurrentChoices = _gameService.ShowChoices(_currentScenarioIndex);
        }


    }
    public void gamePreview()
    {
        if (_gameService.isPublished())
        {
            WelcomeMessage = _gameService.ShowCoworker();
            UpdateScenario();
        }

    }


    [RelayCommand]
    private void StartGame()
    {
        CurrentState = GameState.Choices1;
    }

    [RelayCommand]
    private void SelectChoice(string? choiceText)
    {
        if (string.IsNullOrEmpty(choiceText) || CurrentChoices == null)
            return;

        int adviceIndex = CurrentChoices.IndexOf(choiceText);
        if (adviceIndex < 0)
            return;

        Feedback = _gameService.ChoiceMade(_currentScenarioIndex, adviceIndex);
        CurrentState = _currentScenarioIndex == 0 ? GameState.Reaction1 : GameState.Reaction2;
    }

    [RelayCommand]
    private void GoToNextStep()
    {
        _currentScenarioIndex++;

        if (_currentScenarioIndex < 2)
        {
            UpdateScenario();
            CurrentState = GameState.Choices2;
        }
        else
        {
            Feedback = _gameService.ShowConclusion();
            CurrentState = GameState.Conclusion;
        }
    }

}