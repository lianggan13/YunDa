using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Y.ASIS.App.Converters
{
    class DoubleValueToTextConverter : IValueConverter
    {
        public double Threshold { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
            if (value is double val)
            {
                return val >= Threshold ? "∞" : val.ToString();
            }
            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class BrushToCollapsedConverter : IValueConverter
    {
        public SolidColorBrush TragetBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;

            if (value is SolidColorBrush brush)
            {
                if (TragetBrush == brush)
                    visibility = Visibility.Collapsed;
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
