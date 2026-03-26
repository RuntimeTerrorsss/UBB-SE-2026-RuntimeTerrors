using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Validators;
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
        private readonly ICompanyRepo CompanyRepo;
        private readonly CompanyValidator _validator;

        public CompanyService(ICompanyRepo repo) 
        {
            CompanyRepo = repo;
            _validator = new CompanyValidator();
        }

        private void ValidateCompany(Company company)
        {
            _validator.NameValidator(company.Name);
            _validator.AboutUsValidator(company.AboutUs);
            _validator.PfpValidator(company.ProfilePicturePath);
            _validator.LogoValidator(company.CompanyLogoPath);
            _validator.LocationValidator(company.Location);
            _validator.EmailValidator(company.Email);

            _validator.MiniGameStruggleValidator(company.Scenario1Text);
            _validator.MiniGameStruggleValidator(company.Scenario2Text);
            _validator.MiniGameResponseValidator(company.Scenario1Answer1);
            _validator.MiniGameResponseValidator(company.Scenario1Answer2);
            _validator.MiniGameResponseValidator(company.Scenario1Answer3);
            _validator.MiniGameResponseValidator(company.Scenario2Answer1);
            _validator.MiniGameResponseValidator(company.Scenario2Answer2);
            _validator.MiniGameResponseValidator(company.Scenario2Answer3);
            _validator.MiniGameFeedbackValidator(company.Scenario1Reaction1);
            _validator.MiniGameFeedbackValidator(company.Scenario1Reaction2);
            _validator.MiniGameFeedbackValidator(company.Scenario1Reaction3);
            _validator.MiniGameFeedbackValidator(company.Scenario2Reaction1);
            _validator.MiniGameFeedbackValidator(company.Scenario2Reaction2);
            _validator.MiniGameFeedbackValidator(company.Scenario2Reaction3);
        }

        public void AddCompany(string companyName, string aboutUs, string pfpUrl, string logoUrl, string location, string email)
        {
            Company companyToBeAdded = new Company(companyName, aboutUs, pfpUrl, logoUrl, location, email);
            ValidateCompany(companyToBeAdded);
            CompanyRepo.Add(companyToBeAdded);
        }

        public Company? GetCompanyById(int companyId)
        {
            return CompanyRepo.GetById(companyId);
        }

        public void UpdateCompany(Company company)
        {
            ValidateCompany(company);
            CompanyRepo.Update(company);
        }

        public void RemoveCompany(int companyId)
        {
            CompanyRepo.Remove(companyId);
        }

        public void PrintAll()
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
