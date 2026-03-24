using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OurApp.Core.Services
{
    public class GameService
    {
        private readonly IGameRepo _repository;

        public GameService(IGameRepo repository)
        {
            _repository = repository;
        }

        private Game LoadedGame()
        {
            var game = _repository.Get();
            if (game == null)
                throw new InvalidOperationException("No game is available from the repository.");
            return game;
        }

        public void Save(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            _repository.Save(game);
        }

        public string ShowCoworker()
        {
            return LoadedGame().Buddy.Introduction;
        }

        public string ShowScenarioText(int number)
        {
            var game = LoadedGame();
            if (number < 0 || number >= game.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(number));
            return game.Scenarios[number].Description;
        }

        public List<string> ShowChoices(int number)
        {
            var game = LoadedGame();
            if (number < 0 || number >= game.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(number));
            return game.Scenarios[number].GetAdviceTexts();
        }

        public string ChoiceMade(int numberScenario, int numberAdvice)
        {
            var game = LoadedGame();
            if (numberScenario < 0 || numberScenario >= game.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(numberScenario));
            return game.Scenarios[numberScenario].SelectChoice(numberAdvice);
        }

        public string ShowConclusion()
        {
            return LoadedGame().Conclusion;
        }

        public Game CreateGameFromInput(
            int buddyId,
            string buddyName,
            string buddyIntroduction,
            IReadOnlyList<(string scenarioText, IReadOnlyList<(string advice, string feedback)> choices)> scenarios,
            string conclusion,
            bool publish = false)
        {
            var buddy = new Buddy(buddyId, buddyName, buddyIntroduction);

            var builtScenarios = scenarios
                .Select(s =>
                {
                    var scenario = new Scenario(s.scenarioText);
                    foreach (var (advice, feedback) in s.choices)
                    {
                        scenario.AddChoice(new AdviceChoice(advice, feedback));
                    }

                    return scenario;
                })
                .ToList();

            return new Game(buddy, builtScenarios, conclusion, publish);
        }

        public Game UpdateAdviceChoice(Game existingGame, int scenarioIndex, int choiceIndex, string newAdvice, string newFeedback)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            if (scenarioIndex < 0 || scenarioIndex >= existingGame.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(scenarioIndex));

            var oldScenario = existingGame.Scenarios[scenarioIndex];
            if (choiceIndex < 0 || choiceIndex >= oldScenario.AdviceChoices.Count)
                throw new ArgumentOutOfRangeException(nameof(choiceIndex));

            var newScenario = new Scenario(oldScenario.Description);
            for (int i = 0; i < oldScenario.AdviceChoices.Count; i++)
            {
                var oldChoice = oldScenario.AdviceChoices[i];
                newScenario.AddChoice(i == choiceIndex
                    ? new AdviceChoice(newAdvice, newFeedback)
                    : new AdviceChoice(oldChoice.Advice, oldChoice.Feedback));
            }

            var newScenarios = existingGame.Scenarios.ToList();
            newScenarios[scenarioIndex] = newScenario;

            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game UpdateBuddy(Game existingGame, int buddyId, string buddyName, string buddyIntroduction)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            var newBuddy = new Buddy(buddyId, buddyName, buddyIntroduction);
            return new Game(newBuddy, existingGame.Scenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game UpdateConclusion(Game existingGame, string newConclusion)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            return new Game(existingGame.Buddy, existingGame.Scenarios, newConclusion, existingGame.IsPublished);
        }

        public Game PublishGame(Game existingGame)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            return new Game(existingGame.Buddy, existingGame.Scenarios, existingGame.Conclusion, isPublished: true);
        }

        public Game UnpublishGame(Game existingGame)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            return new Game(existingGame.Buddy, existingGame.Scenarios, existingGame.Conclusion, isPublished: false);
        }

        public Game UpdateScenarioText(Game existingGame, int scenarioIndex, string newScenarioText)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            if (scenarioIndex < 0 || scenarioIndex >= existingGame.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(scenarioIndex));

            var oldScenario = existingGame.Scenarios[scenarioIndex];
            var updatedScenario = CloneScenarioWithText(oldScenario, newScenarioText);

            var newScenarios = existingGame.Scenarios.ToList();
            newScenarios[scenarioIndex] = updatedScenario;
            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game AddScenario(Game existingGame, string scenarioText, IReadOnlyList<(string advice, string feedback)> choices, int? insertAtIndex = null)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));

            var newScenario = new Scenario(scenarioText);
            foreach (var (advice, feedback) in choices)
            {
                newScenario.AddChoice(new AdviceChoice(advice, feedback));
            }

            var newScenarios = existingGame.Scenarios.ToList();
            if (insertAtIndex == null)
            {
                newScenarios.Add(newScenario);
            }
            else
            {
                var idx = insertAtIndex.Value;
                if (idx < 0 || idx > newScenarios.Count) throw new ArgumentOutOfRangeException(nameof(insertAtIndex));
                newScenarios.Insert(idx, newScenario);
            }

            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game RemoveScenario(Game existingGame, int scenarioIndex)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            if (scenarioIndex < 0 || scenarioIndex >= existingGame.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(scenarioIndex));

            var newScenarios = existingGame.Scenarios.ToList();
            newScenarios.RemoveAt(scenarioIndex);
            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game AddAdviceChoice(Game existingGame, int scenarioIndex, string advice, string feedback, int? insertAtIndex = null)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            if (scenarioIndex < 0 || scenarioIndex >= existingGame.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(scenarioIndex));

            var oldScenario = existingGame.Scenarios[scenarioIndex];
            var updatedScenario = CloneScenario(oldScenario);

            var newChoice = new AdviceChoice(advice, feedback);
            if (insertAtIndex == null)
            {
                updatedScenario.AddChoice(newChoice);
            }
            else
            {
                var idx = insertAtIndex.Value;
                if (idx < 0 || idx > oldScenario.AdviceChoices.Count) throw new ArgumentOutOfRangeException(nameof(insertAtIndex));

                updatedScenario = CloneScenarioInsertChoice(oldScenario, idx, newChoice);
            }

            var newScenarios = existingGame.Scenarios.ToList();
            newScenarios[scenarioIndex] = updatedScenario;
            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        public Game RemoveAdviceChoice(Game existingGame, int scenarioIndex, int choiceIndex)
        {
            if (existingGame == null) throw new ArgumentNullException(nameof(existingGame));
            if (scenarioIndex < 0 || scenarioIndex >= existingGame.Scenarios.Count)
                throw new ArgumentOutOfRangeException(nameof(scenarioIndex));

            var oldScenario = existingGame.Scenarios[scenarioIndex];
            if (choiceIndex < 0 || choiceIndex >= oldScenario.AdviceChoices.Count)
                throw new ArgumentOutOfRangeException(nameof(choiceIndex));

            var updatedScenario = new Scenario(oldScenario.Description);
            for (int i = 0; i < oldScenario.AdviceChoices.Count; i++)
            {
                if (i == choiceIndex) continue;
                var c = oldScenario.AdviceChoices[i];
                updatedScenario.AddChoice(new AdviceChoice(c.Advice, c.Feedback));
            }

            var newScenarios = existingGame.Scenarios.ToList();
            newScenarios[scenarioIndex] = updatedScenario;
            return new Game(existingGame.Buddy, newScenarios, existingGame.Conclusion, existingGame.IsPublished);
        }

        private static Scenario CloneScenario(Scenario scenario)
        {
            var cloned = new Scenario(scenario.Description);
            foreach (var c in scenario.AdviceChoices)
            {
                cloned.AddChoice(new AdviceChoice(c.Advice, c.Feedback));
            }
            return cloned;
        }

        private static Scenario CloneScenarioWithText(Scenario scenario, string newText)
        {
            var cloned = new Scenario(newText);
            foreach (var c in scenario.AdviceChoices)
            {
                cloned.AddChoice(new AdviceChoice(c.Advice, c.Feedback));
            }
            return cloned;
        }

        private static Scenario CloneScenarioInsertChoice(Scenario scenario, int insertAtIndex, AdviceChoice newChoice)
        {
            var cloned = new Scenario(scenario.Description);
            for (int i = 0; i < scenario.AdviceChoices.Count + 1; i++)
            {
                if (i == insertAtIndex)
                {
                    cloned.AddChoice(newChoice);
                    continue;
                }

                var oldIndex = i < insertAtIndex ? i : i - 1;
                var c = scenario.AdviceChoices[oldIndex];
                cloned.AddChoice(new AdviceChoice(c.Advice, c.Feedback));
            }

            return cloned;
        }
    }
}
