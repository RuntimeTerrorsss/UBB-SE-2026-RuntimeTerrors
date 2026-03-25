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

    }
}
