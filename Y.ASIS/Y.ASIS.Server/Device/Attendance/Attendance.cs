using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Y.ASIS.Common.Communication;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Models;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Device.Attendance
{
    class Attendance : DeviceBase
    {
        #region properties
        public List<byte> Buffer { get; set; } = new List<byte>();

        public List<AttendanceCommand> Commands { get; set; } = new List<AttendanceCommand>();

        public AttendanceType Type { get; set; }

        public DateTime LastTime { get; set; }

        private DeviceState state;

        public DeviceState State
        {
            get { return state; }
            set
            {
                LastTime = DateTime.Now;
                if (state != value)
                {
                    state = value;
                }
            }
        }
        #endregion

        public Attendance(DeviceInfo info)
        {
            Info = info;
            try
            {
                JObject jobj = JObject.Parse(info.Extension);
                string typeStr = jobj.Value<string>(nameof(Type));
                Type = (AttendanceType)Enum.Parse(typeof(AttendanceType), typeStr);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }


        public void CheckState()
        {
            if (DateTime.Now - LastTime > TimeSpan.FromSeconds(10))
            {
                State = DeviceState.Offline;
            }
        }

        public void AddOrUpdateUser(User user)
        {
            AttendanceAddOrUpdateUserCommandData data = new AttendanceAddOrUpdateUserCommandData()
            {
                Name = user.Name,
                WorkNo = user.No.ToString(),
                IcCard = user.CardNo == 0 ? "" : user.CardNo.ToString(),
                PhotoUrl = user.PhotoUrl.IsNullOrEmptyOrWhiteSpace() ? "" : user.PhotoUrl
            };
            if (user.PhotoUrl.IsNullOrEmptyOrWhiteSpace())
            {
                data.RecogPermission = "3";
                //data.RecogPermission = "8";
                data.PhotoUrl = ImageUtil.DefaultImage;
            }
            AttendanceCommand command = new AttendanceCommand()
            {
                Command = new AttendanceResponse(AttendanceCommandType.GetRequest)
                {
                    Data = data
                }
            };
            Commands.Insert(0, command);
        }

        public void DeleteUser(int workNo)
        {
            AttendanceCommand command = new AttendanceCommand()
            {
                Command = new AttendanceResponse(AttendanceCommandType.GetRequest)
                {
                    Data = new AttendanceDeleteUserCommandData()
                    {
                        WorkNo = workNo.ToString()
                    }
                }
            };
            Commands.Insert(0, command);
        }

        public void SyncTime()
        {
            AttendanceCommand command = new AttendanceCommand()
            {
                Command = new AttendanceResponse(AttendanceCommandType.GetRequest)
                {
                    Data = new AttendanceSyncTimeCommandData()
                    {
                        Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                }
            };
            Commands.Add(command);
        }
    }
}
