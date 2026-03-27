using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iss_project.Code.OurApp.Core.Models
{
    public class SkillRequirement
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }

        public bool IsSelected { get; set; }  // checkbox
        public int Percentage { get; set; }   // user input
    }
}
