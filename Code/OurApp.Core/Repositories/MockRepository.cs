using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
     public class MockRepository
    {   
        public List<Scenario> Scenarios { get; set; }
        public Buddy Buddy {  get; set; }
        public MockRepository() {
            this.Scenarios = new List<Scenario>();
            this.Buddy = new Buddy(1, "Alex");
        }
        public string GetConclusion()
        {
            return "In our company we value teamwork. We are glad that you showed empathy and support!";
        }

        public Scenario GetScenario(int number)
        {
            return Scenarios[number];
        }

        public void MakeMockScenarios()
        {

            var scenario1 = new Scenario("I've been debugging for 5 hours and I'm exhausted, what should I do?");
            scenario1.AddChoice(new AdviceChoice("Take a 15-minute walk", "Great advice! Here at OurApp, we believe breaks increase productivity."));
            scenario1.AddChoice(new AdviceChoice("Ask a senior for help", "Smart move! Collaboration is key to our success."));
            scenario1.AddChoice(new AdviceChoice("Drink another coffee", "Well... it works for now, but don't forget to hydrate!"));

            var scenario2 = new Scenario("I'm feeling overwhelmed by this new sprint planning. Any tips?");
            scenario2.AddChoice(new AdviceChoice("Break tasks into smaller pieces", "Perfect! Small steps lead to big achievements."));
            scenario2.AddChoice(new AdviceChoice("Talk to the Scrum Master", "Exactly! Transparency helps the whole team."));
            scenario2.AddChoice(new AdviceChoice("Work overtime tonight", "We prefer work-life balance. Maybe try prioritizing instead?"));

            this.Scenarios.Add(scenario1);
            this.Scenarios.Add(scenario2);
        }
    }
}
