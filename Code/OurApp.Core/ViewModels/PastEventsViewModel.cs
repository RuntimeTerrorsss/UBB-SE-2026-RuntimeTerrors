using CommunityToolkit.Mvvm.ComponentModel;
using OurApp.Core.Models;
using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class PastEventsViewModel : ObservableObject
    {
        EventsService service;
        public ObservableCollection<Event> Events { get; }

        public PastEventsViewModel(EventsService service)
        {
            this.service = service;
            Events = service.GetPastEvents();
        }
    }
}
