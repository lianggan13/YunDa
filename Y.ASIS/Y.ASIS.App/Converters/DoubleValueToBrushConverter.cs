using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Y.ASIS.App.Converters
{
    class DoubleValueToBrushConverter : IValueConverter
    {
        public double Threshold { get; set; }

        public Brush LessThanBrush { get; set; }

        public Brush GreaterThanBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                return val < Threshold ? LessThanBrush : GreaterThanBrush;
            }
            return GreaterThanBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
