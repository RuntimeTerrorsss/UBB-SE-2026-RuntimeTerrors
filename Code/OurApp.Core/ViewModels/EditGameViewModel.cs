using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace OurApp.Core.ViewModels
{
    public partial class EditGameViewModel : ObservableObject
    {
        private readonly GameService _service;
        private readonly GameValidator _gameValidator = new GameValidator();

        public ObservableCollection<ScenarioInput> Scenarios { get; } = new ObservableCollection<ScenarioInput>();

        public ObservableCollection<int> AvailableBuddyIds { get; } = new ObservableCollection<int> { 0, 1 };

        [ObservableProperty]
        private int _selectedBuddyId = 1;

       
        public string BuddyImagePath => BuddyImageProvider.GetImagePathById(SelectedBuddyId);

     
        partial void OnSelectedBuddyIdChanged(int value)
        {
            OnPropertyChanged(nameof(BuddyImagePath));
        }


        [ObservableProperty]
        private string _buddyName = string.Empty;

        [ObservableProperty]
        private string _buddyIntroduction = string.Empty;

        [ObservableProperty]
        private string _conclusion = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public EditGameViewModel(GameService service)
        {
            _service = service;

            for (int i = 0; i < 2; i++)
            {
                var s = new ScenarioInput();

                s.Choices.Add(new AdviceChoiceInput());
                s.Choices.Add(new AdviceChoiceInput());
                s.Choices.Add(new AdviceChoiceInput());
                Scenarios.Add(s);
            }

            ApplyLoadedGame(_service.GetStoredGame());
            StatusMessage = string.Empty;
        }

        private void ApplyLoadedGame(Game game)
        {
            if (game == null)
                return;

            SelectedBuddyId = game.Buddy.Id;
            BuddyName = game.Buddy.Name ?? string.Empty;
            BuddyIntroduction = game.Buddy.Introduction ?? string.Empty;
            Conclusion = game.Conclusion ?? string.Empty;

            for (int i = 0; i < Scenarios.Count && i < game.Scenarios.Count; i++)
            {
                var scenarioVm = Scenarios[i];
                var scenarioModel = game.Scenarios[i];
                scenarioVm.ScenarioText = scenarioModel.Description ?? string.Empty;

                var adviceChoices = scenarioModel.AdviceChoices;
                for (int j = 0; j < scenarioVm.Choices.Count && j < adviceChoices.Count; j++)
                {
                    scenarioVm.Choices[j].Advice = adviceChoices[j].Advice ?? string.Empty;
                    scenarioVm.Choices[j].Feedback = adviceChoices[j].Feedback ?? string.Empty;
                }
            }
        }

        [RelayCommand]
        private void CreateGame()
        {
            try
            {
                var scenarioTuples = Scenarios
                    .Select(s => (
                        scenarioText: s.ScenarioText ?? string.Empty,
                        choices: (IReadOnlyList<(string advice, string feedback)>)s.Choices
                            .Select(c => (
                                advice: c.Advice ?? string.Empty,
                                feedback: c.Feedback ?? string.Empty))
                            .ToList()
                    ))
                    .ToList();

                _gameValidator.ValidateForActivation(scenarioTuples, Conclusion ?? string.Empty);

                var game = _service.CreateGameFromInput(
                    buddyId: SelectedBuddyId,
                    buddyName: BuddyName,
                    buddyIntroduction: BuddyIntroduction,
                    scenarios: scenarioTuples,
                    conclusion: Conclusion ?? string.Empty,
                    publish: false);

                _service.Save(game);
                StatusMessage = "Game created and saved successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to create game: {ex.Message}";
            }
        }
    }

    public partial class ScenarioInput : ObservableObject
    {
        [ObservableProperty]
        private string _scenarioText = string.Empty;

        public ObservableCollection<AdviceChoiceInput> Choices { get; } = new ObservableCollection<AdviceChoiceInput>();
    }

    public partial class AdviceChoiceInput : ObservableObject
    {
        [ObservableProperty]
        private string _advice = string.Empty;

        [ObservableProperty]
        private string _feedback = string.Empty;
    }
}
