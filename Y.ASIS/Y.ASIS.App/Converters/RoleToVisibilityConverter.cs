using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Role role)
            {
                string functionString = parameter.ToString();
                List<int> functions = functionString.Split('|').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
                return role.Functions != null && role.Functions.Any(i => functions.Contains(i))
                ? Visibility.Visible
                : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
