using System.Collections.Generic;
using System.Windows;
using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// ViewHandleWarningAndFaultWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ViewHandleWarningAndFaultWindow : PopupWindow
    {
        private readonly HandleWarningAndFaultViewModel vm;

        public ViewHandleWarningAndFaultWindow(IEnumerable<FaultMessages> warnings, FaultMessages current)
        {
            vm = new HandleWarningAndFaultViewModel(warnings, current);
            InitializeComponent();
            DataContext = vm;
            vm.NotifyView += OnNotifyView;
        }

        private object OnNotifyView(ViewModelMessage type, params object[] args)
        {
            switch (type)
            {
                case ViewModelMessage.Close:
                    if (Owner != null)
                    {
                        DialogResult = true;
                    }
                    Close();
                    break;
            }

            return null;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
