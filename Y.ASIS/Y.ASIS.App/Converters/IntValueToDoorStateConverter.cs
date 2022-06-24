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
    class IntValueToDoorStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DoorState.Unknown;
            }
            int val = (int)value;
            switch (val)
            {
                case 1:
                    return DoorState.PowerOnOpen;
                case 2:
                    return DoorState.PowerOnClose;
                case 3:
                    return DoorState.PowerOffOpen;
                case 4:
                    return DoorState.PowerOffClose;
                default:
                    return DoorState.Unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
