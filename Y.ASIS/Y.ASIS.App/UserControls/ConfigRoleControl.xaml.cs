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
    /// ConfigRoleControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigRoleControl : UserControl
    {
        public ConfigRoleControl()
        {
            InitializeComponent();
        }

        private void AddRoleButtonClick(object sender, RoutedEventArgs e)
        {
            ConfigViewModel vm = DataContext as ConfigViewModel;
            AddOrUpdateRoleWindow window = new AddOrUpdateRoleWindow(vm.Functions)
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "新增角色"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                vm.RefreshRoles(true);
            }
        }

        private void UpdateRoleButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }
            ConfigViewModel vm = DataContext as ConfigViewModel;
            Role clone = (item.DataContext as Role).JsonDeepCopy();
            AddOrUpdateRoleWindow window = new AddOrUpdateRoleWindow(vm.Functions, clone)
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "编辑角色"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                vm.RefreshRoles(true);
                if (AppGlobal.Instance.GetData("MainViewModel") is MainViewModel mainVm && mainVm.CurrentRole.Id == clone.Id)
                {
                    // mainVm.RefreshCurrentRole();
                }
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
                Role clone = (item.DataContext as Role).JsonDeepCopy();
                AddOrUpdateRoleWindow window = new AddOrUpdateRoleWindow(vm.Functions, clone)
                {
                    Owner = VisualTreeUtil.GetParent<Window>(this),
                    Title = "编辑角色"
                };
                bool? result = window.ShowDialog();
                if ((bool)result)
                {
                    vm.RefreshRoles();
                    if (AppGlobal.Instance.GetData("MainViewModel") is MainViewModel mainVm && mainVm.CurrentRole.Id == clone.Id)
                    {
                        // mainVm.RefreshCurrentRole();
                    }
                }
            }
        }

        private void DeleteRoleButtonClick(object sender, RoutedEventArgs e)
        {
            ConfigViewModel vm = DataContext as ConfigViewModel;

            IEnumerable<int> ids = vm.Roles.Where(i => i.IsChecked).Select(i => i.Id);

            if (!ids.Any())
            {
                return;
            }

            MessageBoxResult result = MessageWindow.Show("确定删除所选内容？", "提示", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }

            DeleteRolesRequest request = new DeleteRolesRequest(ids);
            ResponseData<object> resp = request.Request<ResponseData<object>>();

            if (resp != null && resp.IsSuccess)
            {
                vm.RefreshRoles();
            }
            else
            {
                MessageWindow.Show("操作失败");
            }
        }
    }
}
