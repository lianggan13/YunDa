using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class RoleIdToRoleNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values.Length == 2
                && values[0] is int id
                && values[1] is IEnumerable<Role> roles
                && roles != null
                && roles.Any(i => i.Id == id))
            {
                return roles.FirstOrDefault(i => i.Id == id).Name;
            }
            return "Unknown";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
