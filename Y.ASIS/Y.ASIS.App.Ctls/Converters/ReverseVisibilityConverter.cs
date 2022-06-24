using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Y.ASIS.App.Ctls.Converters
{
    public class ReverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            string strParam = parameter as string;
            bool isHide = (strParam ?? string.Empty).ToLower() == "hide";
            return visibility == Visibility.Visible 
                ? (isHide ? Visibility.Hidden : Visibility.Collapsed)
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
