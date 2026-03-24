using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class EditGameViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _buddyName;
    }
}
