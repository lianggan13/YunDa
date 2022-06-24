using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class IEnumableToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumerable)
            {
                IEnumerator enumerator = enumerable.GetEnumerator();
                StringBuilder builder = new StringBuilder();
                while (enumerator.MoveNext())
                {
                    builder.AppendFormat("{0}、", enumerator.Current);
                }
                string s = builder.ToString();
                return s.TrimEnd('、');
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
