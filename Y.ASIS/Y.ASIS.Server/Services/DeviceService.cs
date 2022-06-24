using System.Collections.Generic;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Server.Device.Attendance;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Models;

namespace Y.ASIS.Server.Services
{
    public class DeviceService
    {
        public static List<DeviceBase> Devices { get; private set; }

        static DeviceService()
        {
            Devices = new List<DeviceBase>();
        }


        public static void BuildDevice(DeviceInfo info)
        {
            switch (info.Type)
            {
                case DeviceType.Attendance:
                    Attendance attendnc = new Attendance(info);
                    AttendanceManager.Instance.Register(attendnc);

                    Devices.Add(attendnc);
                    break;
                case DeviceType.Speaker:
                    Speaker speaker = new Speaker(info);
                    SpeakerManager.Instance.Register(speaker);

                    Devices.Add(speaker);
                    break;
                case DeviceType.Switcher:
                    // no use switcher temporarily
                    //Switcher switcher = new Switcher(info);
                    //SwitcherManager.Instance.Register(switcher);
                    break;
                default:
                    break;
            }
        }

    }
}
