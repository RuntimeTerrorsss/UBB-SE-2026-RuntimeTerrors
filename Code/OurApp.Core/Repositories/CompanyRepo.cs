using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public class CompanyRepo : ICompanyRepo
    {
        ObservableCollection<Company> companies;

        public CompanyRepo()
        {
            companies = new ObservableCollection<Company>();
        }

        public void PrintAll()
        {
            for (int i = 0; i < companies.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{companies[i]} ");
            }
        }
        ObservableCollection<Company> ICompanyRepo.GetAll()
        {
            return companies;
        }
        void ICompanyRepo.Add(Company c)
        {
            companies.Add(c);
        }
        void ICompanyRepo.Remove(Company c)
        {
            companies.Remove(c);
        }


        /// <summary>
        /// Function that searches a company by name and returns it
        /// </summary>
        /// <param name="companyName"> the name of the company being searched </param>
        /// <returns> the company if found, else null </returns>
        public Company? GetCompanyByName(string companyName)
        {
            Company company = null;

            if (string.IsNullOrWhiteSpace(companyName))
            {
                return null;
            }


            foreach (Company compan in companies)
            {
                if (compan.Name == companyName)
                {
                    company = compan;
                }
            }

            return company;
        }
    }
}
