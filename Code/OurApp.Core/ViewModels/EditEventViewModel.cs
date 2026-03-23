using CommunityToolkit.Mvvm.ComponentModel;
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
        EventsService service;
        Event selectedEvent;
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

        [ObservableProperty] private string addError = "";

        public bool validationSuccess => (addError == "");
        public bool createSuccess = false;

        public EditEventViewModel(EventsService service, Event selectedEvent)
        {
            this.service = service;
            this.selectedEvent = selectedEvent;

            this.title = selectedEvent.Title;
            this.description = selectedEvent.Description;
            this.startDate = new DateTimeOffset?(selectedEvent.StartDate);
            this.endDate = new DateTimeOffset?(selectedEvent.EndDate);
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
