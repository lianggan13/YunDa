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
    class IntValueToSignalLightStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            switch (val)
            {
                case 1:
                    return SignalLightState.White;
                case 2:
                    return SignalLightState.Red;
                default:
                    return SignalLightState.Unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
