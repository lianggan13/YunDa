using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.Manager;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Models;
using Y.HIKNVR.SDK;

namespace Y.ASIS.Server.Services.CameraService
{
    public partial class HIKNVRService
    {
        /// <summary>
        /// 注:取消 项目生成 >>【首先32位】 
        /// 注:取消 项目生成 >>【首先32位】 
        /// 注:取消 项目生成 >>【首先32位】 
        /// </summary>
        /// <returns></returns>
        public static bool Login()
        {
            bool pass = false;
            var ip = LocalConfigManager.GetAppSettingValue("HIK.Ip");
            var port = Convert.ToInt32(LocalConfigManager.GetAppSettingValue("HIK.Port"));
            var username = LocalConfigManager.GetAppSettingValue("HIK.Username");
            var password = LocalConfigManager.GetAppSettingValue("HIK.Password");

            try
            {
                pass = HIKNVRClient.Login(ip, port, username, password);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            return pass;
        }

        public static void Switch(params int[] channels)
        {
            try
            {
                HIKNVRClient.Switch(CameraLayout.I, channels);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static void Switch(CameraLayout layout, IEnumerable<int> channels, bool needSort = false)
        {
            try
            {
                HIKNVRClient.Switch(layout, channels, needSort);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static void SwitchAll(CameraLayout layout = CameraLayout.IX)
        {
            try
            {
                var ips = HIKNVRClient.GetCameraIps();
                var chs = Enumerable.Range(1, ips.Count);
                HIKNVRClient.Switch(layout, chs);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static void LinkDoorVideo(Position pos, int doorIndex, List<int> doorStates)
        {
            return;

            var video = pos.Videos.Where(v => v.Type == VideoType.Door).FirstOrDefault(v =>
            {
                JObject jobj = JObject.Parse(v.Extension);
                if (jobj.TryGetValue("DoorIndex", out JToken jt))
                {
                    int vi = jt.ToObject<int>();
                    return vi == doorIndex;
                }
                else
                {
                    return false;
                }
            });

            if (doorStates.Count <= 0)
                return;

            int doorState = doorStates[0];
            if (doorState == 1 || doorState == 3)
            {
                // door open --> link video fullscreen
                Switch(video.Channel);
            }
            else
            {
                // door close --> link video reset
                SwitchAll();
            }

        }
    }
}
