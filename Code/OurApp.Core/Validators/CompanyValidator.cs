using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Validators
{
    public class CompanyValidator
    {
        public bool NameValidator(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
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
            if (string.IsNullOrEmpty(aboutus))
            {
                return true;
            }
            if (aboutus.Length > 2000)
            {
                throw new Exception("AboutUs is too long");
            }
            return true;
        }

        public bool LocationValidator(string location)
        {
            // Optional field per requirements.
            if (string.IsNullOrEmpty(location))
            {
                return true;
            }
            if (location.Length > 300)
            {
                throw new Exception("Location is too long");
            }
            return true;
        }

        public bool EmailValidator(string email)
        {
            // Optional field per requirements.
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            if (!email.Contains("@"))
            {
                throw new Exception("Email must contain '@'");
            }
            if (email.Length > 100)
            {
                throw new Exception("Email is too long");
            }
            return true;
        }

        public bool PfpValidator(string pfp)
        {
            if (string.IsNullOrEmpty(pfp))
            {
                return true;
            }
            if (pfp.Length > 255)
            {
                throw new Exception("Profile picture path is too long!");
            }
            if (!HasAllowedImageExtension(pfp))
            {
                throw new Exception("Profile picture must be .jpg, .jpeg or .png");
            }
            return true;
        }

        public bool LogoValidator(string logo)
        {
            if (string.IsNullOrWhiteSpace(logo))
            {
                throw new Exception("Logo is mandatory");
            }
            if (logo.Length > 255)
            {
                throw new Exception("Logo path is too long");
            }
            if (!HasAllowedImageExtension(logo))
            {
                throw new Exception("Logo must be .jpg, .jpeg or .png");
            }
            return true;
        }

        public bool MiniGameStruggleValidator(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            if (value.Length > 500)
            {
                throw new Exception("Buddy struggle is too long");
            }
            return true;
        }

        public bool MiniGameResponseValidator(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            if (value.Length > 200)
            {
                throw new Exception("Mini-game response is too long");
            }
            return true;
        }

        public bool MiniGameFeedbackValidator(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            if (value.Length > 200)
            {
                throw new Exception("Mini-game feedback is too long");
            }
            return true;
        }

        private static bool HasAllowedImageExtension(string value)
        {
            return value.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                || value.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                || value.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
        }
    }
}
