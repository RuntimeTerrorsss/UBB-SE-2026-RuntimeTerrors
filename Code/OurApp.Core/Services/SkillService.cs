using iss_project.Code.OurApp.Core.Models;
using iss_project.Code.OurApp.Core.Repositories;
using iss_project.Code.OurApp.Core.Repositories.iss_project.Code.OurApp.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iss_project.Code.OurApp.Core.Services
{
    public class SkillService
    {
        private readonly SkillRepository _skillRepository;

        public SkillService()
        {
            _skillRepository = new SkillRepository();
        }

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            return await _skillRepository.GetAllAsync();
        }
    }

}