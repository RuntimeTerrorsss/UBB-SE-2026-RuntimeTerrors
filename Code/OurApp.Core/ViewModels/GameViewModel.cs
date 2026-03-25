using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace OurApp.Core.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        private GameState _currentState = GameState.Start;
        public GameState CurrentState
        {
            get => _currentState;
            set
            {
                if (SetProperty(ref _currentState, value))
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
            }
        }

        public bool IsStartVisible => CurrentState == GameState.Start ? true : false;
        public bool IsChoice1Visible => CurrentState == GameState.Choices1 ? true : false;
        public bool IsReaction1Visible => CurrentState == GameState.Reaction1 ? true : false;
        public bool IsChoice2Visible => CurrentState == GameState.Choices2 ? true : false;
        public bool IsReaction2Visible => CurrentState == GameState.Reaction2 ? true : false;
        public bool IsConclusionVisible => CurrentState == GameState.Conclusion ? true : false;

        public bool IsChoiceActive => IsChoice1Visible || IsChoice2Visible;
        public bool IsReactionActive => IsReaction1Visible || IsReaction2Visible;


        private int _currentScenarioIndex = 0;

        public string WelcomeMessage { get; set; }

        private string _currentQuestion;
        public string CurrentQuestion {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }

        private List<string> _currentChoices;
        public List<string> CurrentChoices { 
            get =>_currentChoices; 
            set => SetProperty(ref _currentChoices, value); 
        }

        private string _feedback;
        public string Feedback {
            get => _feedback;
            set => SetProperty(ref _feedback, value);
        }

        public GameViewModel(GameService gameService)
        {
            _gameService = gameService;
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


        public void StartGame()
        {
            CurrentState = GameState.Choices1;
        }

        public void OnChoiceSelected(int adviceIndex)
        {
            if (adviceIndex < 0) return;

            Feedback = _gameService.ChoiceMade(_currentScenarioIndex, adviceIndex);

            CurrentState = _currentScenarioIndex == 0 ? GameState.Reaction1 : GameState.Reaction2;
        }

        public void GoToNextStep()
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
}
    