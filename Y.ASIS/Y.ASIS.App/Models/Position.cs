using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Y.ASIS.App.Common;
using Y.ASIS.App.Services;
using Y.ASIS.App.Services.CameraService;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Models.Enums;


namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 列位
    /// </summary>
    public class Position : EnumerableObject
    {
        private int id;
        /// <summary>
        /// 获取或设置编号
        /// </summary>
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private int index;
        /// <summary>
        /// 获取或设置股道号
        /// </summary>
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        private string name;
        /// <summary>
        /// 获取或设置股道名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private ObservableCollection<VideoStream> videos;
        /// <summary>
        /// 视频
        /// </summary>
        public ObservableCollection<VideoStream> Videos
        {
            get { return videos; }
            set { SetProperty(ref videos, value); }
        }

        /// <summary>
        /// 额外的视频
        /// </summary>
        public List<VideoStream> ExtraVideos { get; set; }

        private List<int> speakerIds;
        public List<int> SpeakerIds
        {
            get { return speakerIds; }
            set { SetProperty(ref speakerIds, value); }
        }


        private SafeConfirm safeConfirm;
        [JsonIgnore]
        public SafeConfirm SafeConfirm
        {
            get { return safeConfirm; }
            set
            {
                if (safeConfirm != value)
                {
                    safeConfirm = value; // 来自 SafeConfirmConfig.json
                    OnPropertyChanged(nameof(SafeConfirm));
                }
            }
        }

        private PositionState state;
        [JsonIgnore]
        public PositionState State
        {
            get { return state; }
            set
            {
                state = value;
            }
        }

        public void SetPosState(PositionStateNet value)
        {
            // 减少界面刷新的频率
            bool needNotify = false;
            if (state == null)
            {
                state = new PositionState(this);
                needNotify = true;
            }

            if (value == null)
            {
                state = null;
                OnPropertyChanged(nameof(State));
            }
            else
            {
                state.PropertyChanged -= State_PropertyChanged;
                state.PropertyChanged += State_PropertyChanged;

                state.PositionId = value.PositionId;

                state.SystemState = value.SystemState;
                state.Connected = value.Connected;
                state.Isolation = value.Isolation;  // 隔离 开关状态
                state.Grounding = value.Grounding;  // 接地 开关状态
                state.ElecState = value.ElecState;  // 验电 开关状态
                state.ElecResult = value.ElecResult;
                state.ElecValue = value.ElecValue;

                //Tester.TestPlatform(this, value);
                state.Platforms = value.Platforms;

                //Tester.TestTrain(this, value);
                state.Trains = value.Trains;        // 列车

                state.LoopR = value.LoopR;
                state.GroundingR = value.GroundingR;
                state.SignalLight = value.SignalLight;
                state.Tool = value.Tool;
                state.Warning = value.Warning;
                state.Gate = value.Gate;
                state.DepotGate = value.DepotGate;
                state.Mode = value.Mode;
                state.Flow = value.Flow;
                state.Command = value.Command;
                state.Apply = value.Apply;
                state.EnableReset = value.EnableReset;

                if (AppGlobal.Instance.HasLogin()) // && AppGlobal.Instance.MainVM.CurrentPosition != null)
                {
                    state.SafeConfirm = value.SafeConfirm;
                    state.AlgorithmResult = value.AlgorithmResult;
                }

                state.SignalLightCommand = value.SignalLightCommand;
                state.GateCommand = value.GateCommand;
                state.LastTime = value.LastTime;
                state.LocalLastTime = DateTime.Now;

                state.PropertyChanged -= State_PropertyChanged;
                if (needNotify)
                {
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        private void State_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(state.Isolation):

                    switch (state.Isolation)
                    {
                        case 0:
                            AddInfoOptionMessage(TryFindResource("PositionOperation_Isolation_0") as string);
                            break;
                        case 1:
                            AddDangerousOptionMessage(TryFindResource("PositionOperation_Isolation_1") as string);
                            break;
                        case 2:
                            AddSafeOptionMessage(TryFindResource("PositionOperation_Isolation_2") as string);
                            break;
                        case 3:
                            AddInfoOptionMessage(TryFindResource("PositionOperation_Isolation_3") as string);
                            break;
                        case 4:
                            AddInfoOptionMessage(TryFindResource("PositionOperation_Isolation_4") as string);
                            break;
                        case 10:
                            AddDangerousOptionMessage(TryFindResource("PositionOperation_Isolation_10") as string);
                            break;
                    }

                    HIKNVRService.LinkVideo(VideoType.Isolation, state.Isolation);

                    break;
                case nameof(state.Grounding):
                    if (state.Grounding == null || state.Grounding.Count == 0)
                    {
                        return;
                    }

                    HIKNVRService.LinkVideo(VideoType.Grounding, state.Grounding[0]);
                    break;
                case nameof(state.ElecState):
                    HIKNVRService.LinkVideo(VideoType.Elec, state.ElecState);
                    break;
                case nameof(state.Warning):
                    int warning = state.Warning;
                    int sysstate = state.SystemState;
                    HandleWarnings(warning, sysstate);
                    break;
                case nameof(state.SafeConfirm):
                    int safeConfirm = state.SafeConfirm;
                    SetSafeConfirm(safeConfirm);
                    break;
                case nameof(state.AlgorithmResult):
                    int algorithmResult = state.AlgorithmResult;
                    SetAlgorithmConfirm(algorithmResult);
                    break;
                default:
                    return; // 直接返回
            }
        }

        [JsonIgnore]
        public ObservableCollection<Message> OptionMessages { get; } = new ObservableCollection<Message>();

        private bool CheckCondition(int no)
        {
            var sc = SafeConfirmManager.Instance.GetSafeConfirm(no); // data comes from SafeConfirmConfig.json
            if (sc != null)
            {
                SafeConfirm = sc;

                PositionService.ApplyVideoCondition(this, sc);
            }

            return sc != null;
        }

        public void SetSafeConfirm(int safeConfirmNo)
        {
            // No: 1|2|3|4|5|6|7|8|9|101|102|103|104|105|106|107|108|109
            if (CheckCondition(safeConfirmNo))
            {
                AddInfoOptionMessage(SafeConfirm.Name + " 前安全确认...");

                if (AppGlobal.Instance.Project == ProjectType.Shenzhen12)
                {
                    // shen12 自动确认
                    var vm = AppGlobal.Instance.MainVM;
                    var user = vm.CurrentUser;
                    SafeConfirmManager.Instance.SendSafeConfirmRequest<ResponseData<bool>>
                    (
                        posId: id,
                        no: safeConfirmNo,
                        userNo: user.No,
                        callback: resp =>
                        {
                            if (resp != null && resp.Data)
                            {
                                string msg = $"已确认 {SafeConfirm.Name}";
                                AddSafeOptionMessage(msg);
                            }
                            else
                            {
                                string msg = $"{SafeConfirm.Name} 安全确认超时失败!";
                                AddInfoOptionMessage(msg);
                            }
                        }
                    );
                }
            }
        }

        public async void SetAlgorithmConfirm(int algorithmNo)
        {
            // No: 1|2|3|4|5|6|7|8|9|101|102|103|104|105|106|107|108|109
            if (CheckCondition(algorithmNo))
            {
                AddInfoOptionMessage(SafeConfirm.Name + " 前安全算法识别...");

                var vm = AppGlobal.Instance.MainVM;
                var user = vm.CurrentUser;
                var tasks = PositionService.UseVideoCondition(this);
                var ss = await Task.WhenAll(tasks);
                bool pass = ss?.All(s => s != null) == true;

                string msg = "";
                if (pass == true)
                {
                    pass = SafeConfirm.Conditions.Any(c => c.IsSafe == true);
                    msg = $"{SafeConfirm.Name}+  算法识别" + (pass == true ? "通过" : "不通过！");
                }
                else
                {
                    msg = $"{SafeConfirm.Name}+  算法识别失败!";
                }

                // TEST
                pass = true;
                msg = $"{SafeConfirm.Name}+  算法通过(测试)!";

                SafeConfirmManager.Instance.SendAlgorithmConfirmRequest<ResponseData<bool>>
                (
                    posId: id,
                    no: algorithmNo,
                    userNo: user.No,
                    pass: pass,
                    msg: msg,
                    callback: resp =>
                    {
                        //if (resp != null && resp.Data)
                        //{
                        //    string msg = $"已确认 可执行 {SafeConfirm.Name} 操作";
                        //    AddSafeOptionMessage(msg);
                        //}
                        //else
                        //{
                        //    string msg = $"{SafeConfirm.Name} 安全确认超时失败!";
                        //    AddInfoOptionMessage(msg);
                        //}
                    });
            }
        }

        private void HandleWarnings(int warningCode, int systemStateCode)
        {
            if (warningCode == 1)
            {

                switch (systemStateCode)
                {
                    case 2:
                        AddWarningOptionMessage($"操作台分闸警示...");
                        break;
                    case 6:
                        AddWarningOptionMessage($"操作台撤离警示...");
                        break;
                    case 9:
                        AddWarningOptionMessage($"操作台合闸警示...");
                        break;
                    default:
                        break;
                }

            }
        }

        private void AddOptionMessage(Message message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (OptionMessages.Count == 1000)
                {
                    OptionMessages.RemoveAt(0);
                }
                OptionMessages.Add(message);

            });
        }

        public void AddOrdinaryOptionMessage(string content)
        {
            Message message = new Message(MessageType.Ordinary, content);
            AddOptionMessage(message);
        }

        public void AddInfoOptionMessage(string content)
        {
            Message message = new Message(MessageType.Info, content);
            AddOptionMessage(message);

        }

        public void AddWarningOptionMessage(string content)
        {
            Message message = new Message(MessageType.Warning, content);
            AddOptionMessage(message);
        }

        public void AddSafeOptionMessage(string content)
        {
            Message message = new Message(MessageType.Safe, content);
            AddOptionMessage(message);
        }

        public void AddDangerousOptionMessage(string content)
        {
            Message message = new Message(MessageType.Dangerous, content);
            AddOptionMessage(message);
        }


    }


}
