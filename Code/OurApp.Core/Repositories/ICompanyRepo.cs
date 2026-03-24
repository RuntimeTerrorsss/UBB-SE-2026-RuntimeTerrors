using OurApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Repositories
{
    public interface ICompanyRepo
    {
        public void PrintAll();
        ObservableCollection<Company> GetAll();
        void Add(Company c);
        void Remove(Company c);

    }
}
