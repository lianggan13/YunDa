using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;


namespace Y.ASIS.App.ViewModels
{
    public class SafeConfirmViewModel : ViewModelBase
    {
        private Position position;
        public Position Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        private bool manualConfirm;
        public bool ManualConfirm
        {
            get { return manualConfirm; }
            set { SetProperty(ref manualConfirm, value); }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private Dictionary<string, BitmapImage> photos = new Dictionary<string, BitmapImage>();

        public RelayCommand PositionConfirmCommand => new RelayCommand(PositionConfirm, CanPositionConfirm);
        public RelayCommand LookUpPhotoCommand => new RelayCommand(LookUpPhoto, (_) => true);

        public SafeConfirmViewModel()
        {

        }

        public SafeConfirmViewModel(Position position, User user) : this()
        {
            FillData(position, user);
        }

        public void FillData(Position position, User user)
        {
            Position = position;
            CurrentUser = user;
        }

        private void LookUpPhoto(object param)
        {
            var condition = param as SafeCondition;

            if (photos.TryGetValue(condition.Text, out BitmapImage img))
            {
                if (img == null)
                {
                    MessageWindow.Show("获取图片失败！");
                    return;
                }

                ViewDetectPhotoWindow window = new ViewDetectPhotoWindow(condition.Text, img)
                {
                    Owner = Application.Current.MainWindow
                };
                window.ShowDialog();
            }
            else
                MessageWindow.Show("获取图片失败！");
        }


        private bool CanPositionConfirm(object _)
        {
            return ManualConfirm
                || (Position != null && Position.SafeConfirm != null && Position.SafeConfirm.Conditions.All(i => i.IsSafe != null && (bool)i.IsSafe));
        }

        public void PositionConfirm()
        {
            SafeConfirmManager.Instance.SendSafeConfirmRequest<ResponseData<bool>>
            (
                posId: position.Id,
                no: position.State.SafeConfirm,
                userNo: currentUser.No,
                callback: resp => RequestCallback(resp)
            );
        }

        public async void StartAlgorithmTask()
        {
            IsLoading = true;

            var tasks = PositionService.UseVideoCondition(position, photos);
            var ss = await Task.WhenAll(tasks);

            if (ss.Count() > 0 && ss.All(s => s != null))
            {
                // detect all success...
            }
            else if (ss.Count() < 0 || ss.All(s => s == null))
            {
                PopShowMsg("图片识别失败！");
            }
            else
            {
                PopShowMsg("部分图片识别失败！");
            }

            IsLoading = false;
        }

        public void RequestCallback(ResponseData<bool> resp)
        {
            AppDispatcherInvoker(() =>
            {
                if (resp != null && resp.Data)
                {
                    string msg = $"已确认 {position.SafeConfirm.Name}";
                    position.AddSafeOptionMessage(msg);
                }
                else
                {
                    string msg = $"{position?.SafeConfirm?.Name} 安全确认超时失败!";
                    position.AddInfoOptionMessage(msg);
                    PopShowMsg(msg);
                }

                OnNotifyView(ViewModelMessage.Close);
            });
        }

        private void PopShowMsg(string msg)
        {
            var win = WindowManager.FindWindwByType(typeof(SafeConfirmWindow));
            if (win != null)
            {
                MessageWindow.Show(msg); // 弹窗提示 
            }
        }
    }
}
