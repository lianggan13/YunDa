using System;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class IssueTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IssueType type = (IssueType)value;
            switch (type)
            {
                case IssueType.InAndOut:
                    return "进出股道";
                case IssueType.OnlyIn:
                    return "进股道";
                case IssueType.OnlyOut:
                    return "出股道";
                default:
                    throw new Exception();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
