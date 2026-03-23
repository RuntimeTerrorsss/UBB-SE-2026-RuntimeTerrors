using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Models;
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

namespace OurApp.WinUI;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OurEventsPage : Page
{
    public OurEventsViewModel ViewModel { get; }
    public OurEventsPage()
    {
        var mainW = App.MainWin;
        InitializeComponent();
        ViewModel = new OurEventsViewModel(mainW.eventsService);
        this.DataContext = ViewModel;
    }

    private void CreateEventButton_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = App.MainWin;
        mainWindow.RootFrame.Navigate(typeof(CreateEventPage));
    }

    private void EditEvent_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedEvent = button.Tag as Event;

        var mainWindow = App.MainWin;
        mainWindow.RootFrame.Navigate(typeof(EditEventPage), selectedEvent);
    }
}
