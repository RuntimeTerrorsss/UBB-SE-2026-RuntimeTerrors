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
    }
}
