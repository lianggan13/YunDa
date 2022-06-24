using System.Collections.ObjectModel;
using System.Linq;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class AddOrUpdateRoleViewModel : ViewModelBase
    {
        private ObservableCollection<Function> functions;
        public ObservableCollection<Function> Functions
        {
            get { return functions; }
            set { SetProperty(ref functions, value); }
        }

        private Role role;
        public Role Role
        {
            get { return role; }
            set { SetProperty(ref role, value); }
        }

        public RelayCommand AddOrUpdateRoleCommand { get; set; }

        public AddOrUpdateRoleViewModel(ObservableCollection<Function> functions, Role role)
        {
            Role = role;
            Functions = functions;
            Functions.ForEach(i => i.IsChecked = role.Functions.Contains(i.Id));
            Init();
        }

        public AddOrUpdateRoleViewModel(ObservableCollection<Function> functions)
        {
            Role = new Role()
            {
                Functions = new ObservableCollection<int>()
            };
            Functions = functions;
            Functions.ForEach(i => i.IsChecked = false);
            Init();
        }

        private void Init()
        {
            AddOrUpdateRoleCommand = new RelayCommand(AddOrUpdateRole, CanAddOrUpdateRole);
        }

        private void AddOrUpdateRole()
        {
            Role.Functions = new ObservableCollection<int>(Functions.Where(i => i.IsChecked).Select(i => i.Id));
            AddOrUpdateRoleRequest request = new AddOrUpdateRoleRequest(Role);
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

        private bool CanAddOrUpdateRole(object _)
        {
            return !Role.Name.IsNullOrEmptyOrWhiteSpace()
                && !Role.Describe.IsNullOrEmptyOrWhiteSpace();
        }
    }
}
