using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Y.ASIS.App.Services.CameraService;
using Y.ASIS.App.UserControls;
using Y.ASIS.App.ViewModels;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (AppGlobal.Env == AppEnvironment.Production)
            {
                vm.Initialize();
            }
            vm.NotifyView += OnNotifyView;

            App.Current.MainWindow = this;
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow()
            {
                Owner = this
            };
            bool? result = login.ShowDialog();
            if ((bool)result)
            {
                vm.CurrentUser = login.User;

                UnloginPanel.Visibility = Visibility.Collapsed;
                HasLoginPanel.Visibility = Visibility.Visible;
            }
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageWindow.Show($"确定退出当前用户？", "提示", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            vm.CurrentUser = null;
            UnloginPanel.Visibility = Visibility.Visible;
            HasLoginPanel.Visibility = Visibility.Collapsed;
        }

        private void ReturnButtoClick(object sender, RoutedEventArgs e)
        {
            positionFrm.Visibility = Visibility.Collapsed;
            trackFrm.Visibility = Visibility.Visible;
            vm.CurrentPosition.Videos.ForEach(i => i.Playing = false);
        }


        public void OnCurrentTrackChanged(object sender, RoutedEventArgs e)
        {
            CurrentTrackChangedRoutedEventArgs args = e as CurrentTrackChangedRoutedEventArgs;
            vm.CurrentTrack = args.Track;
            vm.CurrentPosition = args.Track.Positions.FirstOrDefault(i => i.IsSelected);
            trackFrm.Visibility = Visibility.Collapsed;
            positionFrm.Visibility = Visibility.Visible;

            Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    vm.CurrentPosition.Videos.ForEach(i => i.Playing = true);
                }), System.Windows.Threading.DispatcherPriority.Background);
            });

            Task.Run(() =>
            {
                HIKNVRService.LinkPositionVideo(vm.CurrentPosition);
            });
        }

        public void OnCurrentPositionChanged(object sender, RoutedEventArgs e)
        {
            CurrentPositionChangedRoutedEventArgs args = e as CurrentPositionChangedRoutedEventArgs;
            if (vm.CurrentPosition == args.Position)
                return;
            Task.Factory.StartNew(() =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    vm.CurrentPosition.Videos.ForEach(i => i.Playing = false);

                    // update current position
                    vm.CurrentPosition = args.Position;
                    vm.CurrentPosition.Videos.ForEach(i => i.Playing = true);
                    // link video 
                    //HIKNVRService.LinkPositionVideo(vm.CurrentPosition);

                }), System.Windows.Threading.DispatcherPriority.Background);
            });
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                e.Handled = true;
            }
            else
            {
                if (e.ClickCount == 1)
                {
                    this.DragMove();
                }
                else if (e.ClickCount == 2)
                {
                    MaximizeNormalize_Clicked(null, null);
                }
            }
        }

        private object OnNotifyView(ViewModelMessage type, params object[] args)
        {
            switch (type)
            {
                case ViewModelMessage.CurrentUserUpdated:
                    MessageWindow.Show($"检测到当前用户登录信息已改变, 请重新登录!");
                    UnloginPanel.Visibility = Visibility.Visible;
                    HasLoginPanel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
            return null;
        }

        private void Minimize_Clicked(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MaximizeNormalize_Clicked(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    SystemCommands.MaximizeWindow(this);
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    SystemCommands.RestoreWindow(this);
                    break;
            }
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown(); // 释放所有资源
            SystemCommands.CloseWindow(this);
        }


    }
}
