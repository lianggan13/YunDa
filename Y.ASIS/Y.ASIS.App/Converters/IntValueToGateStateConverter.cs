using System;
using System.Globalization;
using System.Windows.Data;
using Y.ASIS.App.Ctls.Controls;

namespace Y.ASIS.App.Converters
{
    class IntValueToGateStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //  库门 ::AsGlobalPV:State.Gate[n]
            //  00 = 初始化检测中
            //  01 = 开门
            //  02 = 关门
            //  10 = 故障状态
            //  11 = 未配置

            int val = (int)value;
            var state = GateState.Offline;
            switch (val)
            {
                case 1:
                    state = GateState.Open;
                    break;
                case 2:
                    state = GateState.Close;
                    break;
                case 10:
                    state = GateState.Offline;
                    break;
                default:
                    break;
            }

            //return val == 1 ? GateState.Open : GateState.Close;
            return state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
