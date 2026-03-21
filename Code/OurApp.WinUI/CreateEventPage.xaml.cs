using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Validators;
using OurApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OurApp.WinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateEventPage : Page
    {
        public CreateEventViewModel ViewModel { get; }
        private bool IsLoaded = false;
        private bool StartDateModified = false;
        private bool EndDateModified = false;
        public CreateEventPage()
        {
            var mainW = App.MainWin;
            ViewModel = new CreateEventViewModel(mainW.service);
            this.DataContext = ViewModel;

            InitializeComponent();

            IsLoaded = true;
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            // because click usually runs before command so we must make it run before
            ViewModel.TapCommand.Execute(null);

            if (ViewModel.AddError == "")
            {
                var mainWindow = App.MainWin;
                mainWindow.RootFrame.Navigate(typeof(OurEventsPage));
            }
        }

        private async void AttachImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            // Common image formats
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".gif");

            // Required for WinUI 3 pickers
            IntPtr hwnd = WindowNative.GetWindowHandle(App.MainWin);
            InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file == null)
                return;

            PhotoFileNameTextBlock.Text = file.Name;

            // Read file bytes and convert to base64 (stored in ViewModel.Photo)
            byte[] bytes;
            using (var input = await file.OpenReadAsync())
            using (var reader = new DataReader(input.GetInputStreamAt(0)))
            {
                await reader.LoadAsync((uint)input.Size);
                bytes = new byte[input.Size];
                reader.ReadBytes(bytes);
            }

            ViewModel.Photo = Convert.ToBase64String(bytes);

            // Create preview image from the selected bytes
            var bitmapImage = new BitmapImage();
            using (var memStream = new InMemoryRandomAccessStream())
            {
                await memStream.WriteAsync(bytes.AsBuffer());
                memStream.Seek(0);
                bitmapImage.SetSource(memStream);
            }
            PhotoPreviewImage.Source = bitmapImage;
        }

        private void Title_LostFocus(object sender, RoutedEventArgs e)
        {
            var binding = TitleBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            if (ViewModel.ValidateTitle())
            {
                TitleBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                TitleBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void Description_LostFocus(object sender, RoutedEventArgs e)
        {
            var binding = DescriptionBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            if (ViewModel.ValidateDescription())
            {
                DescriptionBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                DescriptionBox.BorderBrush= new SolidColorBrush(Colors.Red);
            }
        }

        private void Location_LostFocus(object sender, RoutedEventArgs e)
        {
            var binding = LocationBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            if (ViewModel.ValidateLocation())
            {
                LocationBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                LocationBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }

        }

        private void StartDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (!IsLoaded)
                return;

            if (!StartDateModified)
            {
                StartDateModified = true;
                return;
            }

            if (ViewModel.ValidateStartDate())
            {
                StartDatePicker.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                StartDatePicker.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void EndDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (!IsLoaded)
                return;

            if (!EndDateModified)
            {
                EndDateModified = true;
                return;
            }

            if (ViewModel.ValidateEndDate())
            {
                EndDatePicker.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                EndDatePicker.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (StartDateModified)
            {
                if (ViewModel.ValidateDatesCronologity())
                {
                    EndDatePicker.BorderBrush = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    EndDatePicker.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
        }
    }
}
