using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    internal interface ISkillServices
    {
        Task<List<Skill>> GetAllSkillsAsync();
    }
}
