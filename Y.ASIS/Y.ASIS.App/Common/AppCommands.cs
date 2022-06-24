using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Y.ASIS.App.Communication;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.Common
{
    public class AppCommands
    {
        public static RelayCommand CheckBoxGridViewHeaderCommand;

        public static RelayCommand PositionSignalLightSwitchCommand;

        public static RelayCommand PositionGateSwitchCommand;

        static AppCommands()
        {
            CheckBoxGridViewHeaderCommand = new RelayCommand(CheckBoxGridViewHeaderIsCheckChanged);
            PositionSignalLightSwitchCommand = new RelayCommand(PositionSignalLightSwitch);
            PositionGateSwitchCommand = new RelayCommand(PositionGateSwitch);
        }

        private static void CheckBoxGridViewHeaderIsCheckChanged(object parameter)
        {
            CheckBox checkBox = parameter as CheckBox;
            if (checkBox.Tag is IEnumerable<object> list && list.Any())
            {
                object obj = list.First();
                Type type = obj.GetType();
                PropertyInfo property = type.GetProperty("IsChecked");
                if (property != null)
                {
                    foreach (object o in list)
                    {
                        property.SetValue(o, checkBox.IsChecked);
                    }
                }
            }
        }

        private static void PositionSignalLightSwitch(object parameter)
        {
            if (!(parameter is string parameters))
            {
                return;
            }
            int[] array = parameters.Split('|').Select(i => Convert.ToInt32(i)).ToArray();
            int positionId = array[0];
            int index = array[1];
            int command = array[2];
            PositionSignalLightSwitchRequest request = new PositionSignalLightSwitchRequest(positionId, index, command);
            request.RequestAsync<object>(null);
        }

        private static void PositionGateSwitch(object parameter)
        {
            if (!(parameter is string parameters))
            {
                return;
            }
            int[] array = parameters.Split('|').Select(i => Convert.ToInt32(i)).ToArray();
            int positionId = array[0];
            int index = array[1];
            int command = array[2];
            PositionGateSwitchRequest request = new PositionGateSwitchRequest(positionId, index, command);
            request.RequestAsync<object>(null);
        }
    }
}
