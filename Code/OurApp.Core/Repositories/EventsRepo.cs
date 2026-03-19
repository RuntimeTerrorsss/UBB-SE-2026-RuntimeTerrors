using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class EventsRepo
    {
        List<Event> events;

        public EventsRepo() 
        {
            events = new List<Event>();
        }

        public void printAll()
        {
            for (int i = 0; i < events.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{events[i]} ");
            }
        }

        public void Add(Event e)
        {
            events.Add(e);
        }

        public void Remove(Event e)
        {
            events.Remove(e);
        }
    }
}
