using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class VideoTypeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VideoType type = (VideoType)value;
            switch (type)
            {
                case VideoType.Pantograph:
                    return "受电弓";
                case VideoType.TrainNo:
                    return "车号";
                case VideoType.Isolation:
                    return "隔离开关";
                case VideoType.Grounding:
                    return "接地装置";
                case VideoType.Elec:
                    return "验电装置";
                default:
                    return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
