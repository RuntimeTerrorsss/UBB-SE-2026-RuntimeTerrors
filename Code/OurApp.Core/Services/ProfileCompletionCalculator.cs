using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public class ProfileCompletionCalculator
    {
        //Company has:
        //    this.Name = name;
        //    this.AboutUs = aboutus;
        //    this.Pfp_url = pfp_url;
        //    this.Logo_url = logo_url;
        //    this.Location = location;
        //    this.Email = email;

        public (int percentage, List<string> remainingTasks) Calculate(Company company)
        {
            int total = 5;
            int done = 0;
            var tasks = new List<string>();

            if (!string.IsNullOrEmpty(company.CompanyImagePath)) done++;
            else tasks.Add("Upload company picture");

            if (!string.IsNullOrEmpty(company.AboutUs)) done++;
            else tasks.Add("Add company description");

            if (company.Jobs.Count >= 5) done++;
            else tasks.Add("Post at least 5 jobs");

            if (company.Collaborators.Count >= 2) done++;
            else tasks.Add("Add 2 collaborators");

            if (IsMiniGameComplete(company)) done++;
            else tasks.Add("Complete mini-game");

            return ((done * 100) / total, tasks);
        }

        private bool IsMiniGameComplete(Company c)
        {
            var g = c.MiniGame;
            if (g == null) return false;

            return !string.IsNullOrEmpty(g.Struggle1) &&
                   !string.IsNullOrEmpty(g.Struggle2) &&
                   g.Responses1.Count == 3 &&
                   g.Responses2.Count == 3 &&
                   g.Feedbacks.Count == 3;
        }
    }
}
