using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Services;
using System;
using System.Collections.Generic;

namespace OurApp.Core.ViewModels
{
    public partial class GameViewModel : ObservableObject
    {
        private readonly GameService _gameService;
        private readonly Action? _exitApplication;

        private int _currentScenarioIndex;

        [ObservableProperty]
        private GameState _currentState = GameState.Start;

        partial void OnCurrentStateChanged(GameState value)
        {
            OnPropertyChanged(nameof(IsStartVisible));
            OnPropertyChanged(nameof(IsChoice1Visible));
            OnPropertyChanged(nameof(IsReaction1Visible));
            OnPropertyChanged(nameof(IsChoice2Visible));
            OnPropertyChanged(nameof(IsReaction2Visible));
            OnPropertyChanged(nameof(IsConclusionVisible));
            OnPropertyChanged(nameof(IsChoiceActive));
            OnPropertyChanged(nameof(IsReactionActive));
        }
        
        public bool IsStartVisible => CurrentState == GameState.Start;
        public bool IsChoice1Visible => CurrentState == GameState.Choices1;
        public bool IsReaction1Visible => CurrentState == GameState.Reaction1;
        public bool IsChoice2Visible => CurrentState == GameState.Choices2;
        public bool IsReaction2Visible => CurrentState == GameState.Reaction2;
        public bool IsConclusionVisible => CurrentState == GameState.Conclusion;

        public bool IsChoiceActive => IsChoice1Visible || IsChoice2Visible;
        public bool IsReactionActive => IsReaction1Visible || IsReaction2Visible;

        [ObservableProperty]
        private string _welcomeMessage = string.Empty;

        [ObservableProperty]
        private string _currentQuestion = string.Empty;

        [ObservableProperty]
        private List<string>? _currentChoices;

        [ObservableProperty]
        private string _feedback = string.Empty;

        public GameViewModel(GameService gameService, Action? exitApplication = null)
        {
            _gameService = gameService;
            _exitApplication = exitApplication;
            WelcomeMessage = _gameService.ShowCoworker();
            UpdateScenario();
        }

        private void UpdateScenario()
        {
            if (_currentScenarioIndex < 2)
            {
                CurrentQuestion = _gameService.ShowScenarioText(_currentScenarioIndex);
                CurrentChoices = _gameService.ShowChoices(_currentScenarioIndex);
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

        [RelayCommand]
        private void Exit()
        {
            _exitApplication?.Invoke();
        }
    }
}
