using System;
using System.Globalization;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class DoubleValueToElecTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = null;
            if (value is double val)
            {
                text = val < 0 ? "Error" : $"{val} V";
            }
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
