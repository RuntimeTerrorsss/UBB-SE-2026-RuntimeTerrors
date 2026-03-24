using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OurApp.Core.Models;
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
        EventsService eventsService;
        ICompanyService companyService;
        public EventValidator eventValidator = new EventValidator();
        public List<Company> SelectedCollaborators { get; } = new List<Company>();

        [ObservableProperty] private string photo;

        [ObservableProperty] private string title;
        [ObservableProperty] private string titleError;
        private bool titleIsValid = false;

        [ObservableProperty] private string description;
        [ObservableProperty] private string descriptionError;
        private bool descriptionIsValid = true;

        [ObservableProperty] private DateTimeOffset? startDate = DateTimeOffset.Now;
        [ObservableProperty] private string startDateError;
        private bool startDateIsValid = true;

        [ObservableProperty] private DateTimeOffset? endDate = DateTimeOffset.Now;
        [ObservableProperty] private string endDateError;
        private bool endDateIsValid = true;

        [ObservableProperty] private string location;
        [ObservableProperty] private string locationError;
        private bool locationIsValid = false;

        [ObservableProperty] private string addError = "";

        public bool isEverythingValid => (addError == "");
        public bool eventCreatedSuccessfully = false;


        public CreateEventViewModel(EventsService service, ICompanyService companyService)
        {
            this.eventsService = service;
            this.companyService = companyService;
        }


        [RelayCommand]
        public void CreateEvent()
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

                eventsService.AddEvent(Photo, Title, Description, eventStartDateTime, eventEndDateTime, Location, SelectedCollaborators.ToList());
                eventCreatedSuccessfully = true;
            }
            catch (Exception exception)
            {
                eventCreatedSuccessfully = false;
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

        public bool TryAddCollaboratorByName(string companyName, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(companyName))
            {
                errorMessage = "Please enter a company name.";
                return false;
            }

            if (!companyService.TryGetCompanyByName(companyName, out var company))
            {
                errorMessage = "Company was not found.";
                return false;
            }

            if (SelectedCollaborators.Any(c => string.Equals(c.Name, company.Name, StringComparison.OrdinalIgnoreCase)))
            {
                errorMessage = "Company is already added as a collaborator.";
                return false;
            }

            SelectedCollaborators.Add(company);
            return true;
        }

        public void RemoveCollaboratorByName(string companyName)
        {
            var collaborator = SelectedCollaborators
                .FirstOrDefault(c => string.Equals(c.Name, companyName, StringComparison.OrdinalIgnoreCase));

            if (collaborator != null)
            {
                SelectedCollaborators.Remove(collaborator);
            }
        }
    }
}
