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
    /// UserListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigUserControl : UserControl
    {
        public ConfigUserControl()
        {
            InitializeComponent();
        }

        private void TakeOrViewPhotoButtonClick(object sender, RoutedEventArgs e)
        {
            ListViewItem item = VisualTreeUtil.GetParent<ListViewItem>(sender as DependencyObject);
            if (item != null)
            {
                User user = item.DataContext as User;
                ViewUserPhotoWindow window = new ViewUserPhotoWindow(user.Id, user.PhotoUrl)
                {
                    Owner = VisualTreeUtil.GetParent<Window>(this)
                };
                bool? result = window.ShowDialog();
                if ((bool)result)
                {
                    ConfigViewModel vm = DataContext as ConfigViewModel;
                    vm.RefreshUsers();
                }
            }
        }

        private void AddUserButtonClick(object sender, RoutedEventArgs e)
        {
            AddOrUpdateUserWindow window = new AddOrUpdateUserWindow()
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "新增用户"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                ConfigViewModel vm = DataContext as ConfigViewModel;
                vm.RefreshUsers(true);
            }
        }

        private void UpdateUserButtonClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<ListViewItem> items = VisualTreeUtil.GetChild<ListViewItem>(ListViewBlock);
            ListViewItem item = items.FirstOrDefault(i => i.IsSelected);
            if (item == null)
            {
                return;
            }

            UpdateUser(item);
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
                UpdateUser(item);
            }
        }


        private void UpdateUser(ListViewItem item)
        {
            User user = item.DataContext as User;
            if (!user.AllowUpdate)
            {
                MessageWindow.Show("人员已授权，禁止该操作", "提示");
                return;
            }

            User clone = (item.DataContext as User).JsonDeepCopy();
            if (AppGlobal.Instance.MainVM.CurrentUser.RoleId == 1) // Admin
            {
                clone.OldPassword = clone.Password;
            }

            AddOrUpdateUserWindow window = new AddOrUpdateUserWindow(clone)
            {
                Owner = VisualTreeUtil.GetParent<Window>(this),
                Title = "编辑用户"
            };
            bool? result = window.ShowDialog();
            if ((bool)result)
            {
                ConfigViewModel vm = DataContext as ConfigViewModel;
                vm.RefreshUsers(true);
                if (AppGlobal.Instance.GetData("MainViewModel") is MainViewModel mainVm && mainVm.CurrentUser.Id == clone.Id)
                {
                    mainVm.RefreshCurrentUser();
                }
            }
        }


        private void DeleteUserButtonClick(object sender, RoutedEventArgs e)
        {
            ConfigViewModel vm = DataContext as ConfigViewModel;

            var users = vm.Users.Where(i => i.IsChecked).ToList();

            if (users.Any(u => u.Id == AppGlobal.Instance.MainVM.CurrentUser.Id))
            {
                MessageWindow.Show("人员已登录，禁止该操作", "提示");
                return;
            }

            if (users.Any(u => u.RoleId == 1))
            {
                MessageWindow.Show("人员是管理员，禁止该操作", "提示");
                return;
            }

            if (users.Any(u => !u.AllowUpdate))
            {
                MessageWindow.Show("人员已授权，禁止该操作", "提示");
                return;
            }

            var ids = vm.Users.Where(i => i.IsChecked).Select(i => i.Id).ToList();

            if (!ids.Any())
            {
                return;
            }


            MessageBoxResult result = MessageWindow.Show("确定删除所选内容？", "提示", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }

            DeleteUsersRequest request = new DeleteUsersRequest(ids);
            ResponseData<object> resp = request.Request<ResponseData<object>>();

            if (resp != null && resp.IsSuccess)
            {
                vm.RefreshUsers();
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
