using System;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        public RelayCommand LoginCommand { get; set; }

        private string no;
        public string No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public LoginViewModel()
        {
            No = LocalConfigManager.GetAppSettingValue("Login.No");
            Password = LocalConfigManager.GetAppSettingValue("Login.Password");

            InitCommands();
        }

        private void InitCommands()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        public void Login()
        {
            LoginRequest request = new LoginRequest(Convert.ToInt32(no), password);
            ResponseData<User> resp = request.Request<ResponseData<User>>();
            if (resp != null && resp.IsSuccess)
            {
                OnNotifyView(ViewModelMessage.Close, resp.Data);
            }
            else if (resp == null)
            {
                MessageWindow.Show("操作失败");
            }
            else
            {
                MessageWindow.Show("登录失败:" + resp.Message);
            }
        }

        private bool CanLogin(object _)
        {
            return !No.IsNullOrEmptyOrWhiteSpace()
                && int.TryParse(No, out int i) && i > 0
                && !Password.IsNullOrEmptyOrWhiteSpace();
        }
    }
}
