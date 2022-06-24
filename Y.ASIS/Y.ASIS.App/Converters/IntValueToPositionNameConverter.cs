using System;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class IntValueToPositionNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return PositionType.Unknown;
            }
            int val = (int)value;
            switch (val)
            {
                case 1:
                    return PositionType.FirstTrackFirstPosition;
                case 2:
                    return PositionType.SecondTrackFirstPosition;
                case 3:
                    return PositionType.ThirdTrackFirstPosition;
                case 4:
                    return PositionType.ThirdTrackSecondPosition;
                default:
                    return PositionType.Unknown;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
