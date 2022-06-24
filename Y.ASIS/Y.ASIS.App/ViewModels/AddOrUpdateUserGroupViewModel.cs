using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class AddOrUpdateUserGroupViewModel : ViewModelBase
    {
        private UserGroup userGroup;
        public UserGroup UserGroup
        {
            get { return userGroup; }
            set { SetProperty(ref userGroup, value); }
        }

        public RelayCommand AddOrUpdateUserGroupCommand { get; set; }

        public AddOrUpdateUserGroupViewModel()
        {
            UserGroup = new UserGroup();
            Init();
        }

        public AddOrUpdateUserGroupViewModel(UserGroup userGroup)
        {
            UserGroup = userGroup;
            Init();
        }

        private void Init()
        {
            AddOrUpdateUserGroupCommand = new RelayCommand(AddOrUpdateUserGroup, CanAddOrUpdateUserGroup);
        }

        private void AddOrUpdateUserGroup()
        {
            AddOrUpdateUserGroupRequest request = new AddOrUpdateUserGroupRequest(UserGroup);
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

        private bool CanAddOrUpdateUserGroup(object _)
        {
            return !UserGroup.Name.IsNullOrEmptyOrWhiteSpace();
        }
    }
}
