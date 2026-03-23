using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Scenario
    {
        Buddy buddy;
        string text { get; set; }
        List<AdviceChoice> AdviceChoices { get; set; }

        public Scenario(Buddy buddy, string text)
        {
            this.buddy = buddy;
            this.text = text;
        }

        public void AddChoice(AdviceChoice choice)
        {
            this.AdviceChoices.Add(choice);
        }

    }
}
