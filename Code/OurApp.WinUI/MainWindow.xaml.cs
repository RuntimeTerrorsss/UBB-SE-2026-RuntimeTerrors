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
      

        public GameService gameService;


        public MainWindow()
        {
            this.InitializeComponent();

            IGameRepo game_repo = new GameMemoryRepo();
            this.gameService = new GameService(game_repo);

           
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
    }
}