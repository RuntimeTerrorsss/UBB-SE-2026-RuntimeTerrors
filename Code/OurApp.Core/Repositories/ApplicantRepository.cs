using System;
using System.Collections.Generic;
using System.Linq;
using OurApp.Core.Models;

namespace OurApp.Core.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private static readonly List<Applicant> _applicants = new List<Applicant>
        {
            // Seed data
            new Applicant { ApplicantId = 1, Job = new JobPosting{ JobId = 1 }, User = new User(1, "Alice", "alice@test.com"), ApplicationStatus = "Pending", AppliedAt = DateTime.Now.AddDays(-2), CvFileUrl = "demo_cv.xml" },
            new Applicant { ApplicantId = 2, Job = new JobPosting{ JobId = 1 }, User = new User(2, "Bob", "bob@test.com"), ApplicationStatus = "Pending", AppliedAt = DateTime.Now.AddDays(-1), CvFileUrl = "demo_cv.xml" }
        };

        private int _nextId = 3;

        public Applicant GetApplicantById(int applicantId)
        {
            return _applicants.FirstOrDefault(a => a.ApplicantId == applicantId);
        }

        public IEnumerable<Applicant> GetApplicantsByJob(JobPosting job)
        {
            return _applicants.Where(a => a.Job?.JobId == job?.JobId).ToList();
        }

        public void AddApplicant(Applicant applicant)
        {
            applicant.ApplicantId = _nextId++;
            if (applicant.AppliedAt == default)
            {
                applicant.AppliedAt = DateTime.Now;
            }
            _applicants.Add(applicant);
        }

        public void UpdateApplicant(Applicant applicant)
        {
            var existing = GetApplicantById(applicant.ApplicantId);
            if (existing != null)
            {
                existing.AppTestGrade = applicant.AppTestGrade;
                existing.CvGrade = applicant.CvGrade;
                existing.CompanyTestGrade = applicant.CompanyTestGrade;
                existing.InterviewGrade = applicant.InterviewGrade;
                existing.ApplicationStatus = applicant.ApplicationStatus;
                existing.CvFileUrl = applicant.CvFileUrl;
            }
        }
    }
}
