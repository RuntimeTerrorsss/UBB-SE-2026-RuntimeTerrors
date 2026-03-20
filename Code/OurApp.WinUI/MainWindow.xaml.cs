using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;

namespace OurApp.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public GameViewModel ViewModel { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            var repo = new MockRepository();
            var service = new GameService(repo);
            ViewModel = new GameViewModel(service);

            this.Bindings.Update();

            TestLogic();
        }

        private void OnChoiceButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && ViewModel != null)
            {
                string choiceText = button.Content.ToString();
                int index = ViewModel.CurrentChoices.IndexOf(choiceText);
                ViewModel.OnChoiceSelected(index);
                this.Bindings.Update();
            }
        }

        public void TestLogic()
        {
            if (ViewModel == null) return;

            System.Diagnostics.Debug.WriteLine(ViewModel.WelcomeMessage);
            System.Diagnostics.Debug.WriteLine("Scenariu: " + ViewModel.CurrentQuestion);

            foreach (var choice in ViewModel.CurrentChoices)
            {
                System.Diagnostics.Debug.WriteLine("Varianta: " + choice);
            }
        }
    }
}