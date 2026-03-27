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
        IEventsRepo repository;
        public EventsService(IEventsRepo repo) 
        {
            this.repository = repo;
        }

        public void AddEvent(string photo, string title, string description, DateTime start, DateTime end, string location)
        {
            Event e = new Event { Photo = photo, Title = title, Description = description, StartDate = start, EndDate = end, Location = location};
            this.repository.Add(e);
        }
        
        public void printAll()
        {
            this.repository.printAll();
        }

        public ObservableCollection<Event> GetAllEvents()
        {
            return this.repository.GetAll();
        }

        public ObservableCollection<Event> GetCurrentEvents()
        {
            return this.repository.getCurrentEvents();
        }

        public ObservableCollection<Event> GetPastEvents()
        {
            return this.repository.getPastEvents();
        }
    }
}
