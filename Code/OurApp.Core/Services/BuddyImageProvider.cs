using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public static class BuddyImageProvider
    {
        private static readonly Dictionary<int, string> BuddyImages = new Dictionary<int, string>
        {
            { 1, "ms-appx:///Assets/AvatarMale.png" },
            { 2, "ms-appx:///Assets/AvatarFemale.png" }
        };

        public static string GetImagePathById(int id)
        {
            if (BuddyImages.TryGetValue(id, out var path))
            {
                return path;
            }
            return BuddyImages[1];
        }
    }
}
