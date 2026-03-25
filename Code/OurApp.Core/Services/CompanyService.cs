using OurApp.Core.Models;
using OurApp.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Company companyToBeAdded = new Company(1, companyName, aboutUs, pfpUrl, logoUrl, location, email);
            this.CompanyRepo.Add(companyToBeAdded);
        }

        public void printAll()
        {
            this.CompanyRepo.PrintAll();
        }

        /// <summary>
        /// Function that searches a company by name and returns it
        /// </summary>
        /// <param name="companyName"> the name of the company being searched </param>
        /// <returns> the company if found, else null </returns>
        public Company? GetCompanyByName(string companyName)
        {
            return this.CompanyRepo.GetCompanyByName(companyName);
        }
    }
}
