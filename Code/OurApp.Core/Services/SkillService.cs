using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurApp.Core.Services
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