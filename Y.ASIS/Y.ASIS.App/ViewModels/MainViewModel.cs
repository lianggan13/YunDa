using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Y.ASIS.App.Common;
using Y.ASIS.App.Communication;
using Y.ASIS.App.Communication.Algorithm;
using Y.ASIS.App.Communication.Api;
using Y.ASIS.App.Models;
using Y.ASIS.App.Properties;
using Y.ASIS.App.Services;
using Y.ASIS.App.Utility;
using Y.ASIS.App.Windows;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Common.MVVMFoundation;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string now;
        public string Now
        {
            get { return now; }
            set { SetProperty(ref now, value); }
        }

        private ObservableCollection<Track> tracks; //= new ObservableCollection<Track>();
        public ObservableCollection<Track> Tracks
        {
            get { return tracks; }
            set { SetProperty(ref tracks, value); }
        }

        private Track currentTrack;
        public Track CurrentTrack
        {
            get { return currentTrack; }
            set { SetProperty(ref currentTrack, value); }
        }

        private Position currentPosition;
        public Position CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                SetProperty(ref currentPosition, value);
            }
        }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set { SetProperty(ref connected, value); }
        }

        private bool algorithmConnected;
        public bool AlgorithmConnected
        {
            get { return algorithmConnected; }
            set { SetProperty(ref algorithmConnected, value); }
        }

        private Role currentRole;
        public Role CurrentRole
        {
            get { return currentRole; }
            set { SetProperty(ref currentRole, value); }
        }

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                SetProperty(ref currentUser, value);
                RefreshCurrentRole();
            }
        }

        private int unhandleWarningsCount;
        public int UnhandleWarningsCount
        {
            get { return unhandleWarningsCount; }
            set { SetProperty(ref unhandleWarningsCount, value); }
        }

        public RelayCommand PositionCommandCommand => new RelayCommand(PositionCommand);

        public RelayCommand PositionApplyCommand => new RelayCommand(PositionApply);

        public RelayCommand PositionResetCommand => new RelayCommand(PositionReset);


        public MainViewModel()
        {
            Design(); // just for design
        }

        public void Design()
        {
            var tracks = JsonConvert.DeserializeObject<IEnumerable<Track>>(Resources.Tracks);
            Tracks = new ObservableCollection<Track>(tracks);

            var trackStates = JsonConvert.DeserializeObject<IEnumerable<TrackState>>(Resources.TrackStates);
            UpdateTrackState(trackStates);

            //tracks.SelectMany(t => t.Positions).ForEach(t => t.State.Connected = false);

            CurrentPosition = Tracks.ElementAt(0).Positions[0];
            //CurrentUser = ...

            RefreshSysTime();
            CheckCommunication();
        }

        private void RefreshSysTime()
        {
            #region old code
            //TimerManager.Instance.AddSchedule(
            //  () => Now = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss"),
            //  TimeSpan.FromSeconds(1d)
            //);
            #endregion

            TaskHelper.LoopRun(() =>
            {
                Now = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
            }, TimeSpan.FromSeconds(1d));
        }

        private void CheckCommunication()
        {
            var poss = Tracks.SelectMany(t => t.Positions);
            TaskHelper.LoopRun(() =>
            {
                foreach (var pos in poss)
                {
                    if (DateTime.Now - pos.State?.LocalLastTime > TimeSpan.FromSeconds(5d))
                    {
                        pos.State.Connected = false;
                    }
                }

                Connected = HeartRequest.Ping();
                if (Connected && AppGlobal.Env == AppEnvironment.Development)
                {
                    AppGlobal.Env = AppEnvironment.Production;
                    Initialize();
                }

                AlgorithmConnected = AlgorithmHeartRequest.Ping();

            }, TimeSpan.FromSeconds(5d));
        }

        public void Initialize()
        {
            // tracks
            RefreshTracks();
            // unhandled warning
            RefreshUnhandleWarningsCount();

            // send project config 
            // TODO: 后期取消此种方式
            ProjectRequest request = new ProjectRequest();
            _ = request.Request<ResponseData<object>>();

            NancyService.Start(this);
            AppGlobal.Instance.SetData("MainViewModel", this);
            AppGlobal.Instance.MainVM = this;
        }



        private void RefreshTracks()
        {
            CurrentPosition = null;
            TrackListRequest request = new TrackListRequest();
            ResponseData<IEnumerable<Track>> resp = request.Request<ResponseData<IEnumerable<Track>>>();
            if (resp != null && resp.IsSuccess)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Tracks.Clear();
                });
                Tracks = new ObservableCollection<Track>(resp.Data);
            }
        }

        public void RefreshUnhandleWarningsCount()
        {
            GetUnhandleWarningsCountRequest request = new GetUnhandleWarningsCountRequest();
            request.RequestAsync<ResponseData<int>>(resp =>
            {
                if (resp != null && resp.IsSuccess)
                {
                    UnhandleWarningsCount = resp.Data;
                }
            });
        }

        private void PositionReset(object parameter)
        {
            if (CurrentPosition == null)
            {
                return;
            }
            int command = Convert.ToInt32(parameter as string);
            PositionResetRequest request = new PositionResetRequest(CurrentPosition.Id, command, currentUser.No);
            request.RequestAsync<ResponseData<bool>>(resp =>
            {

            });
        }

        private void PositionCommand(object parameter)
        {
            if (CurrentPosition == null)
            {
                return;
            }
            int command = Convert.ToInt32(parameter as string);

            #region plc信息处理部分未有传输警示部分 暂放这
            AppDispatcherInvoker(() =>
            {
                switch (command)
                {
                    case 101:
                        CurrentPosition.AddInfoOptionMessage("远程分闸警示");
                        break;
                    case 105:
                        CurrentPosition.AddInfoOptionMessage("远程撤离警示");
                        break;
                    case 108:
                        CurrentPosition.AddInfoOptionMessage("远程合闸警示");
                        break;
                    default:
                        break;
                }
            });
            #endregion

            PositionCommmandRequest request = new PositionCommmandRequest(CurrentPosition.Id, command, CurrentUser.No);
            request.RequestAsync<ResponseData<bool>>(resp =>
            {

            });
        }

        private void PositionApply(object parameter)
        {
            if (CurrentPosition == null)
            {
                return;
            }
            int apply = Convert.ToInt32(parameter as string);
            PositionApplyRequest request = new PositionApplyRequest(CurrentPosition.Id, apply, CurrentUser.No);
            request.RequestAsync<ResponseData<bool>>(resp =>
            {

            });
        }

        public void ResetPositionSelectedState()
        {
            Tracks.ForEach(i =>
            {
                i.Positions.ForEach(j =>
                {
                    j.IsSelected = false;
                });
            });
        }

        private Role GetRole(int roleId)
        {
            RoleRequest request = new RoleRequest(roleId);
            ResponseData<Role> resp = request.Request<ResponseData<Role>>();
            if (resp != null && resp.IsSuccess)
            {
                return resp.Data;
            }
            return null;
        }

        public void RefreshCurrentRole()
        {
            CurrentRole = CurrentUser == null ? null : GetRole(CurrentUser.RoleId);
        }

        public void RefreshCurrentUser()
        {
            if (CurrentUser == null)
            {
                return;
            }
            LoginRequest request = new LoginRequest(CurrentUser.No, CurrentUser.OldPassword);
            ResponseData<User> resp = request.Request<ResponseData<User>>();
            if (resp != null && resp.IsSuccess)
            {
                string password = CurrentUser.OldPassword;
                CurrentUser = resp.Data;
                CurrentUser.OldPassword = password;
            }
            else
            {
                CurrentUser = null;
                OnNotifyView(ViewModelMessage.CurrentUserUpdated);
            }
        }


        public void UpdateTrackState(IEnumerable<TrackState> trackStates)
        {
            AppDispatcherBeginInvoker(() =>
            {
                foreach (var trackState in trackStates)
                {
                    Track track = Tracks?.FirstOrDefault(t => t.Id == trackState.TrackId);
                    if (track != null)
                    {
                        trackState.PositionStates.ForEach(ps =>
                        {
                            Position position = track.Positions.FirstOrDefault(p => p.Id == ps.PositionId);
                            position?.SetPosState(ps);

                        });
                    }
                }
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        public void UpdateDevice(DeviceStateMessage dsmsg)
        {
            switch (dsmsg.State)
            {
                case DeviceState.Offline:
                    AppMessages.Instance.AddWarningMessage($"{dsmsg.Name} 离线");
                    break;
                case DeviceState.Online:
                    AppMessages.Instance.AddSafeMessage($"{dsmsg.Name} 已连接");
                    break;
                default:
                    break;
            }
        }

        public void AddFaultRecord(FaultRecord record)
        {
            AppDispatcherBeginInvoker(() =>
            {
                int code = int.Parse(record.FaultCode);

                if (!PLCFaultCodeExt.IntValues.Any(x => x == code))
                    return;

                if (!Enum.TryParse(record.FaultCode, out PLCFaultCode faultCode))
                    return;

                //string text = $"{record.TrackNo}股道-{(PLCFaultCode)code}";
                string text = $"{record.TrackNo}股道-{faultCode}";

                if (code >= 995)
                {
                    // 系统级故障
                    MessageWindow.Show(text, "报警");
                }
                else
                {
                    // 一般故障
                    AppMessages.Instance.AddDangerousMessage(text);
                }

            }, System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
