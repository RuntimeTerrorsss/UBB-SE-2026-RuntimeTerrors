using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public interface IGameRepo
    {
        Game Get();
        void Save(Game game);
    }
}
