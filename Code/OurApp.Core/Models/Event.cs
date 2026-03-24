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
        private static int eventIdCounter = 8;
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int HostID { get; set; }
        public List<Company> Collaborators { get; set; }
        //public int CollaboratorID { get; set; }

        public Event(string eventPhoto, string eventTitle, string eventDescription, DateTime eventStartDate, DateTime eventEndDate, string eventLocation, int eventHostID, List<Company> eventCollaborators)
        {
            this.Id = eventIdCounter++;
            this.Photo = eventPhoto;
            this.Title = eventTitle;
            this.Description = eventDescription;
            this.StartDate = eventStartDate;
            this.EndDate = eventEndDate;
            this.Location = eventLocation;
            this.HostID = eventHostID;
            this.Collaborators = eventCollaborators;
        }

        public override string ToString()
        {
            return "Event: " + Photo + " " + Title + " " + Description + " " +
                StartDate.ToString() + " " + EndDate.ToString() + " " + Location + " " + HostID.ToString() +
                " " + Collaborators.ToString() + "\n";
        }
    }
}
