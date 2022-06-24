using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Y.ASIS.App.Ctls.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool IsTrueToVisible { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = value as bool?;
            string strParam = parameter as string;
            bool isHide = (strParam ?? string.Empty).ToLower() == "hide";
            if (isHide)
            {
                if (val.HasValue && val.Value)
                {
                    return IsTrueToVisible ? Visibility.Visible : Visibility.Hidden;
                }
                return IsTrueToVisible ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                if (val.HasValue && val.Value)
                {
                    return IsTrueToVisible ? Visibility.Visible : Visibility.Collapsed;
                }
                return IsTrueToVisible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility? val = value as Visibility?;
            bool result = val.HasValue;
            if (result)
            {
                result = val.Value == Visibility.Visible;
            }
            return result;
        }
    }
}
