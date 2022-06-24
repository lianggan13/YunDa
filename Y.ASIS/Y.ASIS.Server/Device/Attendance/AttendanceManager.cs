using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Y.ASIS.Common.Communication;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Manager;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device.ToolBox;
using Y.ASIS.Server.Services;

namespace Y.ASIS.Server.Device.Attendance
{
    class AttendanceManager
    {
        private static AttendanceManager instance;
        public static AttendanceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AttendanceManager();
                }
                return instance;
            }
        }

        private readonly TcpServer tcpServer;
        public readonly Dictionary<string, Attendance> devices;

        private AttendanceManager()
        {
            devices = new Dictionary<string, Attendance>();
            // int port = Convert.ToInt32(LocalConfigManager.GetAppSettingValue("AttenfanceServer.Port"));
            int port = 9910;
            tcpServer = new TcpServer(port)
            {
                EnableNetTouch = true
            };
            tcpServer.Connected += OnConnected;
            tcpServer.Disconnected += OnDisconnected;
            tcpServer.ReceivedData += OnReceivedData;
            tcpServer.ThrownException += OnThrownException;
            tcpServer.Start();
            InitTask();
        }



        private void InitTask()
        {
            TimerManager.Instance.AddSchedule(() =>
            {
                devices.ForEach(i =>
                {
                    i.Value.CheckState();
                });
            },
            TimeSpan.FromMilliseconds(500));
        }

        public void Register(Attendance attendance)
        {
            if (!devices.ContainsKey(attendance.Info.Ip))
            {
                devices.Add(attendance.Info.Ip, attendance);
            }
        }

        public bool AddOrUpdateUsers(ISet<User> IssueUsers)
        {
            if (IssueUsers == null || !IssueUsers.Any())
            {
                return true;
            }

            foreach (User user in IssueUsers)
            {
                if (string.IsNullOrEmpty(user.PhotoUrl) || string.IsNullOrWhiteSpace(user.PhotoUrl))
                {
                    return false;
                }
            }

            List<User> users = IssueUsers.ToList();
            devices.ForEach(i =>
            {
                users.ForEach(j => { i.Value.AddOrUpdateUser(j); });
            });
            return true;
        }

        public bool DeleteUsers(ISet<int> RevokeUsers)
        {
            if (RevokeUsers == null || !RevokeUsers.Any())
            {
                return true;
            }

            List<int> users = RevokeUsers.ToList();
            devices.ForEach(i =>
            {
                users.ForEach(j => { i.Value.DeleteUser(j); });
            });

            return true;
        }

        private void OnConnected(object sender, SocketEventArgs e)
        {
            LogHelper.Info("刷脸机" + e.Address + "已接入");

            if (devices.TryGetValue(e.Ip, out Attendance attendance))
            {
                attendance.Buffer = new List<byte>();
                attendance.SyncTime();
            }
        }

        private void OnDisconnected(object sender, SocketEventArgs e)
        {
            LogHelper.Warn("刷脸机" + e.Address + "已断开");

            if (devices.TryGetValue(e.Ip, out Attendance attendance))
            {
                attendance.Commands.RemoveAll(c => c.Command.Data is AttendanceSyncTimeCommandData);
            }
        }

        private void OnThrownException(object sender, SocketExceptionEventArgs e)
        {
            LogHelper.Error($"刷脸机 {e.Address} 异常:{e.Exception.Message}", e.Exception);
        }

        private void OnReceivedData(object sender, SocketReceivedDataEventArgs e)
        {
            if (!devices.TryGetValue(e.Ip, out Attendance attendance))
            {
                return;
            }

            attendance.Buffer.AddRange(e.Data);
            if (attendance.Buffer.Count < 4)
            {
                return;
            }

            while (attendance.Buffer.Count >= 4)
            {
                int headerLength = 4;
                int packageLength = BitConverter.ToInt32(attendance.Buffer.Take(4).Reverse().ToArray(), 0);
                int length = headerLength + packageLength;
                if (attendance.Buffer.Count >= length)
                {
                    byte[] data = attendance.Buffer.Skip(headerLength).Take(packageLength).ToArray();
                    if (attendance.Buffer.Count == 0)
                    {
                        return;
                    }
                    attendance.Buffer.RemoveRange(0, length);
                    string json = GetString(data);

                    AttendanceRequest req = json.JsonDeserialize<AttendanceRequest>();
                    if (req != null)
                    {
                        attendance.State = DeviceState.Online;
                        attendance.LastTime = DateTime.Now;
                        switch (req.Command)
                        {
                            case AttendanceCommandType.AKE:
                                string sn = req.Data.Value<string>(nameof(sn));
                                SendHeartbeat(e.Ip, e.Port, sn);
                                break;
                            case AttendanceCommandType.GetRequest:
                                SendCommand(e.Ip, e.Port);
                                break;
                            case AttendanceCommandType.RecogniseResult:
                                AttendanceRecord record = req.Data.ToObject<AttendanceRecord>();
                                HandleAttendaceRecord(attendance, record);
                                SendRecogniseResultCallback(e.Ip, e.Port);
                                break;
                            case AttendanceCommandType.Return:
                                CommandCallback(req.Data, attendance, e.Ip, e.Port);
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                }
                if (attendance.Buffer.Count > 0)
                    attendance.Buffer.RemoveAt(0);
            }
        }


        private void SendHeartbeat(string ip, int port, string sn)
        {
            AttendanceResponse resp = new AttendanceResponse(AttendanceCommandType.AKE)
            {
                Data = new AttendanceHeartbeatData()
                {
                    SN = sn
                }
            };
            Send(ip, port, resp);
        }

        private void SendCommand(string ip, int port, bool delay = false)
        {
            if (devices.TryGetValue(ip, out Attendance attendance))
            {
                AttendanceCommand attendanceCommand = attendance.Commands.FirstOrDefault(i => !i.Executing);
                if (attendanceCommand == null)
                {
                    if (delay)
                    {
                        Thread.Sleep(1000);
                    }
                    attendanceCommand = new AttendanceCommand()
                    {
                        Command = new AttendanceResponse(AttendanceCommandType.GetRequest)
                        {
                            Data = new AttendanceDeviceInfoCommandData()
                        }
                    };
                }
                attendanceCommand.Executing = true;
                Send(ip, port, attendanceCommand.Command);
            }
        }

        private void HandleAttendaceRecord(Attendance attendance, AttendanceRecord record)
        {
            int no = Convert.ToInt32(record.Id);    // 作业人员工号
            if (attendance.Type == AttendanceType.Revoke)
            {
                List<int> list = new List<int>() { no };
                ISet<int> workNos = new HashSet<int>(list);

                DeleteUsers(workNos);

                PLCManager.Instance.PLCDictionary.ForEach(i =>
                {
                    i.Value.RevokeUsers(workNos, PLCNodeType.Workers);
                });

                //DataProvider.Instance.UpdateUserAllowUpdateState(workNos, true);
                PositionService.ClearPositionToUsers(workNos);

                // 工具柜 销权 方式一(直接删除用户)
                //ToolBoxManager.Instance.DeleteUser(deletesSet);

                // 工具柜 销权 方式二()
                ToolBoxManager.Instance.ProhibitKey(workNos);

                // workno --> position

                List<UserAuthRecord> records = DataProvider.Instance.GetUserAuthRecords(list);
                records.ForEach(i =>
                {
                    i.Revoke();
                    DataProvider.Instance.AddOrUpdateUserAuthRecord(i);
                });

                List<ToolAuthRecord> toolRecords = DataProvider.Instance.GetToolAuthRecords(list);
                toolRecords.ForEach(i =>
                {
                    i.Revoke();
                    DataProvider.Instance.AddOrUpdateToolAuthRecord(i);
                });
            }
        }

        private void SendRecogniseResultCallback(string ip, int port)
        {
            AttendanceResponse resp = new AttendanceResponse(AttendanceCommandType.RecogniseResult)
            {
                Data = new AttendanceResponseData()
            };
            Send(ip, port, resp);
        }

        private void CommandCallback(JObject data, Attendance attendance, string ip, int port)
        {
            string propertyName = "result";
            if (data.ContainsKey(propertyName))
            {
                string value = data.GetValue(propertyName).ToObject<string>();
                if (value != "success")
                {
                    AttendanceCommand command = attendance.Commands.FirstOrDefault(i => i.Executing);
                    if (command != null)
                    {
                        LogHelper.Error($"Attendance device [{ip}:{port}] execute command error.\r\nCommand:{command.JsonSerialize()}\r\nResult:{data.JsonSerialize()}");
                    }
                }
                attendance.Commands.RemoveAll(i => i.Executing);
                SendCommand(ip, port, true);
            }
        }

        private void Send(string ip, int port, AttendanceResponse resp)
        {
            string json = resp.JsonSerialize();
            byte[] data = GetBytes(json);
            byte[] lengthByte = BitConverter.GetBytes(data.Length).Reverse().ToArray();
            tcpServer.Send(ip, port, lengthByte.Concat(data).ToArray());

            //string ss = $"运达下发: {json}";
            //Console.WriteLine(ss);
            //LogHelper.Debug(ss);
        }

        private string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        private byte[] GetBytes(string json)
        {
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
