using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class UserFunctionToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<int> functionIds = (IEnumerable<int>)value;
            if (functionIds == null || !functionIds.Any())
            {
                return "---";
            }
            if (!(AppGlobal.Instance.GetData("Functions") is IEnumerable<Function> functions))
            {
                return string.Join("、", functionIds);
            }
            return string.Join("、", functionIds.Select(i => functions.First(j => j.Id == i).Name));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
