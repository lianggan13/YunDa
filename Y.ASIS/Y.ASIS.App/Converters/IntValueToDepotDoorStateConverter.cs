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
    class IntValueToDepotDoorStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            switch (val)
            {
                case 1:
                    return DepotDoorState.Open;
                case 2:
                    return DepotDoorState.Close;
                default:
                    return DepotDoorState.Unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
