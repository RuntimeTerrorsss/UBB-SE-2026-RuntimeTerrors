using CommunityToolkit.Mvvm.ComponentModel;
using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class OurEventsViewModel : ObservableObject
    {
        EventsService service;
        public ObservableCollection<Event> Events { get; }

        public OurEventsViewModel(EventsService service)
        {
            this.service = service;
            Events = service.GetAllEvents();
        }

        //public void LoadElements()
        //{
        //    System.Diagnostics.Debug.WriteLine("elements getting loaded");
        //    var events = this.service.GetAllEvents();
        //    System.Diagnostics.Debug.WriteLine($"Events count: {events.Count}");
        //    Events.Clear();
        //    foreach (var e in events)
        //        Events.Add(e);
        //}
    }
}
