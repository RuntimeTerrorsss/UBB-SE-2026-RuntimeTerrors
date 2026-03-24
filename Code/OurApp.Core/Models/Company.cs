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
        public string Pfp_url { get; set; }
        public string Logo_url { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public Company( string name, string aboutus, string pfp_url, string logo_url, string location, string email) {
            this.Name = name;
            this.AboutUs = aboutus;
            this.Pfp_url = pfp_url;
            this.Logo_url = logo_url;
            this.Location = location;
            this.Email = email;
        }

        public override string ToString()
        {
            return "Company: " + Name + "\n" + AboutUs + "\n" + Pfp_url + Logo_url + Location + Email;
        }
    }
}
