using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Utils;
using Y.ASIS.App.ViewModels;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.UserControls
{
    /// <summary>
    /// UserGroupControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigUserGroupControl : UserControl
    {
        public ConfigUserGroupControl()
        {
            InitializeComponent();
        }

        private void AddUserGroupButtonClick(object sender, RoutedEventArgs e)
        {
            AddOrUpdateUserGroupWindow window = new AddOrUpdateUserGroupWindow()
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "新增班组"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                ConfigViewModel vm = DataContext as ConfigViewModel;
                vm.RefreshUserGroups();
            }
        }

        private void UpdateUserGroupButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }
            ConfigViewModel vm = DataContext as ConfigViewModel;
            UserGroup clone = (item.DataContext as UserGroup).JsonDeepCopy();
            AddOrUpdateUserGroupWindow window = new AddOrUpdateUserGroupWindow(clone)
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "编辑班组"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {

                vm.RefreshUserGroups(true);
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
                ConfigViewModel vm = DataContext as ConfigViewModel;
                UserGroup clone = (item.DataContext as UserGroup).JsonDeepCopy();
                AddOrUpdateUserGroupWindow window = new AddOrUpdateUserGroupWindow(clone)
                {
                    Owner = VisualTreeUtil.GetParent<Window>(this),
                    Title = "编辑班组"
                };
                bool? result = window.ShowDialog();
                if ((bool)result)
                {

                    vm.RefreshUserGroups(true);
                }
            }
        }

        private void DeleteUserGroupButtonClick(object sender, RoutedEventArgs e)
        {
            ConfigViewModel vm = DataContext as ConfigViewModel;

            IEnumerable<int> ids = vm.Groups.Where(i => i.IsChecked).Select(i => i.Id);

            if (!ids.Any())
            {
                return;
            }

            MessageBoxResult result = MessageWindow.Show("确定删除所选内容？", "提示", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }

            DeleteUserGroupsRequest request = new DeleteUserGroupsRequest(ids);
            ResponseData<object> resp = request.Request<ResponseData<object>>();

            if (resp != null && resp.IsSuccess)
            {
                vm.RefreshUserGroups(true);
            }
            else
            {
                MessageWindow.Show("操作失败");
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                ListViewItem item = VisualTreeUtil.GetParent<ListViewItem>(chk);
                item.IsSelected = true;
            }
        }
    }
}
