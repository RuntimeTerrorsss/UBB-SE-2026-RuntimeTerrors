using OurApp.Core.Models;
using System;

namespace OurApp.Core.Repositories
{
    public class GameRepo : IGameRepo
    {
        public Game GetGameById(int id)
        {
            // DB: SELECT ...
            _ = id;
            return new Game();
        }

        public void SaveGame(Game game)
        {
            // DB: INSERT / UPDATE
            _ = game;
        }

        public Game Get() => GetGameById(0);

        public void Save(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            SaveGame(game);
        }
    }
}
