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
    /// AddOrUpdateUserGroupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateUserGroupWindow : PopupWindow
    {
        private readonly AddOrUpdateUserGroupViewModel vm;

        public AddOrUpdateUserGroupWindow()
        {
            vm = new AddOrUpdateUserGroupViewModel();
            InitializeComponent();
            DataContext = vm;
            vm.NotifyView += OnNotifyView;
        }

        public AddOrUpdateUserGroupWindow(UserGroup userGroup)
        {
            vm = new AddOrUpdateUserGroupViewModel(userGroup);
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
                default:
                    break;
            }
            return null;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
