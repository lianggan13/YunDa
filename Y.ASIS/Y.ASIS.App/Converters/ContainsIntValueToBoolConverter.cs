using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class ContainsIntValueToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            string parameterString = parameter.ToString();
            List<int> values = parameterString.Split('|').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
            return values.Contains(val);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
