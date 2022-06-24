using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Converters
{
    class StringToVisibilityConverter : IValueConverter
    {
        public bool IsNullOrEmptyOrWhiteSpaceToVisiable { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hide = parameter != null && (parameter as string).ToUpper() == "HIDE";
            return (value as string).IsNullOrEmptyOrWhiteSpace()
                ? (IsNullOrEmptyOrWhiteSpaceToVisiable ? Visibility.Visible : (hide ? Visibility.Hidden : Visibility.Collapsed))
                : (IsNullOrEmptyOrWhiteSpaceToVisiable ? (hide ? Visibility.Hidden : Visibility.Collapsed) : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
