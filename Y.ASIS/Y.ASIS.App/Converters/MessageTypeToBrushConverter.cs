using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Converters
{
    class MessageTypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageType type = (MessageType)value;
            switch (type)
#pragma warning disable CS1522 // 空的 switch 块
            {
#pragma warning restore CS1522 // 空的 switch 块
                //case MessageType.Fault:
                //    return Brushes.Red;
                //default:
                //    return Brushes.LightBlue;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
