using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Company
    {
        public string Name { get; set; }
        public string AboutUs { get; set; }
        public string ProfilePicturePath { get; set; }
        public string CompanyLogoPath { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }

        //public List<Job> Jobs { get; set; } = new();
        //public List<string> Collaborators { get; set; } = new();
        //public MiniGame MiniGame { get; set; }

        public Company( string name, string aboutus, string pfp_url, string logo_url, string location, string email) {
            this.Name = name;
            this.AboutUs = aboutus;
            this.ProfilePicturePath = pfp_url;
            this.CompanyLogoPath = logo_url;
            this.Location = location;
            this.Email = email;
        }

        public override string ToString()
        {
            return "Company: " + Name + "\n" + AboutUs + "\n" + ProfilePicturePath + CompanyLogoPath + Location + Email;
        }
    }
}
