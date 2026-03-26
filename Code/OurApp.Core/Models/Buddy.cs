using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Models
{
    public class Buddy
    {   
        public int Id { get; set; }

        public string Name { get; set; }

        bool Mood  { get; set; }
        public Buddy(int id, string name) {
            Id = id;
            Name = name;
            Mood = true;
        }

        public void UpdateMood(bool newMood)
        {
            Mood = newMood;
        }

        public bool GetCurrentState()
        {
            return Mood;
        }
    }
}
