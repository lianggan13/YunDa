using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class UserGroupIdToGroupNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2
                && values[0] is int id
                && values[1] is IEnumerable<UserGroup> groups
                && groups.Any(i => i.Id == id))
            {
                return groups.FirstOrDefault(i => i.Id == id).Name;
            }
            //return "";
            return "未分组";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
