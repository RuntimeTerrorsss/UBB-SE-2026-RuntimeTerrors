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
        EventsService eventsService;
        public ObservableCollection<Event> currentEventsCollection { get; }

        public OurEventsViewModel(EventsService service)
        {
            this.eventsService = service;
            currentEventsCollection = service.GetCurrentEvents();
        }
    }
}
