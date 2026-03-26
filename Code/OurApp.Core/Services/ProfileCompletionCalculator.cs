using OurApp.Core.Models;
using System;
using System.Collections.Generic;

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

        public Game game;

        public ProfileCompletionCalculator(Company company, Game game) {
            this.game = game;
        }

        public (int percentage, List<string> remainingTasks) Calculate(Company company)
        {
            int total = 5;
            int done = 0;
            var tasks = new List<string>();

            if (!string.IsNullOrEmpty(company.ProfilePicturePath)) done++;
            else tasks.Add("Upload company picture");

            if (!string.IsNullOrEmpty(company.AboutUs)) done++;
            else tasks.Add("Add company description");

            if (company.PostedJobsCount >= 5) done++;
            else tasks.Add("Post at least 5 jobs");

            if (company.CollaboratorsCount >= 2 || company.Collaborators.Count >= 2) done++;
            else tasks.Add("Add 2 collaborators");

            if (IsMiniGameComplete(this.game)) done++;
            else tasks.Add("Complete mini-game");

            return ((done * 100) / total, tasks);
        }

        private static bool IsMiniGameComplete(Game g)
        {
            return g.IsPublished;
        }
    }
}
