using System;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class FaultTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                PLCFaultCode type = (PLCFaultCode)value;
                return type;
            }
            catch
            {
                return PLCFaultCode.Error;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
