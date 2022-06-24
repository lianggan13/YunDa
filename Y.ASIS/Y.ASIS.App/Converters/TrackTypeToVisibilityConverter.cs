using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class TrackTypeToVisibilityConverter : IValueConverter
    {
        public TrackType VisibleType { get; set; } = TrackType.TPPTT;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TrackType type = (TrackType)value;
            return type == VisibleType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
