using System;

namespace OurApp.Core.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public Company HostCompany { get; set; }

        public string Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public DateTime? PostedAt { get; set; }
    }
}
