using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public class GameService
    {
        MockRepository repository;
        public GameService(MockRepository repo) {
            repository = repo;
        }

        public string ShowCoworker()
        {
            Buddy coworker = repository.Buddy;
            return "Hello! I am " + coworker.Name + ", your future coworker!";

        }

        public string ShowScenarioText(int number)
        {
            repository.Buddy.UpdateMood(false);
            return repository.GetScenario(number).Text;
        }

        public List<string> ShowChoices(int number)
        {
            return repository.GetScenario(number).ShowAdvices();
        }

        public string ChoiceMade(int numberScenario, int numberAdvice)
        {
            return repository.GetScenario(numberScenario).ChooseAdvice(numberAdvice);
        }

        public string ShowConclusion()
        {
            return repository.GetConclusion();
        }
    }
}
