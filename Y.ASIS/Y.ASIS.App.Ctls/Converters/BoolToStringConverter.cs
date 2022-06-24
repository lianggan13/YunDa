using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Y.ASIS.App.Ctls.Converters
{
    [ValueConversion(typeof(bool?), typeof(string))]
    public class BoolToStringConverter
    : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ss = parameter.ToString().Split('|');   // parameter RMC:Collapsed,RMC:Visible
            //✔


            if (bool.TryParse(value?.ToString(), out bool v))
            {

            }
            else
            {

            }

            string parKey = ss.ElementAt(0);
            object parValue = bool.Parse(ss.ElementAt(1)); //Enum.Parse(typeof(Visibility), ss.ElementAt(1));
            if (value == null)
            {
                return null;
            }
            else if (value.Equals(parKey))
            {
                // 满足条件 返回 parameter 中 parValue
                return parValue;
            }
            else
            {
                return !(bool)parValue;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
