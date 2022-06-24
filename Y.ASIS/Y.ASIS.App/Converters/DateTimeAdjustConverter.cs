using System;
using System.Globalization;
using System.Windows.Data;

namespace Y.ASIS.App.Converters
{
    class DateTimeAdjustConverter : IValueConverter
    {
        public double AddDays { get; set; } = 0;

        public double SubDays { get; set; } = 0;

        public double SubSeconds { get; set; } = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HandleConvert(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HandleConvert(value);
        }

        private DateTime HandleConvert(object value)
        {
            DateTime time = (DateTime)value;
            time = time.Date;  // just pick up date part

            time = time.AddDays(AddDays).AddSeconds(-SubSeconds);
            time -= TimeSpan.FromDays(SubDays);
            return time;
        }
    }
}
