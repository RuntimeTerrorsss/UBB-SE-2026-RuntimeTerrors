using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Scenario
    {
        public string Text { get; set; }
        public List<AdviceChoice> AdviceChoices { get; set; }

        public Scenario(string text)
        {
            this.Text = text;
            AdviceChoices = new List<AdviceChoice>();
        }

        public void AddChoice(AdviceChoice choice)
        {
            this.AdviceChoices.Add(choice);
        }

        public List<string> ShowAdvices()
        {
            List<string> choices = new List<string>();

            for(int i = 0; i < this.AdviceChoices.Count; i++)
            {
                choices.Add(AdviceChoices[i].Advice);
            }

            return choices;
        }
        public string ChooseAdvice(int num)
        {
            return AdviceChoices[num].IsChosen();
        }

    }
}
