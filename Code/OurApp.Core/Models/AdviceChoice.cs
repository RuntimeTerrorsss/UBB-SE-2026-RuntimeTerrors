using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class AdviceChoice
    {
        public string Advice { get; set; }
        public string Feedback {  get; set; }


        public AdviceChoice(string advice, string feedback)
        {
            Advice = advice;
            Feedback = feedback;
        }

        public string IsChosen()
        {
            return Feedback;
        }
    }
}
