using System;
using System.Collections.Generic;

namespace OurApp.Core.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AboutUs { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string LogoPictureUrl { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string BuddyName { get; set; }
        public int? AvatarId { get; set; }
        public string FinalQuote { get; set; }
        
        public string Scen1Text { get; set; }
        public string Scen1Answer1 { get; set; }
        public string Scen1Answer2 { get; set; }
        public string Scen1Answer3 { get; set; }
        public string Scen1Reaction1 { get; set; }
        public string Scen1Reaction2 { get; set; }
        public string Scen1Reaction3 { get; set; }
        
        public string Scen2Text { get; set; }
        public string Scen2Answer1 { get; set; }
        public string Scen2Answer2 { get; set; }
        public string Scen2Answer3 { get; set; }
        public string Scen2Reaction1 { get; set; }
        public string Scen2Reaction2 { get; set; }
        public string Scen2Reaction3 { get; set; }

        public Company() { }
    }
}
