using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Validators
{
    public class EventValidator
    {
        public bool IsEventTitleValid(string eventTitle)
        {
            if (eventTitle.Length == 0)
            {
                throw new Exception("Title is mandatory");
            }
            if (eventTitle.Length > 200)
            {
                throw new Exception("Title is too long");
            }
            return true;
        }

        public bool IsEventDescriptionValid(string eventDescription)
        {
            if (eventDescription.Length > 2000)
            {
                throw new Exception("Description is too long");
            }
            return true;
        }

        public bool IsEventLocationValid(string eventLocation)
        {
            if (eventLocation.Length == 0)
            {
                throw new Exception("Location is mandatory");
            }
            if (eventLocation.Length > 300)
            {
                throw new Exception("Location is too long");
            }
            return true;
        }

        public bool IsEventStartDateValid(DateTimeOffset? eventStartDate)
        {
            if (eventStartDate == null)
            {
                throw new Exception("Starting date is mandatory");
            }
            if (eventStartDate < DateTimeOffset.Now)
            {
                throw new Exception("Event must start after creation");
            }
            return true;
        }

        public bool IsEventEndDateValid(DateTimeOffset? eventEndDate)
        {
            if (eventEndDate == null)
            {
                throw new Exception("Ending date is mandatory");
            }
            if (eventEndDate < DateTimeOffset.Now)
            {
                throw new Exception("Event must end after creation");
            }
            return true;
        }

        public bool AreEventDatesCronologicallyValid(DateTimeOffset? eventStartDate, DateTimeOffset? eventEndDate)
        {
            if (eventStartDate > eventEndDate)
            {
                throw new Exception("Event must begin before ending");
            }
            return true;
        }
    }
}
