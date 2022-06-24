using System;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Converters
{
    class OperationTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 服务器或备用操作台下发指令（如果接受即在最高位前加1，拒绝加2，如警示指令拒绝2101）
            string srcCode = value.ToString(); // 原始操作码
            string prefix = ""; // 操作码前缀
            string code;        // 操作码
            string text;
            if (srcCode.Length == 4)
            {
                prefix = srcCode.Substring(0, 1);
                code = srcCode.Substring(1, srcCode.Length - 1);
            }
            else
            {
                code = srcCode;
            }

            if (Enum.TryParse(code, out PLCOperateCode type))
            {
                if (prefix == "2")  // 指令被拒绝
                {
                    text = $"{type}(拒绝)";
                }
                else
                {
                    text = $"{type}";
                }
            }
            else
            {
                text = $"{PLCOperateCode.Error}";
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
