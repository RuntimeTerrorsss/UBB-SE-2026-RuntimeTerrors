using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OurApp.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public Frame RootFrame => rootFrame;

        ICompanyService service;
        public MainWindow()
        {
            ICompanyRepo repo = new CompanyRepo();
            this.service = new CompanyService(repo);

            service.addCompany("ndj", "dnis", "dnjs", "hdjd", "sybau", "dj@");
            service.addCompany("ndj2", "dnis", "dnjs", "hdjd", "sybau", "dj@");
            service.printAll();
            InitializeComponent();
        }

        private void NavigateToViewProfile_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate(typeof(ViewProfilePage));
        }

        private void NavigateToEditProfile_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate (typeof(EditProfilePage));
        }
    }
}
