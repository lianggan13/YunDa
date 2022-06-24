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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.ViewModels;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// ConfigTitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigTitleControl : UserControl
    {
        public ConfigTitleControl()
        {
            InitializeComponent();
        }

        private void UpdateTitleButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }
            Title clone = (item.DataContext as Title).JsonDeepCopy();
            UpdateTitleWindow window = new UpdateTitleWindow(clone)
            {
                Owner = VisualTreeUtil.GetParent<Window>(this)
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                ConfigViewModel vm = DataContext as ConfigViewModel;
                vm.RefreshTitles();
            }
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }
            Point relative = e.GetPosition(item);
            if (relative.X >= 0 && relative.X <= item.ActualWidth
                && relative.Y >= 0 && relative.Y <= item.ActualHeight)
            {
                Title clone = (item.DataContext as Title).JsonDeepCopy();
                UpdateTitleWindow window = new UpdateTitleWindow(clone)
                {
                    Owner = VisualTreeUtil.GetParent<Window>(this)
                };
                bool? result = window.ShowDialog();
                if ((bool)result)
                {
                    ConfigViewModel vm = DataContext as ConfigViewModel;
                    vm.RefreshTitles();
                }
            }
        }
    }
}
