using OurApp.Core.Models;
using System;
using System.Collections.Generic;

namespace OurApp.Core.Services
{
    public class ProfileCompletionCalculator
    {
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

            if (IsMiniGameComplete(company)) done++;
            else tasks.Add("Complete mini-game");

            return ((done * 100) / total, tasks);
        }

        private static bool IsMiniGameComplete(Company c)
        {
            return !string.IsNullOrWhiteSpace(c.Scenario1Text)
                && !string.IsNullOrWhiteSpace(c.Scenario1Answer1)
                && !string.IsNullOrWhiteSpace(c.Scenario1Answer2)
                && !string.IsNullOrWhiteSpace(c.Scenario1Answer3)
                && !string.IsNullOrWhiteSpace(c.Scenario2Text)
                && !string.IsNullOrWhiteSpace(c.Scenario2Answer1)
                && !string.IsNullOrWhiteSpace(c.Scenario2Answer2)
                && !string.IsNullOrWhiteSpace(c.Scenario2Answer3)
                && !string.IsNullOrWhiteSpace(c.Scenario1Reaction1)
                && !string.IsNullOrWhiteSpace(c.Scenario1Reaction2)
                && !string.IsNullOrWhiteSpace(c.Scenario1Reaction3)
                && !string.IsNullOrWhiteSpace(c.Scenario2Reaction1)
                && !string.IsNullOrWhiteSpace(c.Scenario2Reaction2)
                && !string.IsNullOrWhiteSpace(c.Scenario2Reaction3);
        }
    }
}
