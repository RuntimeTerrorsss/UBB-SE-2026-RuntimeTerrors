using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OurApp.Core.Models;

namespace OurApp.Core.Services
{
    public class CollaboratorsService
    {
        CollaboratorsRepo repo;
        public CollaboratorsService(CollaboratorsRepo repo) {
            this.repo = repo;
        }

        public void Add(Event e, Company c)
        {
            this.repo.AddCollaboratorToRepo(e, c);
        }

        public List<Company> GetAllCollaborators(int e)
        {
            return this.repo.GetAllCollaborators(e);
        }
    }
}
