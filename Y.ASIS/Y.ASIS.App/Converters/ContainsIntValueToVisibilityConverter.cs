using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class ContainsIntValueToVisibilityConverter : IValueConverter
    {
        public bool IsHide { get; set; }

        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            string parameterString = parameter.ToString();
            List<int> values = parameterString.Split('|').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
            Visibility visibility = values.Contains(val) ? Visibility.Visible : (IsHide ? Visibility.Hidden : Visibility.Collapsed);
            if (Reverse)
            {
                visibility = visibility > 0 ? Visibility.Visible : (IsHide ? Visibility.Hidden : Visibility.Collapsed);
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
