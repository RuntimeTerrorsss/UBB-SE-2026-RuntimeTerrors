using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Validators
{
    public class EventValidator
    {
        public bool TitleValidator(string title)
        {
            if (title.Length == 0)
            {
                throw new Exception("Title is mandatory");
            }
            if (title.Length > 200)
            {
                throw new Exception("Title is too long");
            }
            return true;
        }

        public bool DescriptionValidator(string description)
        {
            if (description.Length > 2000)
            {
                throw new Exception("Description is too long");
            }
            return true;
        }

        public bool LocationValidator(string location)
        {
            if (location.Length == 0)
            {
                throw new Exception("Location is mandatory");
            }
            if (location.Length > 300)
            {
                throw new Exception("Location is too long");
            }
            return true;
        }

        public bool StartDateValidator(string startDate)
        {
            if (startDate.Length == 0)
            {
                throw new Exception("Starting date is mandatory");
            }
            return true;
        }

        public bool EndDateValidator(string endDate)
        {
            if (endDate.Length == 0)
            {
                throw new Exception("Ending date is mandatory");
            }
            return true;
        }

        public bool DateValidator(string start, string end)
        {
            int comparison = String.Compare(start, end, comparisonType: StringComparison.OrdinalIgnoreCase);

            if (comparison > 0) // start > end
            {
                throw new Exception("Event must begin before ending");
            }
            return true;
        }
    }
}
