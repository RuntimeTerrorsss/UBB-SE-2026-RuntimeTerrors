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
        SessionService sessionService;
        public ObservableCollection<Event> currentEventsCollection { get; }

        /// <summary>
        /// Our Events View Model constructor
        /// </summary>
        /// <param name="service"> events service </param>
        public OurEventsViewModel(EventsService service, SessionService sessionService)
        {
            this.eventsService = service;
            this.sessionService = sessionService;
            currentEventsCollection = service.GetCurrentEvents(sessionService.loggedInUser.Id);
        }
    }
}
