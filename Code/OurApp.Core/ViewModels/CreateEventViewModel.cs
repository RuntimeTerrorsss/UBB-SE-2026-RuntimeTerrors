using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class CreateEventViewModel : ObservableObject
    {
        EventsService service = new EventsService(new EventsRepo());
        public EventValidator validator = new EventValidator();

        [ObservableProperty]
        private string photo;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private DateTimeOffset? startDate = DateTimeOffset.Now;

        [ObservableProperty]
        private DateTimeOffset? endDate = DateTimeOffset.Now;

        [ObservableProperty]
        private string location;



        [RelayCommand]
        void Tap()
        {
            //System.Diagnostics.Debug.WriteLine(Full);
            DateTime start = startDate.Value.DateTime;
            DateTime end = endDate.Value.DateTime;
            service.AddEvent(Photo, Title, Description, start, end, Location);
            service.printAll();
        }
    }
}
