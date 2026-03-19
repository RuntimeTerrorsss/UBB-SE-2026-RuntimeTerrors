using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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
        public CreateEventPage()
        {
            InitializeComponent(); 
            ViewModel = new CreateEventViewModel(); 
            this.DataContext = ViewModel;
            //System.Diagnostics.Debug.WriteLine("DataContext set!"); 
        }

        private void Title_LostFocus(object sender, RoutedEventArgs e)
        {
            var binding = TitleBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();

            EventValidator validator = ViewModel.validator;

            string value = ViewModel?.Title;
            System.Diagnostics.Debug.WriteLine($"Title value: {(value == null ? "NULL" : value)}");

            try
            {
                if (validator.TitleValidator(ViewModel.Title))
                {
                    TitleError.Text = ""; // clear previous error
                    TitleBox.BorderBrush = new SolidColorBrush(Colors.Green);
                }
            }
            catch (Exception ex)
            {
                TitleError.Text = ex.Message; // clear previous error
                TitleBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            
        }
    }
}
