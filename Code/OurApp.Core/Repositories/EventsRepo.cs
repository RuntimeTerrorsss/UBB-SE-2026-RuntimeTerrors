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
        ObservableCollection<Event> eventsCollection;

        public EventsRepo() 
        {
            eventsCollection = new ObservableCollection<Event>();
        }

        public void printAll()
        {
            for (int i = 0; i < eventsCollection.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{eventsCollection[i]} ");
            }
        }

        public ObservableCollection<Event> GetCollectionFromRepo()
        {
            return eventsCollection;
        }

        public void AddEventToRepo(Event eventToBeAdded)
        {
            eventsCollection.Add(eventToBeAdded);
        }

        public void RemoveEventFromRepo(Event eventToBeRemoved)
        {
            eventsCollection.Remove(eventToBeRemoved);
        }

        public ObservableCollection<Event> getCurrentEventsFromRepo()
        {
            ObservableCollection<Event> currentEvents = new ObservableCollection<Event>();

            foreach (Event @event in eventsCollection)
            {
                DateTime eventEndDate = @event.EndDate;
                DateTime todaysDate = DateTime.Now;

                if (eventEndDate.Date >= todaysDate.Date)
                {
                    currentEvents.Add(@event);
                }
            }

            return currentEvents;
        }

        public ObservableCollection<Event> getPastEventsFromRepo()
        {
            ObservableCollection<Event> pastEvents = new ObservableCollection<Event>();

            foreach (Event @event in eventsCollection)
            {
                DateTime eventEndDate = @event.EndDate;
                DateTime todaysDate = DateTime.Now;

                if (eventEndDate.Date < todaysDate.Date)
                {
                    pastEvents.Add(@event);
                }
            }

            return pastEvents;
        }


        public void UpdateEventToRepo(int eventIdToBeUpdated, string newEventPhoto, string newEventTitle, string newEventDescription, DateTime newEventStartDate, DateTime newEventEndDate, string newEventLocation)
        {
            foreach (Event @event in eventsCollection)
            {
                if (@event.Id == eventIdToBeUpdated)
                {
                    @event.Photo = newEventPhoto;
                    @event.Title = newEventTitle;
                    @event.Description = newEventDescription;
                    @event.StartDate = newEventStartDate;
                    @event.EndDate = newEventEndDate;
                    @event.Location = newEventLocation;
                    return;
                }
            }
        }
    }
}
