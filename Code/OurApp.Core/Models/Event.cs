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
        public string Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int HostID { get; set; }
        public int CollaboratorID { get; set; }

        public Event(string photo, string title, string description, DateTime startDate, DateTime endDate, string location, int hostID, int collaboratorID)
        {
            this.Photo = photo;
            this.Title = title;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Location = location;
            this.HostID = hostID;
            this.CollaboratorID = collaboratorID;
        }

        public override string ToString()
        {
            return "Event: " + Photo + " " + Title + " " + Description + " " +
                StartDate.ToString() + " " + EndDate.ToString() + " " + Location + " " + HostID.ToString() +
                " " + CollaboratorID.ToString() + "\n";
        }
    }
}
