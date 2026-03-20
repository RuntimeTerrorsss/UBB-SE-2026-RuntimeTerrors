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

        [ObservableProperty] private string photo;

        [ObservableProperty] private string title;
        [ObservableProperty] private string titleError;

        [ObservableProperty] private string description;
        [ObservableProperty] private string descriptionError;

        [ObservableProperty]
        private DateTimeOffset? startDate = DateTimeOffset.Now;

        [ObservableProperty]
        private DateTimeOffset? endDate = DateTimeOffset.Now;

        [ObservableProperty] private string location;
        [ObservableProperty] private string locationError;


        [RelayCommand]
        void Tap()
        {
            DateTime start = startDate.Value.DateTime;
            DateTime end = endDate.Value.DateTime;
            service.AddEvent(Photo, Title, Description, start, end, Location);
            service.printAll();
        }

        public bool ValidateTitle()
        {
            try
            {
                if (validator.TitleValidator(Title))
                {
                    TitleError = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                TitleError = ex.Message;
            }
            return false;
        }

        public bool ValidateDescription()
        {
            try
            {
                if (validator.DescriptionValidator(Description))
                {
                    DescriptionError = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                DescriptionError = ex.Message;
            }
            return false;
        }

        public bool ValidateLocation()
        {
            try
            {
                if (validator.LocationValidator(Location))
                {
                    LocationError = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                LocationError = ex.Message;
            }
            return false;
        }
    }
}
