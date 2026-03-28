using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    internal interface ISkillRepository
    {
        Task<List<Skill>> GetAllAsync();
    }
}
