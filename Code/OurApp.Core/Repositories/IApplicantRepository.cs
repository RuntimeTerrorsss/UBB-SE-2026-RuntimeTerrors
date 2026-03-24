using System.Collections.Generic;
using OurApp.Core.Models;

namespace OurApp.Core.Repositories
{
    public interface IApplicantRepository
    {
        Applicant GetApplicantById(int applicantId);
        IEnumerable<Applicant> GetApplicantsByJob(JobPosting job);
        void AddApplicant(Applicant applicant);
        void UpdateApplicant(Applicant applicant);
    }
}
