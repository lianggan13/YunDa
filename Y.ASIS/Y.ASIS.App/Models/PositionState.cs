using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Y.ASIS.App.Services;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 列位状态
    /// </summary>
    public class PositionState : NotifyObjectBase
    {
        public PositionState(Position position)
        {
            Position = position;
        }


        public int PositionId { get; set; }

        public Position Position { get; private set; }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set
            {
                if (connected != value && Position != null)
                {
                    connected = value;
                    OnPropertyChanged("Connected");

                    if (connected)
                    {
                        Position.AddSafeOptionMessage(TryFindResource("Position_Connection_True") as string);
                    }
                    else
                    {
                        Position.AddDangerousOptionMessage(TryFindResource("Position_Connection_False") as string);
                    }
                }
            }
        }

        private int systemState;
        /// <summary>
        /// 系统状态
        /// </summary>
        public int SystemState
        {
            get { return systemState; }
            set { SetProperty(ref systemState, value); }
        }

        Random random = new Random();
        private int isolation;

        /// <summary>
        /// 隔离开关状态
        /// 01=合闸位置
        /// 02=分闸位置
        /// 03=合闸中
        /// 04=分闸中
        /// </summary>
        public int Isolation
        {
            get { return isolation; }
            set
            {
                if (isolation != value)
                {
                    isolation = value;
                    OnPropertyChanged(nameof(Isolation));
                }
            }
        }

        private ObservableCollection<int> grounding;
        /// <summary>
        /// 接地开关状态
        /// </summary>
        public ObservableCollection<int> Grounding
        {
            get { return grounding; }
            set
            {
                if (grounding == null || value == null || grounding.Count != value.Count)
                {
                    grounding = value;
                    OnPropertyChanged(nameof(Grounding));
                    if (grounding != null && Position != null)
                    {
                        for (int i = 0; i < grounding.Count; i++)
                        {
                            string key;
                            string message;
                            switch (grounding[i])
                            {
                                case 0:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_0";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddInfoOptionMessage(message);
                                    break;
                                case 1:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_1";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddSafeOptionMessage(message);
                                    break;
                                case 2:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_2";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddDangerousOptionMessage(message);
                                    break;
                                case 3:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_3";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddInfoOptionMessage(message);
                                    break;
                                case 4:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_4";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddInfoOptionMessage(message);
                                    break;
                                case 10:
                                    key = $"PositionOperation_Grounding_{grounding.Count}_10";
                                    message = string.Format(TryFindResource(key) as string, i + 1);
                                    Position.AddDangerousOptionMessage(message);
                                    break;
                            }
                        }
                    }
                }
                else if (!grounding.SequenceEqual(value))
                {
                    for (int i = 0; i < grounding.Count; i++)
                    {
                        if (grounding[i] == value[i])
                        {
                            continue;
                        }
                        grounding[i] = value[i];
                        if (Position == null)
                        {
                            continue;
                        }
                        string key;
                        string message;
                        switch (grounding[i])
                        {
                            case 0:
                                key = $"PositionOperation_Grounding_{grounding.Count}_0";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddInfoOptionMessage(message);
                                break;
                            case 1:
                                key = $"PositionOperation_Grounding_{grounding.Count}_1";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddSafeOptionMessage(message);
                                break;
                            case 2:
                                key = $"PositionOperation_Grounding_{grounding.Count}_2";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddDangerousOptionMessage(message);
                                break;
                            case 3:
                                key = $"PositionOperation_Grounding_{grounding.Count}_3";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddInfoOptionMessage(message);
                                break;
                            case 4:
                                key = $"PositionOperation_Grounding_{grounding.Count}_4";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddInfoOptionMessage(message);
                                break;
                            case 10:
                                key = $"PositionOperation_Grounding_{grounding.Count}_10";
                                message = string.Format(TryFindResource(key) as string, i + 1);
                                Position.AddDangerousOptionMessage(message);
                                break;
                        }
                    }

                    OnPropertyChanged("Grounding");
                }
            }
        }



        private int elecState;
        /// <summary>
        /// 验电装置状态
        /// </summary>
        public int ElecState
        {
            get { return elecState; }
            set
            {
                if (elecState != value)
                {
                    elecState = value;


                    OnPropertyChanged(nameof(ElecState));
                }
            }
        }

        private int elecResult;
        /// <summary>
        /// 验电结果
        /// 01=验电有电
        /// 02=验电无电
        /// </summary>
        public int ElecResult
        {
            get { return elecResult; }
            set
            {
                if (elecResult != value)
                {
                    elecResult = value;
                    OnPropertyChanged("ElecResult");

                    if (Position == null)
                    {
                        return;
                    }

                    switch (elecResult)
                    {
                        case 1:
                            Position.AddDangerousOptionMessage(TryFindResource("PositionOperation_ElecResult_1") as string);
                            break;
                        case 2:
                            Position.AddSafeOptionMessage(TryFindResource("PositionOperation_ElecResult_2") as string);
                            break;
                    }
                }
            }
        }


        private double elecValue;
        /// <summary>
        /// 验电装置读数
        /// </summary>
        public double ElecValue
        {
            get { return elecValue; }
            set { SetProperty(ref elecValue, value); }
        }

        private ObservableCollection<Platform> platforms;
        /// <summary>
        /// 平台状态
        /// </summary>
        public ObservableCollection<Platform> Platforms
        {
            get { return platforms; }
            set
            {
                if (platforms == null || value == null
                        || !JsonConvert.SerializeObject(value).Equals(JsonConvert.SerializeObject(platforms)))
                //|| !platforms.SequenceEqual(value)
                {

                    if (platforms == null)
                    {
                        platforms = new ObservableCollection<Platform>();
                    }

                    for (int i = 0; i < value?.Count; i++)
                    {
                        var v = value.ElementAtOrDefault(i);
                        var p = platforms.ElementAtOrDefault(i);

                        if (v != null && p != null)
                        {
                            if (!v.Doors.SequenceEqual(p.Doors))
                            {
                                // Doors Changed
                                PositionService.Messenger.NotifyColleagues(
                                    PositionService.PlatformDoorsChanged,
                                    new { Position, DoorIndex = i, DoorStates = v.Doors });
                            }

                            platforms[i] = v; // update platform
                        }
                        else
                        {
                            if (platforms.Count < i + 1)
                            {
                                platforms.Add(v);  // add platform
                            }
                            else
                            {
                                platforms[i] = v; // update platform
                            }
                        }
                    }

                    //if (platforms.Count > value.Count)
                    //{
                    //    for (int i = value.Count; i < platforms.Count; i++)
                    //    {
                    //        platforms.RemoveAt(i);
                    //    }
                    //}

                    if (platforms.Count > 0)
                    {
                        for (int i = 0; i < platforms.Count; i++)
                        {
                            var p = platforms.ElementAtOrDefault(i);
                            if (p != null)
                            {
                                p.Index = i + 1;
                            }

                        }
                    }
                    OnPropertyChanged(nameof(Platforms));
                }
            }
        }

        private ObservableCollection<Train> trains;
        public ObservableCollection<Train> Trains
        {
            get { return trains; }
            set
            {
                if (trains == null || value == null
                    || !JsonConvert.SerializeObject(value).Equals(JsonConvert.SerializeObject(trains)))
                {
                    if (trains == null)
                    {
                        trains = new ObservableCollection<Train>();
                    }

                    for (int i = 0; i < value?.Count; i++)
                    {
                        var v = value.ElementAtOrDefault(i);
                        var t = trains.ElementAtOrDefault(i);

                        if (v != null && t != null)
                        {
                            if (string.IsNullOrEmpty(t.No))
                            {
                                t.No = v.No;
                            }
                            t.LeftPantograph = v.LeftPantograph;
                            t.RightPantograph = v.RightPantograph;
                            t.State = v.State;

                            //trains[i] = value[i]; // update train
                        }
                        else
                        {
                            if (trains.Count < i + 1)
                            {
                                trains.Add(v); // add train
                            }
                            else
                            {
                                trains[i] = v; // update train
                            }
                        }
                    }

                    OnPropertyChanged(nameof(Trains));
                }

                #region old code
                //if (value != null)
                //{
                //    for (int i = 0; i < value?.Count; i++)
                //    {
                //        var v = value.ElementAt(i);
                //        var t = trains?.ElementAtOrDefault(i);
                //        if (t == null || v == null)
                //            continue;

                //        // 有车 --> 异常： 01 --> 03
                //        if (t.State == "1" && v.State == "3")
                //        {
                //            trains[i] = null;
                //        }

                //        // 异常 --> 无车： 03 --> 02
                //        if (t.State == "3" && v.State == "1")
                //        {
                //            trains[i] = null;
                //        }
                //    }

                //    trains = new ObservableCollection<Train>(value?.ToList());   // 来自 Server 的 value,对本地赋值时，进行深拷贝
                //    OnPropertyChanged(nameof(Trains));
                //}
                #endregion
            }
        }

        private double groundingR;
        /// <summary>
        /// 接地电阻值
        /// </summary>
        public double GroundingR
        {
            get { return groundingR; }
            set { SetProperty(ref groundingR, value); }
        }

        private ObservableCollection<double> loopR;
        /// <summary>
        /// 回路电阻值
        /// </summary>
        public ObservableCollection<double> LoopR
        {
            get { return loopR; }
            set
            {
                if (loopR == null || value == null || !loopR.SequenceEqual(value))
                {
                    loopR = value;
                    OnPropertyChanged("LoopR");
                }
            }
        }

        private ObservableCollection<int> signalLight;
        /// <summary>
        /// 信号灯
        /// </summary>
        public ObservableCollection<int> SignalLight
        {
            get { return signalLight; }
            set
            {
                if (signalLight == null || value == null || !signalLight.SequenceEqual(value))
                {
                    signalLight = value;
                    OnPropertyChanged("SignalLight");
                }
            }
        }

        private int tool;
        /// <summary>
        /// 工具领用情况
        /// </summary>
        public int Tool
        {
            get { return tool; }
            set
            {
                switch (AppGlobal.Instance.Project)
                {
                    case ProjectType.NationalRailway:
                        break;
                    case ProjectType.CityRailway_1:
                        break;
                    case ProjectType.CityRailway_2:
                        value = -9999;
                        break;
                    case ProjectType.Shenzhen12:
                        break;
                }

                SetProperty(ref tool, value);
            }
        }

        private int warning;
        /// <summary>
        /// 警示灯状态
        /// </summary>
        public int Warning
        {
            get { return warning; }
            set { SetProperty(ref warning, value); }
        }

        private ObservableCollection<int> gate;
        /// <summary>
        /// 道闸机
        /// </summary>
        public ObservableCollection<int> Gate
        {
            get { return gate; }
            set
            {
                if (gate == null || value == null || !gate.SequenceEqual(value))
                {
                    gate = value;
                    OnPropertyChanged("Gate");
                }
            }
        }

        private ObservableCollection<int> depotGate;
        /// <summary>
        /// 库门
        /// </summary>
        public ObservableCollection<int> DepotGate
        {
            get { return depotGate; }
            set
            {
                if (depotGate == null || value == null || !depotGate.SequenceEqual(value))
                {
                    depotGate = value;
                    OnPropertyChanged("DepotGate");
                }
            }
        }

        private int mode;
        /// <summary>
        /// 操作模式
        /// </summary>
        public int Mode
        {
            get { return mode; }
            set { SetProperty(ref mode, value); }
        }

        private int flow;
        /// <summary>
        /// 当前所处流程
        /// </summary>
        public int Flow
        {
            get { return flow; }
            set { SetProperty(ref flow, value); }
        }

        private int command;
        /// <summary>
        /// 系统指令
        /// </summary>
        public int Command
        {
            get { return command; }
            set
            {
                if (command != value)
                {
                    SetProperty(ref command, value);
                }
            }
        }

        private int apply;
        /// <summary>
        /// 控制申请
        /// </summary>
        public int Apply
        {
            get { return apply; }
            set { SetProperty(ref apply, value); }
        }

        private int enableReset;
        /// <summary>
        /// 设备对位状态(人数清零状态)
        /// </summary>
        public int EnableReset
        {
            get { return enableReset; }
            set { SetProperty(ref enableReset, value); }
        }

        private int safeConfirm = 0;
        /// <summary>
        /// 安全确认
        /// </summary>
        public int SafeConfirm
        {
            get { return safeConfirm; }
            set
            {
                if (safeConfirm != value)
                {
                    safeConfirm = value;
                    OnPropertyChanged(nameof(SafeConfirm));
                }
            }
        }

        private int algorithmResult = 0;
        /// <summary>
        /// 算法确认
        /// </summary>
        public int AlgorithmResult
        {
            get { return algorithmResult; }
            set
            {
                if (algorithmResult != value)
                {
                    algorithmResult = value;
                    OnPropertyChanged(nameof(AlgorithmResult));
                }
            }
        }

        private ObservableCollection<int> signalLightCommand;
        /// <summary>
        /// 信号灯指令
        /// </summary>
        public ObservableCollection<int> SignalLightCommand
        {
            get { return signalLightCommand; }
            set
            {
                if (signalLightCommand == null || value == null || !signalLightCommand.SequenceEqual(value))
                {
                    signalLightCommand = value;
                    OnPropertyChanged("SignalLightCommand");
                }
            }
        }

        private ObservableCollection<int> gateCommand;
        public ObservableCollection<int> GateCommand
        {
            get { return gateCommand; }
            set
            {
                if (gateCommand == null || value == null || !gateCommand.SequenceEqual(value))
                {
                    gateCommand = value;
                    OnPropertyChanged("GateCommand");
                }
            }
        }

        public long LastTime { get; set; }

        [JsonIgnore]
        public DateTime LocalLastTime { get; set; } = DateTime.Now;
    }


    /// <summary>
    /// 列位状态
    /// </summary>
    public class PositionStateNet
    {
        public int PositionId { get; set; }

        public bool Connected { get; set; }

        private int systemState;
        /// <summary>
        /// 系统状态
        /// </summary>
        public int SystemState { get; set; }

        private int isolation;
        /// <summary>
        /// 隔离开关状态
        /// </summary>
        public int Isolation { get; set; }

        private ObservableCollection<int> grounding;
        /// <summary>
        /// 接地开关状态
        /// </summary>
        public ObservableCollection<int> Grounding
        {
            get { return grounding; }
            set
            {
                grounding = value;
            }
        }

        private int elecState;
        /// <summary>
        /// 验电装置状态
        /// </summary>
        public int ElecState
        {
            get { return elecState; }
            set
            {
                elecState = value;
            }
        }

        private int elecResult;
        /// <summary>
        /// 验电结果
        /// </summary>
        public int ElecResult
        {
            get { return elecResult; }
            set
            {
                elecResult = value;
            }
        }


        private double elecValue;
        /// <summary>
        /// 验电装置读数
        /// </summary>
        public double ElecValue
        {
            get { return elecValue; }
            set { elecValue = value; }
        }


        private ObservableCollection<Platform> platforms;
        /// <summary>
        /// 平台状态
        /// </summary>
        public ObservableCollection<Platform> Platforms
        {
            get { return platforms; }
            set
            {
                platforms = value;
            }
        }

        //private List<Platform> platforms;
        ///// <summary>
        ///// 平台状态
        ///// </summary>
        //public List<Platform> Platforms
        //{
        //    get { return platforms; }
        //    set
        //    {
        //        platforms = value;
        //    }
        //}


        private ObservableCollection<Train> trains; //= new ObservableCollection<Train>();
        public ObservableCollection<Train> Trains
        {
            get { return trains; }
            set
            {
                trains = value;
            }
        }

        private double groundingR;
        /// <summary>
        /// 接地电阻值
        /// </summary>
        public double GroundingR
        {
            get { return groundingR; }
            set { groundingR = value; }
        }

        private ObservableCollection<double> loopR;
        /// <summary>
        /// 回路电阻值
        /// </summary>
        public ObservableCollection<double> LoopR
        {
            get { return loopR; }
            set
            {
                loopR = value;
            }
        }

        private ObservableCollection<int> signalLight;
        /// <summary>
        /// 信号灯
        /// </summary>
        public ObservableCollection<int> SignalLight
        {
            get { return signalLight; }
            set
            {
                signalLight = value;
            }
        }

        private int tool;
        /// <summary>
        /// 工具领用情况
        /// </summary>
        public int Tool
        {
            get { return tool; }
            set
            {
                tool = value;
            }
        }

        private int warning;
        /// <summary>
        /// 警示灯状态
        /// </summary>
        public int Warning
        {
            get { return warning; }
            set { warning = value; }
        }

        private ObservableCollection<int> gate;
        /// <summary>
        /// 道闸机
        /// </summary>
        public ObservableCollection<int> Gate
        {
            get { return gate; }
            set
            {
                gate = value;
            }
        }

        private ObservableCollection<int> depotGate;
        /// <summary>
        /// 库门
        /// </summary>
        public ObservableCollection<int> DepotGate
        {
            get { return depotGate; }
            set
            {
                depotGate = value;
            }
        }

        private int mode;
        /// <summary>
        /// 操作模式
        /// </summary>
        public int Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private int flow;
        /// <summary>
        /// 当前所处流程
        /// </summary>
        public int Flow
        {
            get { return flow; }
            set { flow = value; }
        }

        private int command;
        /// <summary>
        /// 系统指令
        /// </summary>
        public int Command
        {
            get { return command; }
            set
            {
                command = value;
            }
        }

        private int apply;
        /// <summary>
        /// 控制申请
        /// </summary>
        public int Apply
        {
            get { return apply; }
            set { apply = value; }
        }

        private int enableReset;
        /// <summary>
        /// 设备对位状态(人数清零状态)
        /// </summary>
        public int EnableReset
        {
            get { return enableReset; }
            set { enableReset = value; }
        }

        private int safeConfirm = -1;
        /// <summary>
        /// 安全确认状态编号
        /// </summary>
        public int SafeConfirm
        {
            get { return safeConfirm; }
            set
            {
                safeConfirm = value;
            }
        }

        private int algorithmResult = 0;
        /// <summary>
        /// 算法结果状态编号
        /// </summary>
        public int AlgorithmResult
        {
            get { return algorithmResult; }
            set
            {
                algorithmResult = value;
            }
        }

        private ObservableCollection<int> signalLightCommand;
        /// <summary>
        /// 信号灯指令
        /// </summary>
        public ObservableCollection<int> SignalLightCommand
        {
            get { return signalLightCommand; }
            set
            {
                signalLightCommand = value;
            }
        }

        private ObservableCollection<int> gateCommand;
        public ObservableCollection<int> GateCommand
        {
            get { return gateCommand; }
            set
            {
                gateCommand = value;
            }
        }

        public long LastTime { get; set; }
    }
}
