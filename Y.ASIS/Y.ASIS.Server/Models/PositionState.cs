using System.Collections.Generic;
using System.Collections.ObjectModel;
using Y.ASIS.Common.Utils;

namespace Y.ASIS.Server.Models
{
    public class PositionState
    {
        public PositionState(int positionId)
        {
            PositionId = positionId;
            Trains.CollectionChanged += (s, e) =>
            {
                UpdateLastTime();
            };
        }

        public int PositionId { get; set; }

        private bool connected;
        /// <summary>
        /// 通信状态
        /// </summary>
        public bool Connected
        {
            get { return connected; }
            set { SetProperty(ref connected, value); }
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

        private int isolation;
        /// <summary>
        /// 隔离开关状态
        /// </summary>
        public int Isolation
        {
            get { return isolation; }
            set { SetProperty(ref isolation, value); }
        }

        private IReadOnlyList<int> grounding = new List<int>();
        /// <summary>
        /// 接地开关状态
        /// </summary>
        public IReadOnlyList<int> Grounding
        {
            get { return grounding; }
            set { SetProperty(ref grounding, value); }
        }

        private int elecState;
        /// <summary>
        /// 验电装置状态
        /// </summary>
        public int ElecState
        {
            get { return elecState; }
            set { SetProperty(ref elecState, value); }
        }

        private int elecResult;
        /// <summary>
        /// 验电结果
        /// </summary>
        public int ElecResult
        {
            get { return elecResult; }
            set { SetProperty(ref elecResult, value); }
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

        private List<Platform> platforms = new List<Platform>();
        /// <summary>
        /// 平台信息
        /// </summary>
        public List<Platform> Platforms
        {
            get { return platforms; }
            set { SetProperty(ref platforms, value); }
        }

        private ObservableCollection<Train> trains = new ObservableCollection<Train>();
        /// <summary>
        /// 车位状态
        /// </summary>
        public ObservableCollection<Train> Trains
        {
            get { return trains; }
            set { SetProperty(ref trains, value); }
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

        private IReadOnlyList<double> loopR = new List<double>();
        /// <summary>
        /// 回路电阻值
        /// </summary>
        public IReadOnlyList<double> LoopR
        {
            get { return loopR; }
            set { SetProperty(ref loopR, value); }
        }

        private IReadOnlyList<int> signalLight;
        /// <summary>
        /// 信号灯状态
        /// </summary>
        public IReadOnlyList<int> SignalLight
        {
            get { return signalLight; }
            set { SetProperty(ref signalLight, value); }
        }

        private int tool;
        /// <summary>
        /// 工具领用数量
        /// </summary>
        public int Tool
        {
            get { return tool; }
            set { SetProperty(ref tool, value); }
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

        private IReadOnlyList<int> gate = new List<int>();
        /// <summary>
        /// 道闸机
        /// </summary>
        public IReadOnlyList<int> Gate
        {
            get { return gate; }
            set { SetProperty(ref gate, value); }
        }

        private IReadOnlyList<int> depotGate = new List<int>();
        /// <summary>
        /// 库门
        /// </summary>
        public IReadOnlyList<int> DepotGate
        {
            get { return depotGate; }
            set { SetProperty(ref depotGate, value); }
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
        /// 操作指令
        /// </summary>
        public int Command
        {
            get { return command; }
            set { SetProperty(ref command, value); }
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

        private int safeConfirm;
        /// <summary>
        /// 安全确认状态
        /// </summary>
        public int SafeConfirm
        {
            get { return safeConfirm; }
            set
            {
                safeConfirm = value;
            }
        }

        private int algorithmResult;
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


        private IReadOnlyList<int> signalLightCommand = new List<int>();
        /// <summary>
        /// 信号灯指令
        /// </summary>
        public IReadOnlyList<int> SignalLightCommand
        {
            get { return signalLightCommand; }
            set { SetProperty(ref signalLightCommand, value); }
        }

        private IReadOnlyList<int> gateCommand = new List<int>();
        public IReadOnlyList<int> GateCommand
        {
            get { return gateCommand; }
            set { SetProperty(ref gateCommand, value); }
        }

        /// <summary>
        /// 上一次状态更新的时间
        /// </summary>
        public long LastTime { get; private set; }

        public void UpdateLastTime()
        {
            LastTime = TimeUtil.TimeStamp();
        }

        private void SetProperty<T>(ref T item, T value)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                UpdateLastTime();
            }
        }
    }
}
