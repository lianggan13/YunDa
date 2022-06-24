using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class MutiSignalLightAndGateVisiblityConverter : IMultiValueConverter
    {
        public bool IsHide { get; set; }

        public bool Reverse { get; set; }

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {

            Visibility[] visibilities = new Visibility[2];
            string[] parameterString = parameter.ToString().Split('-');

            if (value[0] != null && value[1] != null)
            {
                if (!int.TryParse(value[0].ToString(), out int index))
                {
                    return Visibility.Collapsed;
                }
                //int index = (int)value[0];
                Role role = value[1] as Role;

                List<int> values = parameterString[0].Split('|').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
                visibilities[0] = values.Contains(index) ? Visibility.Visible : (IsHide ? Visibility.Hidden : Visibility.Collapsed);
                if (Reverse)
                {
                    visibilities[0] = visibilities[0] > 0 ? Visibility.Visible : (IsHide ? Visibility.Hidden : Visibility.Collapsed);
                }

                List<int> functions = parameterString[1].Split('|').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
                if (role == null)
                {
                    visibilities[1] = Visibility.Collapsed;
                }
                else
                {
                    visibilities[1] = role.Functions != null && role.Functions.Any(i => functions.Contains(i))
                                        ? Visibility.Visible
                                        : Visibility.Collapsed;
                }

                return visibilities[0] == visibilities[1] ? visibilities[0] : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
