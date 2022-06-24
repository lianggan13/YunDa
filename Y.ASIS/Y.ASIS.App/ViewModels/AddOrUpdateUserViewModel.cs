using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class AddOrUpdateUserViewModel : ViewModelBase
    {
        private bool isUpdate;

        private User user;
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private string no;
        public string No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private ObservableCollection<UserGroup> groups;
        public ObservableCollection<UserGroup> Groups
        {
            get { return groups; }
            set { SetProperty(ref groups, value); }
        }

        private ObservableCollection<Role> roles;
        public ObservableCollection<Role> Roles
        {
            get { return roles; }
            set { SetProperty(ref roles, value); }
        }

        public RelayCommand AddOrUpdateUserCommand { get; set; }

        public AddOrUpdateUserViewModel()
        {
            User = new User();
            No = user.No.ToString();
            Init();
        }

        public AddOrUpdateUserViewModel(User user)
        {
            User = user;
            No = user.No.ToString();
            isUpdate = true;
            Init();
        }

        private void AddOrUpdateUser()
        {
            if (User.Name.Length > 50)
            {
                MessageWindow.Show("用户名称过长, 请修改后重试");
                return;
            }
            User.No = Convert.ToInt32(No);
            User.AllowUpdate = true;
            AddOrUpdateUserRequest request = new AddOrUpdateUserRequest(User);
            ResponseData<object> resp = request.Request<ResponseData<object>>();
            if (resp != null && resp.IsSuccess)
            {
                OnNotifyView(ViewModelMessage.Close);
            }
            else if (resp != null)
            {
                MessageWindow.Show("操作失败, " + resp.Message);
            }
            else
            {
                MessageWindow.Show("操作失败");
            }
        }

        private bool CanAddOrUpdateUser(object _)
        {
            bool con1 = User.RoleId != 0 && User.UserGroupId != 0;
            bool con2 = !User.Name.IsNullOrEmptyOrWhiteSpace() && int.TryParse(No, out int i) && i > 0;
            bool con3 = false;
            if (!isUpdate)
            {
                // 新增
                con3 = !User.NewPassword.IsNullOrEmptyOrWhiteSpace() && User.NewPassword == User.ConfirmPassword;
            }
            else
            {
                // 修改
                con3 = (!User.OldPassword.IsNullOrEmptyOrWhiteSpace() && !User.NewPassword.IsNullOrEmptyOrWhiteSpace() && User.NewPassword == User.ConfirmPassword)
                    || (User.OldPassword.IsNullOrEmptyOrWhiteSpace() && User.NewPassword.IsNullOrEmptyOrWhiteSpace() && User.NewPassword == User.ConfirmPassword)
                    || (!User.OldPassword.IsNullOrEmptyOrWhiteSpace());

            }

            return con1 && con2 && con3;
        }

        private void Init()
        {
            GetGroups();
            GetRoles();
            AddOrUpdateUserCommand = new RelayCommand(AddOrUpdateUser, CanAddOrUpdateUser);
        }

        private void GetGroups()
        {

            UserGroupListRequest request = new UserGroupListRequest();
            ResponseData<IEnumerable<UserGroup>> resp = request.Request<ResponseData<IEnumerable<UserGroup>>>();
            if (resp != null && resp.IsSuccess && resp.Data != null)
            {
                Groups = new ObservableCollection<UserGroup>(resp.Data);
            }
        }

        private void GetRoles()
        {
            RoleListRequest request = new RoleListRequest();
            ResponseData<IEnumerable<Role>> resp = request.Request<ResponseData<IEnumerable<Role>>>();
            if (resp != null && resp.IsSuccess && resp.Data != null)
            {
                Roles = new ObservableCollection<Role>(resp.Data);
            }
        }
    }
}
