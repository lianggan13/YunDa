using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// SafeConfirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SafeConfirmWindow : PopupWindow
    {
        public SafeConfirmWindow(Position position, User user)
        {
            InitializeComponent();
            vm.NotifyView += OnNotifyView;
            vm.FillData(position, user);
        }

        private void SafeConfirmWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.StartAlgorithmTask();
        }

        private object OnNotifyView(ViewModelMessage type, params object[] args)
        {
            if (type == ViewModelMessage.Close)
            {
                Close();
            }
            return null;
        }
    }
}
