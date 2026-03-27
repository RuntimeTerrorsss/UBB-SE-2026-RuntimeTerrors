using OurApp.Core.Models;

namespace OurApp.Core.Services
{
    public interface ICollaboratorsService
    {
        void AddCollaborator(Event eventToBeCollaboratedOn, Company companyInvitedToCollaborate);
        List<Company> GetAllCollaborators(int loggedInCompanyId);
    }
}