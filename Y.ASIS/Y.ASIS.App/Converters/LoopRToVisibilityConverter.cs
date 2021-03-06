using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class LoopRToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<double> loopR = value as IList<double>;
            string parameterString = parameter as string;
            int.TryParse(parameterString, out int index);
            try
            {
                return loopR != null && loopR[index] >= 0 && loopR[index] <= 5 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
