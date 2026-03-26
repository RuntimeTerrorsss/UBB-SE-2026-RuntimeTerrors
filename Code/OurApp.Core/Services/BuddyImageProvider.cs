using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.Services
{
    public static class BuddyImageProvider
    {
        // Dicționar care mapează ID-ul (int) către calea imaginii (string)
        private static readonly Dictionary<int, string> BuddyImages = new Dictionary<int, string>
        {
            { 0, "ms-appx:///Assets/AvatarMale.png" },
            { 1, "ms-appx:///Assets/AvatarFemale.png" }
            // Poți adăuga oricâți "buddies" aici
        };

        public static string GetImagePathById(int id)
        {
            // Dacă ID-ul există în dicționar, îl returnăm. 
            // Dacă nu, returnăm o imagine default (ID 0) ca să nu crape aplicația.
            if (BuddyImages.TryGetValue(id, out var path))
            {
                return path;
            }
            return BuddyImages[0];
        }
    }
}
