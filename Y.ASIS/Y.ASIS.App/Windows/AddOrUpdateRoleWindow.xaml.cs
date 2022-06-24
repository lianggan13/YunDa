using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// AddOrUpdateRoleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateRoleWindow : PopupWindow
    {
        private readonly AddOrUpdateRoleViewModel vm;

        public AddOrUpdateRoleWindow(ObservableCollection<Function> functions)
        {
            vm = new AddOrUpdateRoleViewModel(functions);
            InitializeComponent();
            DataContext = vm;
            vm.NotifyView += OnNotifyView;
        }

        public AddOrUpdateRoleWindow(ObservableCollection<Function> functions, Role role)
        {
            vm = new AddOrUpdateRoleViewModel(functions, role);
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

        private void AllSelectCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            vm.Functions.ForEach(i => i.IsChecked = true);
        }

        private void AllSelectCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            vm.Functions.ForEach(i => i.IsChecked = false);
        }
    }
}
