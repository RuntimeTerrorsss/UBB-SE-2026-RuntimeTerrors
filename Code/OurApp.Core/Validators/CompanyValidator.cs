using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Validators
{
    internal class CompanyValidator
    {
        public bool NameValidator(string name)
        {
            if (name.Length == 0)
            {
                throw new Exception("Name is mandatory");
            }
            if (name.Length > 200)
            {
                throw new Exception("Name is too long");
            }
            return true;
        }

        public bool AboutUsValidator(string aboutus)
        {
            if (aboutus.Length > 2000)
            {
                throw new Exception("AboutUs is too long");
            }
            return true;
        }

        public bool LocationValidator(string location)
        {
            if (location.Length == 0)
            {
                throw new Exception("Location is mandatory");
            }
            if (location.Length > 300)
            {
                throw new Exception("Location is too long");
            }
            return true;
        }

        public bool EmailValidator(string email)
        {
            if (email.Length == 0)
            {
                throw new Exception("Email is mandatory");
            }
            if (email.Length > 200)
            {
                throw new Exception("Email is too long");
            }
            return true;
        }

        public bool PfpValidator(string pfp)
        {
            if (pfp.Length > 200)
            {
                throw new Exception("Profile picture path is too long!");
            }
            return true;
        }

        public bool LogoValidator(string logo)
        {
            if (logo.Length == 0)
            {
                throw new Exception("Logo is mandatory");
            }
            if (logo.Length > 200)
            {
                throw new Exception("Logo path is too long");
            }
            return true;
        }
    }
}
