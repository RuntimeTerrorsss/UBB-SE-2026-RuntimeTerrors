using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;
using System;

namespace OurApp.WinUI;

public sealed partial class ViewProfilePage : Page
{
    public CompanyProfileViewModel ViewModel { get; }

    public ViewProfilePage()
    {
        var repo = new CompanyRepo();
        var companyService = new CompanyService(repo);
        ViewModel = new CompanyProfileViewModel(companyService, new ProfileCompletionCalculator());
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.NavigateEditProfileRequested += (_, _) =>
        {
            App.MainWin.RootFrame.Navigate(typeof(EditProfilePage), ViewModel.CompanyId);
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var id = e.Parameter is int companyId ? companyId : 1;
        ViewModel.Load(id);
    }
}
