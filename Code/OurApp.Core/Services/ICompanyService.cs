using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public interface ICompanyService
    {
        void addCompany(string companyName, string aboutUs, string pfpUrl, string logoUrl, string location, string email);
        void printAll();
    }
}
