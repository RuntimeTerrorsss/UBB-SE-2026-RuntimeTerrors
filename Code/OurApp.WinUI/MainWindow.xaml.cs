using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OurApp.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public Frame RootFrame => rootFrame;
        public EventsService eventService { get; }

        public GameService gameService;

        ICompanyService companyService;
        public MainWindow()
        {
            this.InitializeComponent();

            IGameRepo game_repo = new GameMemoryRepo();
            this.gameService = new GameService(game_repo);

            ICompanyRepo repo = new CompanyRepo();
            this.companyService = new CompanyService(repo);

            companyService.addCompany("Acme Corp", "We build things.", "profile.jpg", "logo.png", "Cluj", "hello@acme.test");
            companyService.printAll();

            IEventsRepo eventRepo = new EventsRepo();

            // hardcode events
            Event ev1 = new Event("", "Event1", "This is such a cool event. You should attend.", new DateTime(2026, 1, 21, 14, 0, 0), new DateTime(2026, 1, 24, 18, 0, 0), "Cluj-Napoca, Cluj", 1, 2);
            Event ev2 = new Event("", "Event2", "This is another event. You should attend.", new DateTime(2026, 3, 21, 14, 0, 0), new DateTime(2026, 3, 24, 18, 0, 0), "Cluj-Napoca, Cluj", 1, 2);
            Event ev3 = new Event("", "Event3", "Join us.", new DateTime(2026, 5, 21, 14, 0, 0), new DateTime(2026, 5, 21, 18, 0, 0), "Sibiu, Sibiu", 1, 2);
            Event ev4 = new Event("", "Event4", "", new DateTime(2026, 3, 18, 14, 0, 0), new DateTime(2026, 3, 19, 18, 0, 0), "Bucuresti", 1, 2);

            eventRepo.Add(ev1);
            eventRepo.Add(ev2);
            eventRepo.Add(ev3);
            eventRepo.Add(ev4);
            eventService = new EventsService(eventRepo);
        }

        private void NavigateToEditGame_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Clicked to editgame");
            RootFrame.Navigate(typeof(EditGame), gameService);
        }

        private void NavigateToGamePage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Clicked to nav");
            RootFrame.Navigate(typeof(GamePage), gameService);
        }

        private void NavigateToOurEvents_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Clicked to nav");
            RootFrame.Navigate(typeof(OurEventsPage));
        }

        private void NavigateToPastEvents_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate(typeof(PastEventsPage));
        }

        private void NavigateToViewProfile_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate(typeof(ViewProfilePage), 1);
        }

        private void NavigateToEditProfile_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate(typeof(EditProfilePage), 1);
        }
    }
}