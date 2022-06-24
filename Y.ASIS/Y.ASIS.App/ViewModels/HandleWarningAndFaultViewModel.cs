using System.Collections.Generic;
using System.Collections.ObjectModel;
using Y.ASIS.App.Common;
using Y.ASIS.App.Communication.Api;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.ViewModels
{
    class HandleWarningAndFaultViewModel : ViewModelBase
    {
        private ObservableCollection<FaultMessages> warnings;
        public ObservableCollection<FaultMessages> Warnings
        {
            get { return warnings; }
            set { SetProperty(ref warnings, value); }
        }

        private FaultMessages current;
        public FaultMessages Current
        {
            get { return current; }
            private set
            {
                SetProperty(ref current, value);
            }
        }

        public RelayCommand HandleWarningCommand { get; set; }

        public HandleWarningAndFaultViewModel(IEnumerable<FaultMessages> warnings, FaultMessages current)
        {
            Warnings = new ObservableCollection<FaultMessages>(warnings);
            Current = current;
            InitCommands();
        }

        private void InitCommands()
        {
            HandleWarningCommand = new RelayCommand(HandleWarning);
        }

        private void HandleWarning()
        {
            if (Current == null)
            {
                return;
            }

            if (!(AppGlobal.Instance.GetData("MainViewModel") is MainViewModel vm)
                || vm.CurrentUser == null)
            {
                return;
            }

            HandleWarningRequest request = new HandleWarningRequest(
                Current.Id,
                vm.CurrentUser.No,
                Current.Remarks);
            ResponseData<object> resp = request.Request<ResponseData<object>>();
            if (resp != null && resp.IsSuccess)
            {
                vm.RefreshUnhandleWarningsCount();
                OnNotifyView(ViewModelMessage.Close);
            }
            else if (resp != null)
            {
                MessageWindow.Show("操作失败: " + resp.Message);
            }
            else
            {
                MessageWindow.Show("操作失败");
            }
        }
    }
}
