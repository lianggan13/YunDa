using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel vm;

        public User User { get; private set; }

        public LoginWindow()
        {
            vm = new LoginViewModel();
            InitializeComponent();
            DataContext = vm;
            vm.NotifyView += OnNotifyView;
        }

        private object OnNotifyView(ViewModelMessage type, params object[] args)
        {
            switch (type)
            {
                case ViewModelMessage.Close:
                    User = args[0] as User;
                    User.OldPassword = vm.Password;
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

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                DialogResult = false;
            }
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            vm.NotifyView -= OnNotifyView;
            base.OnClosed(e);
        }
    }
}
