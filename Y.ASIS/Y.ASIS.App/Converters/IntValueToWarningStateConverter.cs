using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Y.ASIS.App.Ctls.Controls;

namespace Y.ASIS.App.Converters
{
    class IntValueToWarningStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            if (val == 1)
            {
                return WarningLightState.Warning;
            }
            else if (val == 2)
            {
                return WarningLightState.NoWarning;
            }
            return WarningLightState.Unknown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
