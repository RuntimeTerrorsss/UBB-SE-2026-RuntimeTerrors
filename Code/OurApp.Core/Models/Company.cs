using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string AboutUs { get; set; }
        public string ProfilePicturePath { get; set; }
        public string CompanyLogoPath { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string BuddyName { get; set; }
        public int? AvatarId { get; set; }
        public string FinalQuote { get; set; }
        public string Scenario1Text { get; set; }
        public string Scenario1Answer1 { get; set; }
        public string Scenario1Answer2 { get; set; }
        public string Scenario1Answer3 { get; set; }
        public string Scenario1Reaction1 { get; set; }
        public string Scenario1Reaction2 { get; set; }
        public string Scenario1Reaction3 { get; set; }
        public string Scenario2Text { get; set; }
        public string Scenario2Answer1 { get; set; }
        public string Scenario2Answer2 { get; set; }
        public string Scenario2Answer3 { get; set; }
        public string Scenario2Reaction1 { get; set; }
        public string Scenario2Reaction2 { get; set; }
        public string Scenario2Reaction3 { get; set; }
        public int PostedJobsCount { get; set; }
        public int CollaboratorsCount { get; set; }

        public List<string> Collaborators { get; set; } = new();

        public Company(
            string name,
            string aboutus,
            string pfpUrl,
            string logoUrl,
            string location,
            string email,
            int companyId = 1,
            string buddyName = "",
            int? avatarId = null,
            string finalQuote = "",
            string scenario1Text = "",
            string scenario1Answer1 = "",
            string scenario1Answer2 = "",
            string scenario1Answer3 = "",
            string scenario1Reaction1 = "",
            string scenario1Reaction2 = "",
            string scenario1Reaction3 = "",
            string scenario2Text = "",
            string scenario2Answer1 = "",
            string scenario2Answer2 = "",
            string scenario2Answer3 = "",
            string scenario2Reaction1 = "",
            string scenario2Reaction2 = "",
            string scenario2Reaction3 = "",
            int postedJobsCount = 0,
            int collaboratorsCount = 0)
        {
            CompanyId = companyId;
            Name = name ?? "";
            AboutUs = aboutus ?? "";
            ProfilePicturePath = pfpUrl ?? "";
            CompanyLogoPath = logoUrl ?? "";
            Location = location ?? "";
            Email = email ?? "";
            BuddyName = buddyName ?? "";
            AvatarId = avatarId;
            FinalQuote = finalQuote ?? "";
            Scenario1Text = scenario1Text ?? "";
            Scenario1Answer1 = scenario1Answer1 ?? "";
            Scenario1Answer2 = scenario1Answer2 ?? "";
            Scenario1Answer3 = scenario1Answer3 ?? "";
            Scenario1Reaction1 = scenario1Reaction1 ?? "";
            Scenario1Reaction2 = scenario1Reaction2 ?? "";
            Scenario1Reaction3 = scenario1Reaction3 ?? "";
            Scenario2Text = scenario2Text ?? "";
            Scenario2Answer1 = scenario2Answer1 ?? "";
            Scenario2Answer2 = scenario2Answer2 ?? "";
            Scenario2Answer3 = scenario2Answer3 ?? "";
            Scenario2Reaction1 = scenario2Reaction1 ?? "";
            Scenario2Reaction2 = scenario2Reaction2 ?? "";
            Scenario2Reaction3 = scenario2Reaction3 ?? "";
            PostedJobsCount = postedJobsCount;
            CollaboratorsCount = collaboratorsCount;
        }

        public override string ToString()
        {
            return $"Company[{CompanyId}]: {Name}, {Email}";
        }
    }
}
