using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class GameMemoryRepo : IGameRepo
    {
        private Game game;

        public GameMemoryRepo()
        {
            game = new Game();
        }

        public Game Get()
        {
            return game;
        }

        public void Save(Game updatedGame)
        {
            game = updatedGame;
        }

        public Game GetGame(int companyId)
        {
            // In-memory repo currently stores a single game regardless of company.
            _ = companyId;
            return game;
        }

        public void SaveGame(Game game, int companyId)
        {
            // In-memory repo currently stores a single game regardless of company.
            _ = companyId;
            Save(game);
        }
    }
}
