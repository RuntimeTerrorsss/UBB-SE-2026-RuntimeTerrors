using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
                if (e.EndDate.Date >= DateTime.Now.Date)
                {
                    currentEvents.Add(e);
                }
            }
            return currentEvents;
        }

        public ObservableCollection<Event> getPastEvents()
        {
            ObservableCollection<Event> pastEvents = new ObservableCollection<Event>();
            foreach (Event e in events)
            {
                if (e.EndDate.Date < DateTime.Now.Date)
                {
                    pastEvents.Add(e);
                }
            }
            return pastEvents;
        }


        public void Update(int id, string photo, string title, string description, DateTime start, DateTime end, string location)
        {
            foreach (Event e in events)
            {
                if (e.Id == id)
                {
                    e.Photo = photo;
                    e.Title = title;
                    e.Description = description;
                    e.StartDate = start;
                    e.EndDate = end;
                    e.Location = location;
                    return;
                }
            }
        }
    }
}
