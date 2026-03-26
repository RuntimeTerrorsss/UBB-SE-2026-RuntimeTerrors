using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media.Imaging;

namespace OurApp.WinUI;

public sealed partial class ViewProfilePage : Page
{
    public CompanyProfileViewModel ViewModel { get; }

    public ViewProfilePage()
    {
        var mainWindow = App.mainWindow;
        ViewModel = new CompanyProfileViewModel(mainWindow.companyService, new ProfileCompletionCalculator());
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.NavigateEditProfileRequested += (_, _) =>
        {
            App.mainWindow.RootFrame.Navigate(typeof(EditProfilePage), ViewModel.CompanyId);
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var id = e.Parameter is int companyId ? companyId : 1;
        ViewModel.Load(id);

        TryRenderCompanyLogo();
    }

    private async void TryRenderCompanyLogo()
    {
        try
        {
            var raw = ViewModel.Company?.CompanyLogoPath ?? "";
            if (string.IsNullOrWhiteSpace(raw))
            {
                CompanyLogoHintText.Text = "(no logo)";
                CompanyLogoBrush.ImageSource = null;
                return;
            }

            // If the DB stored a data-URI, decode and show it.
            const string prefix = "data:image/";
            const string base64Marker = ";base64,";
            if (raw.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var base64Index = raw.IndexOf(base64Marker, StringComparison.OrdinalIgnoreCase);
                if (base64Index < 0)
                    throw new FormatException("Invalid image data URI.");

                var base64 = raw.Substring(base64Index + base64Marker.Length);
                var bytes = Convert.FromBase64String(base64);

                var bitmap = new BitmapImage();
                using (var mem = new InMemoryRandomAccessStream())
                {
                    await mem.WriteAsync(bytes.AsBuffer());
                    mem.Seek(0);
                    bitmap.SetSource(mem);
                }

                CompanyLogoHintText.Text = "";
                CompanyLogoBrush.ImageSource = bitmap;
                return;
            }

            // Otherwise, don't spam the UI with the raw string.
            CompanyLogoHintText.Text = "(logo set)";
            CompanyLogoBrush.ImageSource = null;
        }
        catch
        {
            CompanyLogoHintText.Text = "(logo could not be rendered)";
            CompanyLogoBrush.ImageSource = null;
        }
    }
}
