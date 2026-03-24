using CommunityToolkit.Mvvm.DependencyInjection;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public class EventsService
    {
        IEventsRepo eventsRepository;
        public EventsService(IEventsRepo eventsRepo) 
        {
            this.eventsRepository = eventsRepo;
        }

        public void AddEvent(string eventPhoto, string eventTitle, string eventDescription, DateTime eventStartDate, DateTime eventEndDate, string eventLocation, List<Company> collaborators)
        {
            Event eventToBeAdded = new Event(eventPhoto, eventTitle, eventDescription, eventStartDate, eventEndDate, eventLocation, 1, collaborators ?? new List<Company>());
            this.eventsRepository.AddEventToRepo(eventToBeAdded);
        }

        public void UpdateEvent(int eventIdToBeUpdated, string newEventPhoto, string newEventTitle, string newEventDescription, DateTime newEventStartDate, DateTime newEventEndDate, string newEventLocation)
        {
            this.eventsRepository.UpdateEventToRepo(eventIdToBeUpdated, newEventPhoto, newEventTitle, newEventDescription, newEventStartDate, newEventEndDate, newEventLocation);
        }

        public void printAll()
        {
            this.eventsRepository.printAll();
        }

        //public ObservableCollection<Event> GetAllEvents()
        //{
        //    return this.eventsRepository.GetCollectionFromRepo();
        //}

        public ObservableCollection<Event> GetCurrentEvents()
        {
            return this.eventsRepository.getCurrentEventsFromRepo();
        }

        public ObservableCollection<Event> GetPastEvents()
        {
            return this.eventsRepository.getPastEventsFromRepo();
        }
    }
}
