using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    internal interface IEventsRepo
    {
        void Add(Event e);
        void Remove(Event e);
    }
}
