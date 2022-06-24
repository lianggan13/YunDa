using System;
using System.Globalization;
using System.Windows.Data;

namespace Y.ASIS.App.Ctls.Converters
{
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class ReverseBoolConverter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.TryParse(value?.ToString(), out bool b))
            {
                return !b;
            }
            else
            {
                return null;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
