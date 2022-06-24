using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Common;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.Models.Enums;

namespace Y.ASIS.App.Views.Frames
{
    /// <summary>
    /// PositionFrame.xaml 的交互逻辑
    /// </summary>
    public partial class PositionFrame : UserControl
    {
        public PositionFrame()
        {
            InitializeComponent();
        }


        private void SafeConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = AppGlobal.Instance.MainVM;
            if (vm.CurrentPosition != null)
            {
                SafeConfirmWindow window = new SafeConfirmWindow(vm.CurrentPosition, vm.CurrentUser)
                {
                    Owner = Application.Current.MainWindow,
                };
                window.ShowDialog();
            }
        }

        private void OnCurrentPositionChanged(object sender, RoutedEventArgs e)
        {
            var mwin = Application.Current.MainWindow as MainWindow;
            mwin.OnCurrentPositionChanged(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (AppGlobal.Instance.Project)
            {
                case ProjectType.NationalRailway:
                    break;
                case ProjectType.CityRailway_1:
                    break;
                case ProjectType.CityRailway_2:
                    btnChkElec.SetBinding(Button.VisibilityProperty, "");
                    btnMvChkElec.SetBinding(Button.VisibilityProperty, "");
                    btnChkElec.Visibility = Visibility.Collapsed;
                    btnMvChkElec.Visibility = Visibility.Collapsed;
                    break;
                case ProjectType.Shenzhen12:
                    safeConfirmPal.Visibility = Visibility.Collapsed; // 隐藏 安全确认 StackPanel 
                    break;
            }
        }
    }
}
