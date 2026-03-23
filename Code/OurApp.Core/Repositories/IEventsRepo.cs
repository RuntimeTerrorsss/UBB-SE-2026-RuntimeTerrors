using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public interface IEventsRepo
    {
        ObservableCollection<Event> GetAll();
        void Add(Event e);
        void Remove(Event e);
        public void printAll();
        ObservableCollection<Event> getCurrentEvents();
        ObservableCollection<Event> getPastEvents();
        void Update(int id, string photo, string title, string description, DateTime start, DateTime end, string location);

    }
}
