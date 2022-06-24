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
using Y.ASIS.App.ViewModels;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : PopupWindow
    {
        private readonly ConfigViewModel vm;

        public ConfigWindow()
        {
            vm = new ConfigViewModel();
            InitializeComponent();
            DataContext = vm;
        }

        private TabItem lastItem;

        private void OnTabItemSelectionChanged(object sender, RoutedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            TabItem item = tab.SelectedItem as TabItem;
            if (lastItem == item)
            {
                return;
            }
            lastItem = item;
            string method = item.Tag as string;
            vm.InvokeMethod(method);
        }
    }
}
