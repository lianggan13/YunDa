using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Common.Utils;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Common;
using Y.ASIS.Server.Communication;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Models;
using Y.ASIS.Server.Services;
using Y.ASIS.Server.Services.CameraService;
using Y.ASIS.Server.Services.Main;

namespace Y.ASIS.Server.Device
{
    class PLC
    {
        private const byte NotConfigCode = 11;
        private readonly static Dictionary<string, PLCNode> nodes;

        static PLC()
        {
            string file = Path.Combine(ServerGlobal.ExecuteDirectory, "Config", "OpcNodes.json");
            using (StreamReader reader = File.OpenText(file))
            {
                string json = reader.ReadToEnd();
                nodes = json.JsonDeserialize<Dictionary<string, PLCNode>>();
            }
        }

        private readonly UaClient client;
        private readonly IReadOnlyList<MonitorNode> monitorNodes;

        public Position Position { get; private set; }

        public PLC(Position position)
        {
            Position = position;

            if (Position.OpcConfig.Address.IsNullOrEmptyOrWhiteSpace()
                || Position.OpcConfig.Username.IsNullOrEmptyOrWhiteSpace()
                || Position.OpcConfig.Password.IsNullOrEmptyOrWhiteSpace())
            {
                return;
            }

            // MonitorNodes <-- PLCNodes
            monitorNodes = nodes.Select(kv =>
            {
                string name = kv.Key;
                PLCNode node = kv.Value;
                PLCNodeType nodeType = (PLCNodeType)Enum.Parse(typeof(PLCNodeType), name);
                void callback(dynamic param)
                {
                    OnValueChanged(nodeType, param);
                    if (param != null && nodeType != PLCNodeType.SystemTime)
                        MainService.PushTrackState();
                }
                return new MonitorNode(nodeType, node.Identifier, node.NamespaceIndex, callback);
            }).ToList();


            client = new UaClient(Position.OpcConfig.Address, Position.OpcConfig.Username, Position.OpcConfig.Password, monitorNodes);
            client.Connected += (s, e) =>
            {
                //foreach (var m in monitorNodes)
                //{
                //    if (TryGetValue(m.NodeType, out dynamic value))
                //    {
                //        //m.Callback?.Invoke(SystemErrorValue);
                //        OnValueChanged(m.NodeType, value);
                //    }
                //}
                Position.State.Connected = true;
                LogHelper.Info($"PLC connected: {Position.OpcConfig.Address}");
            };
            client.Disconnected += (s, e) =>
            {
                Position.State.Connected = false;
                LogHelper.Warn($"PLC disconnected: {Position.OpcConfig.Address}");
            };
            client.Start();

            InitTask();
        }

        public void InitTask()
        {
            TimerManager.Instance.AddSchedule(() =>
            {
                TryWriteValue(PLCNodeType.SystemTime, (int)TimeUtil.DateTimeToTimeStamp(DateTime.Now.AddHours(8d)));
            }, TimeSpan.FromSeconds(10d));
        }

        private void OnValueChanged(PLCNodeType type, dynamic value)
        {
            if (value == null)
                return;

            switch (type)
            {
                case PLCNodeType.SystemState:
                    OnSystemStateChanged(value);
                    break;
                case PLCNodeType.SystemError:
                    OnSystemErrorStateChanged(value);
                    break;
                case PLCNodeType.IsolationState:
                    OnIsolationStateChanged(value);
                    break;
                case PLCNodeType.GroundingState:
                    OnGroundingStateChanged(value);
                    break;
                case PLCNodeType.ElecState:
                    OnElecStateChanged(value);
                    break;
                case PLCNodeType.ElecResult:
                    OnElecResultChanged(value);
                    break;
                case PLCNodeType.SignalLightState:
                    OnSignalLightStateChanged(value);
                    break;
                case PLCNodeType.ToolState:
                    OnToolStateChanged(value);
                    break;
                case PLCNodeType.GateState:
                    OnGateStateChanged(value);
                    break;
                case PLCNodeType.DepotGateState:
                    OnDepotGateStateChanged(value);
                    break;
                case PLCNodeType.DoorState:
                    OnDoorStateChanged(value);
                    break;
                case PLCNodeType.DoorLocker:
                    OnDoorLockerChanged(value);
                    break;
                case PLCNodeType.GangwayState:
                    OnGangwayStateChanged(value);
                    break;
                case PLCNodeType.TrainState:
                    OnTrainStateChanged(value);
                    break;
                case PLCNodeType.WarningState:
                    OnWarningStateChanged(value);
                    break;
                case PLCNodeType.ElecValue:
                    OnElecValueChanged(value);
                    break;
                case PLCNodeType.GroundingResistance:
                    OnGroundingResistanceChanged(value);
                    break;
                case PLCNodeType.LoopResistance:
                    OnLoopResistanceChanged(value);
                    break;
                case PLCNodeType.OperationRecord:
                    OnOperationRecordChanged(value);
                    break;
                case PLCNodeType.FaultRecordState:
                    OnFaultRecordStateChanged(value);
                    break;
                case PLCNodeType.AccessRecordState:
                    OnAccessRecordStateChanged(value);
                    break;
                case PLCNodeType.TopPersonInfo:
                    OnTopPersonInfoChanged(value);
                    break;
                case PLCNodeType.PassCount:
                    OnPassCountChanged(value);
                    break;
                case PLCNodeType.OperatorPersonInfo:
                    OnOperatorPersonInfoChanged(value);
                    break;
                case PLCNodeType.OperationMode:
                    OnOperationModeChanged(value);
                    break;
                case PLCNodeType.OperationFlow:
                    OnOperationFlowChanged(value);
                    break;
                case PLCNodeType.OperationCommand:
                    OnOperationCommandChanged(value);
                    break;
                case PLCNodeType.OperationApply:
                    OnOperationApplyChanged(value);
                    break;
                case PLCNodeType.FaultRecord:
                    OnFaultRecordChange(value);
                    break;
                case PLCNodeType.EnableReset:
                    OnEnableResetChanged(value);
                    break;
                case PLCNodeType.SafeConfirm:
                    OnSafeConfirmChanged(value);
                    break;
                case PLCNodeType.AlgorithmResult:
                    OnAlgorithmResultChanged(value);
                    break;
                //case PLCNodeType.AlgorithmMsg:
                //    OnAlgorithmMsgChanged(value);
                //    break;
                case PLCNodeType.SignalLightCommand:
                    OnSignalLightCommandChanged(value);
                    break;
                case PLCNodeType.GateCommand:
                    OnGateCommandChanged(value);
                    break;
                default:
                    break;
            }

        }

        private void OnSystemStateChanged(int inputvalue)
        {
            Position.State.SystemState = inputvalue;
        }

        #region === Command ===
        public IEnumerable<Worker> GetIssuedOperators()
        {
            List<Worker> operators = new List<Worker>();
            if (TryGetValue(PLCNodeType.Operators, out string[] values))
            {
                var users = DataProvider.Instance.UserList;
                values.ForEach(i =>
                {
                    if (!i.IsNullOrEmptyOrWhiteSpace())
                    {
                        string[] array = i.Split('#');
                        int no = Convert.ToInt32(array[0]);
                        User user = users.FirstOrDefault(j => j.No == no);
                        Worker _operator = new Worker()
                        {
                            No = no,
                            Name = user != null ? user.Name : "Unknown",
                            UserGroupId = user != null ? user.UserGroupId : 0
                        };
                        operators.Add(_operator);
                    }
                });
            }
            return operators;
        }

        public IEnumerable<Worker> GetIssuedWorkers()
        {
            List<Worker> workers = new List<Worker>();
            if (TryGetValue(PLCNodeType.Workers, out string[] values))
            {
                var users = DataProvider.Instance.UserList;
                values.ForEach(i =>
                {
                    if (!i.IsNullOrEmptyOrWhiteSpace())
                    {
                        string[] array = i.Split('#');
                        int no = Convert.ToInt32(array[0]);
                        User user = users.FirstOrDefault(j => j.No == no);
                        Worker worker = new Worker()
                        {
                            No = no,
                            Name = user != null ? user.Name : "Unknown",
                            UserGroupId = user != null ? user.UserGroupId : 0
                        };
                        workers.Add(worker);
                    }
                });
            }
            return workers;
        }

        public bool IssueUsers(ISet<User> workers, ISet<User> operators, bool? isInspect)
        {
            bool success = true;
            if (isInspect == true)
            {
                workers = new HashSet<User>(operators); //  操作人员 copy to 作业人员
                success = TryWriteValue(PLCNodeType.ID_Inspect, $"{operators.ElementAt(0).No}");
            }

            if (success)
            {
                bool success1 = IssueUsers(workers, PLCNodeType.Workers);
                bool success2 = IssueUsers(operators, PLCNodeType.Operators);
                success = success1 && success2;
            }

            return success;
        }

        public bool IssueUsers(ISet<User> users, PLCNodeType type)
        {
            if (users == null || !users.Any())
            {
                return true;
            }

            if (type != PLCNodeType.Workers
                && type != PLCNodeType.Operators)
            {
                return false;
            }

            foreach (User user in users)
            {
                if (user.PhotoUrl.IsNullOrEmptyOrWhiteSpace())
                {
                    return false;
                }
            }

            string format = "{0}#{1}##Url={2}";

            if (users != null && users.Any())
            {
                bool success = TryGetValue(type, out string[] values);
                if (!success)
                {
                    return false;
                }
                List<User> list = users.ToList();
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    int no = Convert.ToInt32(values[i].Split('#')[0]);
                    User user = list.FirstOrDefault(j => j.No == no);
                    if (user == null)
                    {
                        continue;
                    }
                    string card = user.CardNo == null ? "000000" : user.CardNo.ToString();
                    values[i] = string.Format(format, user.No, card, user.PhotoUrl);
                    list.Remove(user);
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (!values[i].IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    User user = list.FirstOrDefault();
                    if (user == null)
                    {
                        break;
                    }
                    string card = user.CardNo == null ? "000000" : user.CardNo.ToString();
                    values[i] = string.Format(format, user.No, card, user.PhotoUrl);
                    list.Remove(user);
                }

                return TryWriteValue(type, values);
            }
            return false;
        }

        public bool RevokeUsers(ISet<int> workers, ISet<int> operators)
        {
            return RevokeUsers(workers, PLCNodeType.Workers)
                && RevokeUsers(operators, PLCNodeType.Operators);
        }

        public bool RevokeUsers(ISet<int> nos, PLCNodeType type)
        {
            if (nos == null || !nos.Any())
            {
                return true;
            }
            if (type != PLCNodeType.Workers
                && type != PLCNodeType.Operators)
            {
                return false;
            }
            if (nos != null && nos.Any())
            {
                bool success = TryGetValue(type, out string[] values);
                if (!success)
                {
                    return false;
                }
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    int no = Convert.ToInt32(values[i].Split('#')[0]);

                    if (nos.Contains(no))
                    {
                        values[i] = "";
                    }
                }

                int emptyIndex = -1;
                for (int i = 0; i < values.Length; i++)
                {
                    if (emptyIndex == -1 && values[i].IsNullOrEmptyOrWhiteSpace())
                    {
                        emptyIndex = i;
                        continue;
                    }
                    if (emptyIndex != -1 && !values[i].IsNullOrEmptyOrWhiteSpace())
                    {
                        values[emptyIndex] = values[i];
                        values[i] = "";
                        i = emptyIndex;
                        emptyIndex = -1;
                    }
                }
                return TryWriteValue(type, values);
            }
            return false;
        }

        public bool SetCommand(int command)
        {
            if (command > 100
                && command < 110
                && command % 100 == Position.State.Command)
            {
                return TryWriteValue(PLCNodeType.OperationCommand, (ushort)command);
            }
            return false;
        }

        public bool SetApply(int apply)
        {
            if (((apply > 100 && apply < 103) || (apply > 200 && apply < 203))
                && apply % 100 == Position.State.Apply)
            {
                return TryWriteValue(PLCNodeType.OperationApply, (ushort)apply);
            }
            return false;
        }

        public bool SetReset(int command)
        {
            if (Position.State.EnableReset != 0
                && command == 100 + Position.State.EnableReset)
            {
                return TryWriteValue(PLCNodeType.EnableReset, (ushort)command);
            }
            return false;
        }

        public bool SafeConfirm(int command)
        {
            if (Position.State.SafeConfirm != 0
                && command == 100 + Position.State.SafeConfirm)
            {
                return TryWriteValue(PLCNodeType.SafeConfirm, (ushort)command);
            }
            return false;
        }

        public bool SetAlgorithmResult(int command, string msg)
        {
            if (Position.State.AlgorithmResult != 0
                && command == 100 + Position.State.AlgorithmResult || command == 200 + Position.State.AlgorithmResult)
            {
                bool success1 = TryWriteValue(PLCNodeType.AlgorithmResult, (ushort)command);
                bool success2 = TryWriteValue(PLCNodeType.AlgorithmMsg, msg);
                return success1 && success2;
            }
            return false;
        }


        public bool SetSignalLightCommand(int index, int command)
        {
            if (index >= 0
                && index < 6
                && (command == 101 || command == 102))
            {
                bool success = TryGetValue(PLCNodeType.SignalLightCommand, out ushort[] commands);
                if (!success)
                {
                    return false;
                }
                if (command % 100 != commands[index])
                {
                    return false;
                }
                commands[index] = (ushort)command;
                return TryWriteValue(PLCNodeType.SignalLightCommand, commands);
            }
            return false;
        }

        public bool SetGateCommand(int index, int command)
        {
            if (index >= 0
                && index < 6
                && (command == 101 || command == 102))
            {
                bool success = TryGetValue(PLCNodeType.GateCommand, out ushort[] commands);
                if (!success)
                {
                    return false;
                }
                if (command % 100 != commands[index])
                {
                    return false;
                }
                commands[index] = (ushort)command;
                return TryWriteValue(PLCNodeType.GateCommand, commands);
            }
            return false;
        }
        #endregion

        #region === Handle ==

        private void OnSystemErrorStateChanged(dynamic inputvalue)
        {
            byte value = (byte)inputvalue;

            if (value == 0)
            {
                return;
            }
            Exception exception = new Exception("Format error:" + value);

            var track = DataProvider.Instance.GetTrackByPosId(Position.ID);

            for (int i = 0; i < 8; i++)
            {
                if (BitUtil.GetBit(value, i))
                {
                    FaultRecord record = new FaultRecord()
                    {
                        Index = Position.Index,
                        TrackNo = track.No
                    };
                    try
                    {
                        record.Time = DateTime.Now;
                        record.FaultCode = (999 - i).ToString();
                        if (int.Parse(record.FaultCode) == (int)PLCFaultCode.门锁异常开启错误)
                        {
                            string text = $"，{track.No}股道门禁非法闯入，请尽快撤离，";
                            SpeakerManager.Instance.Start(Position.SpeakerIds, text);
                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, ex);
                    }

                    DataProvider.Instance.AddFaultRecord(record);
                    PushMessage message = new PushMessage(PushDataType.Fault, record);
                    PushTaskService.Instance.Push(message, 10);
                }

            }
        }

        private void OnIsolationStateChanged(byte value)
        {
            Position.State.Isolation = value;
        }

        private void OnGroundingStateChanged(byte[] states)
        {
            Position.State.Grounding = states?.Where(i => i != NotConfigCode).Select(i => (int)i).ToList();
        }

        private void OnElecStateChanged(byte[] states)
        {
            Position.State.ElecState = states[0];
        }

        private void OnElecResultChanged(byte value)
        {
            Position.State.ElecResult = value;
        }

        private void OnSignalLightStateChanged(byte[] states)
        {
            if (states == null)
                return;

            Position.State.SignalLight = states.Select(i => (int)i).ToList();
            if (Position.State.SignalLight.Count == 6)
            {
                if (Position.State.SignalLight[0] == 1 || Position.State.SignalLight[5] == 1)
                {
                    //SpeakerManager.Instance.WareHouse(Position.SpeakerIds, Position.Index.ToString());
                }

                if (Position.State.SignalLight[1] == 1 || Position.State.SignalLight[4] == 1)
                {
                    //SpeakerManager.Instance.ExWareHouse(Position.SpeakerIds, Position.Index.ToString());
                }
            }
        }

        private void OnToolStateChanged(byte value)
        {
            //  Position.State.Tool = value;
        }

        private void OnGateStateChanged(byte[] states)
        {
            Position.State.Gate = states.Select(i => (int)i).ToList();
        }

        private void OnDepotGateStateChanged(byte[] states)
        {
            Position.State.DepotGate = states.Select(i => (int)i).ToList();
        }

        private void OnDoorStateChanged(byte[] states)
        {
            states = states.Where(i => i != NotConfigCode).ToArray();
            bool success = TryGetValue(PLCNodeType.DoorIndex, out byte[] indexes);
            if (!success)
            {
                return;
            }
            indexes = indexes.Where(i => i != NotConfigCode).ToArray();

            // check DoorIndexes & Platforms
            if (Position.State.Platforms.Count > indexes.Length)
            {
                Position.State.Platforms.RemoveRange(indexes.Length, Position.State.Platforms.Count - indexes.Length);
            }

            success = TryGetValue(PLCNodeType.DoorLocker, out byte[] lockers);
            if (!success)
            {
                return;
            }

            // create Platforms by DoorIndexes (maybe just once)
            if (!Position.State.Platforms.Any())
            {
                success = TryGetValue(PLCNodeType.GangwayState, out byte[] gangway);
                gangway = gangway.Where(i => i != NotConfigCode).ToArray();
                if (!success)
                {
                    return;
                }

                success = TryGetValue(PLCNodeType.TopPersonInfo, out string[] persons);
                if (!success)
                {
                    return;
                }

                for (int i = 0; i < indexes.Length; i++)
                {
                    Position.State.Platforms.Add(new Platform());
                }

                for (int i = 0; i < gangway.Length; i++)
                {
                    int index = indexes[i];
                    Position.State.Platforms[index].Gangway = gangway[i];
                }
                Position.State.UpdateLastTime();
                return;
            }

            Dictionary<int, List<int>> pairs = GetPlatformDoorState(states, lockers, indexes);
            pairs.ForEach(pair =>
            {
                Position.State.Platforms[pair.Key].Doors = pair.Value;
            });
            Position.State.UpdateLastTime();
        }

        private void OnDoorLockerChanged(byte[] lockers)
        {
            if (!Position.State.Platforms.Any())
            {
                return;
            }
            lockers = lockers.Where(i => i != NotConfigCode).ToArray();
            bool success = TryGetValue(PLCNodeType.DoorIndex, out byte[] indexes);
            if (!success)
            {
                return;
            }
            success = TryGetValue(PLCNodeType.DoorState, out byte[] states);
            states = states.Where(i => i != NotConfigCode).ToArray();
            if (!success)
            {
                return;
            }
            Dictionary<int, List<int>> pairs = GetPlatformDoorState(states, lockers, indexes);
            pairs.ForEach(pair =>
            {
                Position.State.Platforms[pair.Key].Doors = pair.Value;

                HIKNVRService.LinkDoorVideo(Position, pair.Key, pair.Value);
            });
            Position.State.UpdateLastTime();
        }

        private Dictionary<int, List<int>> GetPlatformDoorState(byte[] states, byte[] lockers, byte[] indexes)
        {
            Dictionary<int, List<int>> pairs = new Dictionary<int, List<int>>();
            for (int i = 0; i < states.Length; i++)
            {
                int index = indexes[i];
                if (!pairs.ContainsKey(index))
                {
                    pairs[i] = new List<int>();
                }

                // 00=未知、通信失败等异常状态 01=使能状态下开 02=使能状态下关 03=未使能状态下开 04=未使能状态下关
                int code = states[i] * lockers[i];
                if (code > 0)
                {
                    if (states[i] == 1)
                    {
                        if (lockers[i] == 1)
                        {
                            code = 1; // 使能状态下开
                        }
                        else if (lockers[i] == 2)
                        {
                            code = 2; // 使能状态下关
                        }
                    }
                    else if (states[i] == 2)
                    {
                        if (lockers[i] == 1)
                        {
                            code = 3; // 未使能状态下开
                        }
                        else if (lockers[i] == 2)
                        {
                            code = 4; // 未使能状态下关
                        }
                    }
                }

                try
                {
                    pairs[i].Add(code);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }

            return pairs;
        }

        private void OnGangwayStateChanged(byte[] states)
        {
            if (!Position.State.Platforms.Any())
            {
                return;
            }
            states = states.Where(i => i != NotConfigCode).ToArray();
            bool success = TryGetValue(PLCNodeType.DoorIndex, out byte[] indexes);
            if (!success)
            {
                return;
            }
            for (int i = 0; i < states.Length; i++)
            {
                int index = indexes[i];
                Position.State.Platforms[index].Gangway = states[i];
            }
            Position.State.UpdateLastTime();
        }

        private void Execute(object sender, ElapsedEventArgs e)
        {

        }


        private void OnTrainStateChanged(dynamic value)
        {
            if (value == null)
                return;
            // 00 = 初始化检测中
            // 01 = 有效1（检测到有效有车）
            // 02 = 有效0（检测到有效无车）
            // 03 = 异常（单个检测到超时）
            // 11 = 未配置

            byte[] states = (byte[])value;

            if (!Position.State.Trains.Any())
            {
                for (int i = 0; i < states.Length; i++)
                {
                    Position.State.Trains.Add(null);
                }
            }
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] == 2 && Position.State.Trains[i] != null)
                {
                    //  02 = 有效0（检测到有效无车）
                    Position.State.Trains[i] = null;
                }
                else if (states[i] != 2 && Position.State.Trains[i] == null)
                {
                    //  00 = 初始化检测中
                    //  01 = 有效1（检测到有效有车）
                    Position.State.Trains[i] = new Train();
                    Position.State.Trains[i].State = states[i].ToString();

                    Position.State.Trains[i].No = PositionService.GetTrainNo(Position, trainIndex: i);
                }
                else if (states[i] != 2 && Position.State.Trains[i] != null)
                {
                    if (states[i] == 3)
                    {
                        //  03 = 异常（单个检测到超时）
                        Position.State.Trains[i].State = states[i].ToString();
                    }
                }
                else
                {

                }

                if (Position.State.Trains[i] != null)
                {
                    Position.State.Trains[i].State = states[i].ToString();
                }

                //  11 = 未配置
            }
        }

        private void OnWarningStateChanged(dynamic value)
        {
            if (value == null)
                return;

            short sv = (short)value;
            if (sv == 1)
            {

                if (Position.State.SystemState == 2)
                {
                    Speaker.SpeakerManager.Instance.SwitchOff(Position.SpeakerIds, Position.ID.ToString());
                }
                else if (Position.State.SystemState == 6)
                {
                    Speaker.SpeakerManager.Instance.Evacuate(Position.SpeakerIds, Position.ID.ToString());
                }
                else if (Position.State.SystemState == 9)
                {
                    Speaker.SpeakerManager.Instance.SwitchOn(Position.SpeakerIds, Position.ID.ToString());
                }

            }

            Position.State.Warning = (short)value;
        }

        private void OnElecValueChanged(string value)
        {
            bool flag = double.TryParse(value, out double val);
            Position.State.ElecValue = flag ? val : -1d;
        }

        private void OnGroundingResistanceChanged(string value)
        {
            bool flag = double.TryParse(value, out double val);
            Position.State.GroundingR = flag ? val : -1d;
        }

        private void OnLoopResistanceChanged(string[] values)
        {
            if (values == null)
                return;
            Position.State.LoopR = values.Where(i => i != "Null").Select(i =>
            {
                bool flag = double.TryParse(i, out double val);
                return flag ? val : -1d;
            }).ToList();
        }

        private void OnOperationRecordChanged(string value)
        {
            if (value.IsNullOrEmptyOrWhiteSpace())
            {
                return;
            }
            string[] array = value.Split(' ');
            OperationRecord record = new OperationRecord()
            {
                Index = Position.Index
            };
            try
            {
                if (array.Length != 4)
                {
                    throw new Exception("Format error:" + value);
                }

                record.Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}");
                //record.TrackNo = DataProvider.Instance.GetTrackByPosIndex(Position.Index).No;
                record.TrackNo = DataProvider.Instance.GetTrackByPosId(Position.ID).No;

                if (array[2].Trim() == "Auto" || array[2].Trim() == "???")
                {
                    record.WorkerNo = "";
                }
                else
                {
                    record.WorkerNo = Convert.ToInt32(array[3].Trim()) >= 103 ?
                             (-1).ToString() : Convert.ToInt32(array[2].Trim()).ToString();
                }


                if (string.IsNullOrEmpty(record.WorkerNo) || record.WorkerNo == "-1")
                {
                    // 来自备用操作台的操作记录 强行设置当前登录用户 id
                    if (ServerGlobal.CurrentUser != null)
                    {
                        record.WorkerNo = ServerGlobal.CurrentUser.No.ToString();
                    }
                    else
                    {
                        return; // 直接返回
                    }
                }

                record.OperationCode = array[3].Trim();
                if (PositionService.RevokeOptCode.Any(r => r == int.Parse(record.OperationCode)))
                {
                    int posId = Position.ID;
                    var users = DataProvider.Instance.UserList.Where(u => u.PositionIds.Contains(posId));
                    PositionService.RemovePositionToUsers(posId, users?.Select(u => u.No));
                    LogHelper.Info($"完成清除操作权限:{record.OperationCode}");
                }

                DataProvider.Instance.AddOrUpdateOperationRecord(record);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            finally
            {
                TryWriteValue(PLCNodeType.OperationRecordState, (ushort)1);
            }
        }

        private void OnFaultRecordChange(string value)
        {
            if (value.IsNullOrEmptyOrWhiteSpace())
            {
                return;
            }
            string[] array = value.Split(' ');
            Exception exception = new Exception("Format error:" + value);

            FaultRecord record = new FaultRecord()
            {
                Index = Position.Index,
                TrackNo = DataProvider.Instance.GetTrackByPosId(Position.ID).No
            };
            try
            {
                if (array.Length != 4)
                {
                    throw exception;
                }
                record.Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}");
                record.DeviceNo = array[2].Trim();
                record.FaultCode = array[3].Trim();
            }
            catch
            {
                throw exception;
            }

            if (int.TryParse(record.FaultCode, out int code))
            {
                if (code == 506 || code == 526)
                {
                    int doorIndex = code == 506 ? 1 : 2;
                    string text = $"，各单位请注意，{record.TrackNo}股道{doorIndex}号门，有人尾随!";
                    SpeakerManager.Instance.Start(Position.SpeakerIds, text);
                }
            }

            if (int.Parse(record.FaultCode) < 996)
            {
                DataProvider.Instance.AddFaultRecord(record);
            }
            PushMessage message = new PushMessage(PushDataType.Fault, record);
            PushTaskService.Instance.Push(message, 10);


            TryWriteValue(PLCNodeType.FaultRecordState, (ushort)1);
        }

        private void OnOperationRecordStateChanged(dynamic value)
        {

        }

        private void OnFaultRecordStateChanged(dynamic value)
        {
            //if (value == 0)
            //{
            //    string s = GetValue<string>(PLCNodeType.FaultRecord);
            //    string[] array = s.Split(' ');
            //    Exception exception = new Exception("Format error:" + s);
            //    FaultRecord record = new FaultRecord()
            //    {
            //        TrackNo = Position.TrackNo,
            //        Index = Position.Index
            //    };
            //    try
            //    {
            //        if (array.Length != 4)
            //        {
            //            throw exception;
            //        }
            //        record.Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}");
            //        record.DeviceNo = array[2].Trim();
            //        record.FaultCode = array[3].Trim();
            //    }
            //    catch
            //    {
            //        throw exception;
            //    }
            //    Console.WriteLine(s);

            //    //DataProvider.Instance.AddFaultRecord(record);
            //    //PushMessage message = new PushMessage(Common.Models.Enums.PushDataType.Fault, record);
            //    //PushTaskManager.Instance.Push(message, 10);

            //    WriteValue(PLCNodeType.FaultRecordState, (ushort)1);
            //}
        }

        private void OnAccessRecordStateChanged(dynamic value)
        {
            //if (value == 0)
            //{
            //    string s = GetValue<string>(PLCNodeType.AccessRecord);
            //    string[] array = s.Split(' ');
            //    Exception exception = new Exception("Format error:" + s);
            //    AccessRecord record = new AccessRecord()
            //    {
            //        TrackNo = Position.TrackNo,
            //        Index = Position.Index
            //    };
            //    try
            //    {
            //        if (array.Length != 4)
            //        {
            //            throw exception;
            //        }
            //        record.Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}");
            //        record.WorkerNo = array[2].Trim();
            //        int val = int.Parse(array[3].Trim());
            //        record.DoorId = (byte)(val / 1 % 10);
            //        record.IsValid = val / 10 % 10 == 1;
            //        record.Type = (AccessRecord.AccessType)(val / 100 % 10);
            //        record.Method = (AccessRecord.AccessMethod)(val / 1000 % 10);
            //    }
            //    catch
            //    {
            //        throw exception;
            //    }
            //    Console.WriteLine(s);
            //    // DataProvider.Instance.AddAccessRecord(record);
            //    WriteValue(PLCNodeType.AccessRecordState, (ushort)1);
            //}
        }

        private void OnTopPersonInfoChanged(string[] values)
        {
            if (!Position.State.Platforms.Any())
            {
                return;
            }
            bool success = TryGetValue(PLCNodeType.DoorIndex, out byte[] indexes);
            if (!success)
            {
                return;
            }

            indexes = indexes.Where(i => i != NotConfigCode).ToArray();


            Dictionary<int, List<Worker>> workers = new Dictionary<int, List<Worker>>();

            indexes.ForEach(i =>
            {
                if (!workers.ContainsKey(i))
                {
                    workers[i] = new List<Worker>();
                }
            });

            values.ForEach(i =>
            {
                string[] array = i.Split(' ');
                if (array.Length == 4)
                {
                    int doorIndex = Convert.ToInt32(array[3].Trim());
                    int platformIndex = indexes[doorIndex];
                    Worker worker = new Worker()
                    {
                        Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}"),
                        No = Convert.ToInt32(array[2].Trim()),
                        Name = DataProvider.Instance.UserList.FirstOrDefault(j => j.No == Convert.ToInt32(array[2].Trim())).Name,
                    };
                    workers[platformIndex].Add(worker);

                    AccessRecord record = new AccessRecord()
                    {
                        WorkerNo = worker.No.ToString(),
                        DoorId = doorIndex,
                        Time = worker.Time,
                    };

                    DataProvider.Instance.AddAccessRecord(record);


                    // 更新人员记录 登顶时间
                    var userAuthRecord = DataProvider.Instance.GetUserAuthRecordsByUserNo(worker.No);
                    if (userAuthRecord != null)
                    {
                        userAuthRecord.WorkTime = worker.Time;
                        DataProvider.Instance.AddOrUpdateUserAuthRecord(userAuthRecord);
                    }
                }
            });

            workers.ForEach(pair =>
            {
                Position.State.Platforms[pair.Key].Workers = pair.Value;
            });

            Position.State.UpdateLastTime();
        }

        /// <summary>
        /// 门禁客流计统计人数::AsGlobalPV:State.Passcount[n]
        /// </summary>
        /// <param name="values">“1”、“-2”、“Error”、“Null”，负号代表出门人数大于进门人数，“Error”代表设备错误</param>
        private void OnPassCountChanged(string[] values)
        {
            if (!Position.State.Platforms.Any())
            {
                return;
            }
            bool success = TryGetValue(PLCNodeType.DoorIndex, out byte[] indexes);
            if (!success)
            {
                return;
            }

            indexes = indexes.Where(i => i != NotConfigCode).ToArray();

            Dictionary<int, string> passCounts = new Dictionary<int, string>();
            indexes.ForEach(i =>
            {
                if (!passCounts.ContainsKey(i))
                {
                    passCounts[i] = "";
                }
            });

            for (int i = 0; i < values.Length; i++)
            {
                if (!values[i].IsNullOrEmptyOrWhiteSpace())
                {
                    passCounts[i] = values[i];
                }

            }

            passCounts.ForEach(pair =>
            {
                Position.State.Platforms[pair.Key].PassCount = pair.Value;
            });

            Position.State.UpdateLastTime();
        }

        private void OnOperatorPersonInfoChanged(dynamic value)
        {
            //string s = (string)value;
            //string[] array = s.Split(' ');
            //if (array.Length == 3)
            //{
            //    Position.State.Operator = new Position.Operator()
            //    {
            //        Time = DateTime.Parse($"{array[0].Trim()} {array[1].Trim()}"),
            //        No = array[2].Trim()
            //    };
            //}
        }

        private void OnOperationFlowChanged(byte value)
        {
            Position.State.Flow = value;
        }

        private void OnOperationModeChanged(byte value)
        {
            Position.State.Mode = value;
        }

        private void OnOperationCommandChanged(ushort value)
        {
            Position.State.Command = value;
        }

        private void OnOperationApplyChanged(ushort value)
        {
            Position.State.Apply = value;
        }

        private void OnEnableResetChanged(ushort value)
        {
            Position.State.EnableReset = value;
        }

        private void OnSafeConfirmChanged(ushort value)
        {
            Position.State.SafeConfirm = value;
            // 调用算法识别
            // 发送安全确定控制命令
            //LogHelper.Debug($"PosIndex:{Position.Index}  SafeConfirm:{value}");
        }

        private void OnAlgorithmResultChanged(ushort value)
        {

            Position.State.AlgorithmResult = value;
            //Position.Videos
            // 调用算法识别
            // 发送安全确定控制命令
            //LogHelper.Debug($"PosIndex:{Position.Index}  SafeConfirm:{value}");
        }

        private void OnAlgorithmMsgChanged(dynamic value)
        {


        }

        private void OnSignalLightCommandChanged(ushort[] commands)
        {
            if (commands == null)
                return;
            Position.State.SignalLightCommand = commands.Select(i => (int)i).ToList();
        }

        private void OnGateCommandChanged(ushort[] commands)
        {
            if (commands == null)
                return;
            Position.State.GateCommand = commands.Select(i => (int)i).ToList();
        }

        #endregion

        #region === OPC Api ===
        private PLCNode GetNode(PLCNodeType type)
        {
            string key = type.ToString();
            if (!nodes.ContainsKey(key))
            {
                throw new Exception("Opc nodes file not contains key: " + key);
            }
            return nodes[key];
        }

        private bool TryGetValue<T>(PLCNodeType type, out T value)
        {
            value = default;
            PLCNode node = GetNode(type);
            if (client != null && client.IsConnected)
            {
                return client.TryGetValue(node.Identifier, node.NamespaceIndex, out value);
            }
            return false;
        }

        private bool TryWriteValue<T>(PLCNodeType node, T value)
        {
            PLCNode n = GetNode(node);
            if (client != null && client.IsConnected)
            {
                return client.TryWriteValue(n.Identifier, n.NamespaceIndex, value);
            }
            return false;
        }




        #endregion
    }
}
