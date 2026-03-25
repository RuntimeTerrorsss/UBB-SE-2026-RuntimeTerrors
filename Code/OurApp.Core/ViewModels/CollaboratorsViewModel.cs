using CommunityToolkit.Mvvm.ComponentModel;
using OurApp.Core.Models;
using OurApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurApp.Core.ViewModels
{
    public partial class CollaboratorsViewModel : ObservableObject
    {
        public List<Company> allCollaborators { get; }
        private readonly CollaboratorsService collaboratorsService;
        private readonly SessionService sessionService;
        public CollaboratorsViewModel(CollaboratorsService collaboratorsService, SessionService sessionService)
        {
            this.collaboratorsService = collaboratorsService;
            this.sessionService = sessionService;

            this.allCollaborators = collaboratorsService.GetAllCollaborators(sessionService.loggedInUser.Id);
        }
    }
}
