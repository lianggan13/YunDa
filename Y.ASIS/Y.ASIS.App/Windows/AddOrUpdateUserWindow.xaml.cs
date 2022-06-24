using System.Windows;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// AddOrUpdateUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateUserWindow : PopupWindow
    {
        private readonly AddOrUpdateUserViewModel vm;

        /// <summary>
        /// Add User
        /// </summary>
        public AddOrUpdateUserWindow()
        {
            vm = new AddOrUpdateUserViewModel();
            InitializeComponent();
            AddBlock.Visibility = Visibility.Visible;
            DataContext = vm;
            vm.NotifyView += OnNotifyView;
        }

        // Update User
        public AddOrUpdateUserWindow(User user)
        {
            vm = new AddOrUpdateUserViewModel(user);
            InitializeComponent();
            WorkerNoTextBox.IsEnabled = false;
            UpdateBlock.Visibility = Visibility.Visible;
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
                default:
                    break;
            }
            return null;
        }

        private void ReadCardNoButtonClick(object sender, RoutedEventArgs args)
        {
            int cardNo = CardUtil.GetCardUid();
            if (cardNo <= 0)
            {
                MessageWindow.Show("卡信息读取失败, 错误码:" + cardNo);
                return;
            }
            vm.User.CardNo = CardUtil.GetCardUid();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UnbindingCardNoButtonClick(object sender, RoutedEventArgs e)
        {
            vm.User.CardNo = 0;
        }
    }
}
