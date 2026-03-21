using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class EventsRepo : IEventsRepo
    {
        ObservableCollection<Event> events;

        public EventsRepo() 
        {
            events = new ObservableCollection<Event>();
        }

        public void printAll()
        {
            for (int i = 0; i < events.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{events[i]} ");
            }
        }

        public ObservableCollection<Event> GetAll()
        {
            return events;
        }

        public void Add(Event e)
        {
            events.Add(e);
        }

        public void Remove(Event e)
        {
            events.Remove(e);
        }

        public ObservableCollection<Event> getCurrentEvents()
        {
            ObservableCollection<Event> currentEvents = new ObservableCollection<Event>();
            foreach (Event e in events)
            {
                if (e.StartDate >= DateTime.Now)
                {
                    currentEvents.Add(e);
                }
            }
            return currentEvents;
        }
    }
}
