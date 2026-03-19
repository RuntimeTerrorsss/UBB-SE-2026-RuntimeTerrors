using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OurApp.Core.Models
{
    public class Event
    {
        private string photo { get; set; }
        private string title { get; set; }
        private string description { get; set; }
        private string startDate { get; set; }
        private string endDate { get; set; }
        private string location { get; set; }
        private int hostID { get; set; }
        private int collaboratorID { get; set; }

        public Event(string photo, string title, string description, string startDate, string endDate, string location, int hostID, int collaboratorID)
        {
            this.photo = photo;
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
            this.hostID = hostID;
            this.collaboratorID = collaboratorID;
        }

        public override string ToString()
        {
            return "Event: " + photo + " " + title + " " + description + " " +
                startDate + " " + endDate + " " + location + " " + hostID.ToString() +
                " " + collaboratorID.ToString() + "\n";
        }
    }
}
