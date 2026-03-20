using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public class GameViewModel
    {
        private readonly GameService _gameService;
        private int _currentScenarioIndex = 0;

        // Acestea sunt „ecranele” pe care le va vedea utilizatorul în WinUI
        public string WelcomeMessage { get; set; }
        public string CurrentQuestion { get; set; }
        public List<string> CurrentChoices { get; set; }
        public string Feedback { get; set; }

        public GameViewModel(GameService gameService)
        {
            _gameService = gameService;

            // Inițializăm datele de start apelând metodele tale din Service
            WelcomeMessage = _gameService.ShowCoworker();
            UpdateScenario();
        }

        // Metodă care „reîmprospătează” datele pentru scenariul curent
        private void UpdateScenario()
        {
            CurrentQuestion = _gameService.ShowScenarioText(_currentScenarioIndex);
            CurrentChoices = _gameService.ShowChoices(_currentScenarioIndex);
        }

        // Metodă pe care o vei apela când utilizatorul apasă un buton
        public void OnChoiceSelected(int adviceIndex)
        {
            Feedback = _gameService.ChoiceMade(_currentScenarioIndex, adviceIndex);

            // Trecem la următorul scenariu dacă mai există
            _currentScenarioIndex++;
            // Aici vei adăuga logica: dacă s-au terminat scenariile, arată concluzia!
        }
    }
}
