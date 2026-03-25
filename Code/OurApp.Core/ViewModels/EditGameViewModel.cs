using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OurApp.Core.ViewModels
{
    public partial class EditGameViewModel : ObservableObject
    {
        private readonly GameService _service;

        public ObservableCollection<ScenarioInput> Scenarios { get; } = new ObservableCollection<ScenarioInput>();

        [ObservableProperty]
        private string _buddyIdText = "1";

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
                var s = new ScenarioInput
                {
                    ScenarioText = string.Empty
                };

                s.Choices.Add(new AdviceChoiceInput());
                s.Choices.Add(new AdviceChoiceInput());
                Scenarios.Add(s);
            }

            StatusMessage = string.Empty;
        }

        [RelayCommand]
        private void CreateGame()
        {
            try
            {
                int buddyId = 1;
                if (!int.TryParse(BuddyIdText, out buddyId))
                    buddyId = 1;

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


                var game = _service.CreateGameFromInput(
                    buddyId: buddyId,
                    buddyName: BuddyName,
                    buddyIntroduction: BuddyIntroduction,
                    scenarios: scenarioTuples,
                    conclusion: Conclusion,
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
