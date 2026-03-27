using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;

namespace OurApp.WinUI.Converters
{
    public class BoolToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // value will be a tuple-like or combined object via MultiBinding alternative
            // BUT since WinUI doesn't support MultiBinding easily,
            // we will pass a custom wrapper string: "touched|valid"

            if (value is string s)
            {
                var parts = s.Split('|');

                if (parts.Length != 2)
                    return new SolidColorBrush(Colors.Transparent);

                bool isTouched = bool.Parse(parts[0]);
                bool isValid = bool.Parse(parts[1]);

                if (!isTouched)
                    return new SolidColorBrush(Colors.Transparent);

                return isValid
                    ? new SolidColorBrush(Colors.Green)
                    : new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}