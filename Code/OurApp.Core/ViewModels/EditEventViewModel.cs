using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
using OurApp.Core.Services;
using OurApp.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class EditEventViewModel : ObservableObject
    {
        EventsService eventsService;
        Event eventToEdit;
        public EventValidator eventValidator = new EventValidator();

        [ObservableProperty] private string photo;

        [ObservableProperty] private string title;
        [ObservableProperty] private string titleError;
        private bool titleIsValid = true;

        [ObservableProperty] private string description;
        [ObservableProperty] private string descriptionError;
        private bool descriptionIsValid = true;

        [ObservableProperty] private DateTimeOffset? startDate;
        [ObservableProperty] private string startDateError;
        private bool startDateIsValid = true;

        [ObservableProperty] private DateTimeOffset? endDate;
        [ObservableProperty] private string endDateError;
        private bool endDateIsValid = true;

        [ObservableProperty] private string location;
        [ObservableProperty] private string locationError;
        private bool locationIsValid = true;

        [ObservableProperty] private string addError = "";

        public bool isEverythingValid => (addError == "");
        public bool eventUpdatedSuccessfully = false;

        public EditEventViewModel(EventsService service, Event selectedEvent)
        {
            this.eventsService = service;
            this.eventToEdit = selectedEvent;

            this.title = selectedEvent.Title;
            this.description = selectedEvent.Description;
            this.startDate = new DateTimeOffset?(selectedEvent.StartDate);
            this.endDate = new DateTimeOffset?(selectedEvent.EndDate);
            this.location = selectedEvent.Location;
        }

        [RelayCommand] public void EditEvent()
        {
            if (!titleIsValid || !descriptionIsValid || !startDateIsValid || !endDateIsValid || !locationIsValid)
            {
                AddError = "Please enter valid inputs before creating an event";
                return;
            }

            try
            {
                AddError = "";
                DateTime eventStartDateTime = startDate.Value.DateTime;
                DateTime eventEndDateTime = endDate.Value.DateTime;

                eventsService.UpdateEvent(eventToEdit.Id, Photo, Title, Description, eventStartDateTime, eventEndDateTime, Location);
                eventUpdatedSuccessfully = true;
            }
            catch (Exception)
            {
                eventUpdatedSuccessfully = false;
            }
        }


        public bool ValidateTitle()
        {
            try
            {
                if (eventValidator.IsEventTitleValid(Title))
                {
                    TitleError = "";
                    titleIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                TitleError = ex.Message;
                titleIsValid = false;
            }
            return false;
        }

        public bool ValidateDescription()
        {
            try
            {
                if (eventValidator.IsEventDescriptionValid(Description))
                {
                    DescriptionError = "";
                    descriptionIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                DescriptionError = ex.Message;
                descriptionIsValid = false;
            }
            return false;
        }

        public bool ValidateStartDate()
        {
            try
            {
                if (eventValidator.IsEventStartDateValid(StartDate))
                {
                    StartDateError = "";
                    startDateIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                StartDateError = ex.Message;
                startDateIsValid = false;
            }
            return false;
        }

        public bool ValidateEndDate()
        {
            try
            {
                if (eventValidator.IsEventEndDateValid(EndDate))
                {
                    EndDateError = "";
                    endDateIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                EndDateError = ex.Message;
                endDateIsValid = false;
            }
            return false;
        }

        public bool ValidateDatesCronologity()
        {
            try
            {
                if (eventValidator.AreEventDatesCronologicallyValid(StartDate, EndDate))
                {
                    StartDateError = "";
                    EndDateError = "";
                    endDateIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                StartDateError = ex.Message;
                EndDateError = ex.Message;
                endDateIsValid = false;
            }
            return false;
        }

        public bool ValidateLocation()
        {
            try
            {
                if (eventValidator.IsEventLocationValid(Location))
                {
                    LocationError = "";
                    locationIsValid = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LocationError = ex.Message;
                locationIsValid = false;
            }
            return false;
        }
    }
}
