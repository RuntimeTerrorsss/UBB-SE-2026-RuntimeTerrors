using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public class CompanyService : ICompanyService
    {
        ICompanyRepo CompanyRepo;

        public CompanyService(ICompanyRepo repo) 
        {
            CompanyRepo = repo;
        }

        public void addCompany(string companyName, string aboutUs, string pfpUrl, string logoUrl, string location, string email)
        {
            Company companyToBeAdded = new Company(companyName, aboutUs, pfpUrl, logoUrl, location, email);
            this.CompanyRepo.Add(companyToBeAdded);
        }

        public void printAll()
        {
            this.CompanyRepo.PrintAll();
        }

        public bool TryGetCompanyByName(string companyName, out Company company)
        {
            company = null;

            if (string.IsNullOrWhiteSpace(companyName))
            {
                return false;
            }

            company = CompanyRepo
                .GetAll()
                .FirstOrDefault(c => string.Equals(c.Name, companyName.Trim(), StringComparison.OrdinalIgnoreCase));

            return company != null;
        }
    }
}
