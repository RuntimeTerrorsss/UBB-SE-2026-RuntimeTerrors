using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
     public class GameRepo
    {
            public Game GetGameById(int id)
            {
                // aici vei face query în DB
                // SELECT ...
                return new Game();
            }

            public void SaveGame(Game game)
            {
                // INSERT / UPDATE în DB
            }
        }
    }
