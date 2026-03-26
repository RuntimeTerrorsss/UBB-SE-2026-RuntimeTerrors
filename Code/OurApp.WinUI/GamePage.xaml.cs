using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OurApp.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GameViewModel ViewModel { get; set; }

        public GamePage()
        {
            var repo = new MockRepository();
            var service = new GameService(repo);
            ViewModel = new GameViewModel(service);

            this.InitializeComponent();
        }


        private void OnStartGameClick(object sender, RoutedEventArgs e)
        {
            ViewModel.StartGame();
        }

        private void OnChoiceButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content != null)
            {
                string choiceText = button.Content.ToString() ?? "";
                if (ViewModel.CurrentChoices != null)
                {
                    int index = ViewModel.CurrentChoices.IndexOf(choiceText);
                    if (index != -1)
                    {
                        ViewModel.OnChoiceSelected(index);
                    }
                }
            }
        }

        private void OnNextStepClick(object sender, RoutedEventArgs e)
        {
            ViewModel.GoToNextStep();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        { 
          if (this.XamlRoot?.Content != null)
            {
                Application.Current.Exit();
            }
        }
    }
}

