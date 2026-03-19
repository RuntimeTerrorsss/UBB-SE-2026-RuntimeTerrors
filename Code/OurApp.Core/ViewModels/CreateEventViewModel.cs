using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
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

        [ObservableProperty]
        private string photo;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string startDate;

        [ObservableProperty]
        private string endDate;

        [ObservableProperty]
        private string location;



        [RelayCommand]
        void Tap()
        {
            //System.Diagnostics.Debug.WriteLine(Full);
            service.AddEvent(Photo, Title, Description, StartDate, EndDate, Location);
            service.printAll();
        }
    }
}
