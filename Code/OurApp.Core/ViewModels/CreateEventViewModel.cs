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
using System.Net;
using System.Net.Mail;

namespace OurApp.Core.ViewModels
{
    public partial class CreateEventViewModel : ObservableObject
    {
        private readonly EventsService eventsService;
        private readonly ICompanyService companyService;
        private readonly SessionService sessionService;

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


        /// <summary>
        /// Create Event View Model constructor
        /// </summary>
        /// <param name="eventsService"> events service </param>
        /// <param name="companyService"> company service </param>
        /// <param name="sessionService"> session service </param>
        public CreateEventViewModel(EventsService eventsService, ICompanyService companyService, SessionService sessionService)
        {
            this.eventsService = eventsService;
            this.companyService = companyService;
            this.sessionService = sessionService;
        }


        /// <summary>
        /// Function that sends an email to a company
        /// </summary>
        /// <param name="destinationCompany"> company to send email to </param>
        private void SendMailToCompany(Company destinationCompany)
        {
            System.Diagnostics.Debug.WriteLine($"Sending email to {destinationCompany.Name}");
            System.Diagnostics.Debug.WriteLine($"{destinationCompany.Name} receives invitation\n");
        }

        /// <summary>
        /// Function that sends the invitations to all the selected companies, 
        /// after the user creates the event
        /// </summary>
        private void SendInvitations()
        {
            foreach (Company invitedCompany in this.SelectedCollaborators)
            {
                this.SendMailToCompany(invitedCompany);
            }
        }


        /// <summary>
        /// Function that tries to create a new event
        /// </summary>
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

                int hostId = sessionService.loggedInUser.Id;
                eventsService.AddEvent(Photo, Title, Description, eventStartDateTime, eventEndDateTime, Location, hostId, SelectedCollaborators.ToList());
                eventCreatedSuccessfully = true;

                SendInvitations();
            }
            catch (Exception exception)
            {
                eventCreatedSuccessfully = false;
            }
        }


        /// <summary>
        /// Function that sets some flags, used in the View, if the event title is valid
        /// </summary>
        /// <returns> true if the title is valid, false otherwise </returns>
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


        /// <summary>
        /// Function that sets some flags, used in the View, if the event description is valid
        /// </summary>
        /// <returns> true if the description is valid, false otherwise </returns>
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

        /// <summary>
        /// Function that sets some flags, used in the View, if the event location is valid
        /// </summary>
        /// <returns> true if the location is valid, false otherwise </returns>
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

        /// <summary>
        /// Function that sets some flags, used in the View, if the event starting date is valid
        /// </summary>
        /// <returns> true if the starting date is valid, false otherwise </returns>
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


        /// <summary>
        /// Function that sets some flags, used in the View, if the event ending date is valid
        /// </summary>
        /// <returns> true if the ending date is valid, false otherwise </returns>
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

        /// <summary>
        /// Function that sets some flags, used in the View, if the event dates are cronologically valid
        /// </summary>
        /// <returns> true if the dates are valid, false otherwise </returns>
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

        /// <summary>
        /// Function that tries to add a collaborator to the event
        /// </summary>
        /// <param name="companyName"> the invited company's name </param>
        /// <param name="errorMessage"> the error message returned </param>
        /// <returns> true if the company name exists, false otherwise </returns>
        public bool TryAddCollaboratorByName(string companyName, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(companyName))
            {
                errorMessage = "Please enter a company name.";
                return false;
            }

            if (!companyService.GetCompanyByName(companyName, out var company))
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

        /// <summary>
        /// Function that removes a collaborator
        /// </summary>
        /// <param name="companyName"> the name of the company to be removed from the collaborators list </param>
        public void RemoveCollaboratorByName(string companyName)
        {
            foreach (Company selectedCompany in SelectedCollaborators)
            {
                if (selectedCompany.Name == companyName)
                {
                    SelectedCollaborators.Remove(selectedCompany);
                }
            }
            //var collaborator = SelectedCollaborators
            //    .FirstOrDefault(c => string.Equals(c.Name, companyName, StringComparison.OrdinalIgnoreCase));

            //if (collaborator != null)
            //{
            //    SelectedCollaborators.Remove(collaborator);
            //}
        }
    }
}
