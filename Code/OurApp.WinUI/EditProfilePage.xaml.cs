using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;
using System;

namespace OurApp.WinUI;

public sealed partial class EditProfilePage : Page
{
    public EditCompanyProfileViewModel ViewModel { get; }

    public EditProfilePage()
    {
        var mainWindow = App.mainWindow;
        ViewModel = new EditCompanyProfileViewModel(mainWindow.companyService);
        InitializeComponent();
        DataContext = ViewModel;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var id = e.Parameter is int companyId ? companyId : 1;
        ViewModel.Load(id);
    }

    private void NavigateBack_Click(object sender, RoutedEventArgs e)
    {
        var mainW = App.mainWindow;
        if (mainW.RootFrame.CanGoBack)
            mainW.RootFrame.GoBack();
        else
            mainW.RootFrame.Navigate(typeof(ViewProfilePage), ViewModel.CompanyId);
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        var err = ViewModel.TrySave();
        if (err is null)
        {
            var ok = new ContentDialog
            {
                Title = "Profile saved",
                Content = "Your changes were saved.",
                CloseButtonText = "OK",
                XamlRoot = XamlRoot
            };
            await ok.ShowAsync();
            App.mainWindow.RootFrame.Navigate(typeof(ViewProfilePage), ViewModel.CompanyId);
            return;
        }

        var bad = new ContentDialog
        {
            Title = "Could not save",
            Content = err,
            CloseButtonText = "OK",
            XamlRoot = XamlRoot
        };
        await bad.ShowAsync();
    }

    private async void Cancel_Click(object sender, RoutedEventArgs e)
    {
        var confirm = new ContentDialog
        {
            Title = "Discard changes?",
            Content = "Are you sure you want to cancel the modifications?",
            PrimaryButtonText = "Yes",
            CloseButtonText = "No",
            XamlRoot = XamlRoot
        };
        var result = await confirm.ShowAsync();
        if (result != ContentDialogResult.Primary)
            return;

        NavigateBack_Click(sender, e);
    }
}
