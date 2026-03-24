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
        private readonly EventsService eventsService;
        public ObservableCollection<Event> pastEventsCollection { get; }


        /// <summary>
        /// Past Events View Model constructor
        /// </summary>
        /// <param name="service"> events service </param>
        public PastEventsViewModel(EventsService service)
        {
            this.eventsService = service;
            pastEventsCollection = service.GetPastEvents();
        }
    }
}
