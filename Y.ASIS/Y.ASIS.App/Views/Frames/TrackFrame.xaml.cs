using System.Windows;
using System.Windows.Controls;
using Y.ASIS.App.Common;
using Y.ASIS.App.Windows;

namespace Y.ASIS.App.Views.Frames
{
    /// <summary>
    /// TrackFrame.xaml 的交互逻辑
    /// </summary>
    public partial class TrackFrame : UserControl
    {
        public TrackFrame()
        {
            InitializeComponent();
        }

        private void OnCurrentTrackChanged(object sender, RoutedEventArgs e)
        {
            var mwin = Application.Current.MainWindow as MainWindow;
            mwin.OnCurrentTrackChanged(sender, e);
        }

        private void ConfigButtonClick(object sender, RoutedEventArgs e)
        {
            ConfigWindow window = new ConfigWindow()
            {
                Owner = Application.Current.MainWindow,
            };
            window.ShowDialog();
        }

        private void AuthorityManagerButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = AppGlobal.Instance.MainVM;

            AuthorityManagerWindow window = new AuthorityManagerWindow(vm.Tracks)
            {
                Owner = Application.Current.MainWindow,
            };
            window.ShowDialog();
        }

        private void QueryButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = AppGlobal.Instance.MainVM;

            QueryWindow window = new QueryWindow(vm.Tracks)
            {
                Owner = Application.Current.MainWindow,
            };
            window.ShowDialog();
        }
    }
}
