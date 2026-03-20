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
        private bool validTitle = false;

        [ObservableProperty] private string description;
        [ObservableProperty] private string descriptionError;
        private bool validDescription = true;

        [ObservableProperty] private DateTimeOffset? startDate = DateTimeOffset.Now;
        [ObservableProperty] private string startDateError;
        private bool validStartDate = true;

        [ObservableProperty] private DateTimeOffset? endDate = DateTimeOffset.Now;
        [ObservableProperty] private string endDateError;
        private bool validEndDate = true;

        [ObservableProperty] private string location;
        [ObservableProperty] private string locationError;
        private bool validLocation = false;

        [ObservableProperty] private string addError;

        [RelayCommand]
        void Tap()
        {
            if (!validTitle || !validDescription || !validStartDate || !validEndDate || !validLocation)
            {
                AddError = "Please enter valid inputs before creating an event";
                return;
            }

            AddError = "";
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
                    validTitle = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                TitleError = ex.Message;
                validTitle = false;
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
                    validDescription = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                DescriptionError = ex.Message;
                validDescription = false;
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
                    validLocation = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LocationError = ex.Message;
                validLocation = false;
            }
            return false;
        }

        public bool ValidateStartDate()
        {
            try
            {
                if (validator.StartDateValidator(StartDate))
                {
                    StartDateError = "";
                    validStartDate = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                StartDateError = ex.Message;
                validStartDate = false;
            }
            return false;
        }

        public bool ValidateEndDate()
        {
            try
            {
                if (validator.EndDateValidator(EndDate))
                {
                    EndDateError = "";
                    validEndDate = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                EndDateError = ex.Message;
                validEndDate = false;
            }
            return false;
        }

        public bool ValidateDatesCronologity()
        {
            try
            {
                if (validator.DateCronologityValidator(StartDate, EndDate))
                {
                    EndDateError = "";
                    validEndDate = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                EndDateError = ex.Message;
                validEndDate = false;
            }
            return false;
        }
    }
}
