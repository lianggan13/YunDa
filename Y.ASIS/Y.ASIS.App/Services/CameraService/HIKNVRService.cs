using Newtonsoft.Json.Linq;
using Renderer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Utils;
using Y.ASIS.Models.Enums;
using Y.HIKNVR.SDK;


namespace Y.ASIS.App.Services.CameraService
{
    public class HIKNVRService
    {
        public static List<Camera> PlayingCameras { get; } = new List<Camera>();

        public static bool CheckRenderEnv()
        {
            try
            {
                D3DImageSource dImageSource = new D3DImageSource();
                dImageSource.SetupSurface(10, 10, FrameFormat.YV12);

                return dImageSource.IsDeviceAvailable;
            }
            catch
            {
                return false;
            }
        }

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
            HIKNVRClient.Switch(CameraLayout.I, channels);
        }

        public static void Switch(CameraLayout layout, IEnumerable<int> channels, bool needSort = false)
        {
            HIKNVRClient.Switch(layout, channels, needSort);
        }

        public static void SwitchAll()
        {
            //var ipChs = HIKNVRClient.GetCameraIpSwitchChannels();
            LinkPositionVideo(AppGlobal.Instance.MainVM.CurrentPosition);
        }

        public static void LinkVideo(VideoType type, int value)
        {
            var camera = PlayingCameras.FirstOrDefault(c => c.VideoStream.Type == type);

            try
            {
                camera?.LinkVideo(value);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static void LinkDoorVideo(Position pos, int doorIndex, List<int> doorStates)
        {
            var camera = PlayingCameras.FirstOrDefault(c =>
            {
                if (!pos.Videos.Any(v => v.ID == c.VideoStream.ID))
                    return false;

                if (c.VideoStream.Type != VideoType.Door)
                    return false;

                JObject jobj = JObject.Parse(c.VideoStream.Extension);
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

            if (camera == null || doorStates.Count <= 0)
                return;

            try
            {
                camera.LinkVideo(doorStates[0]);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static void LinkPositionVideo(Position pos)
        {
            if (pos == null)
                return;

            List<VideoStream> videos = new List<VideoStream>();
            if (pos.Videos?.Count > 0)
                videos.AddRange(pos.Videos);
            if (pos.ExtraVideos?.Count > 0)
                videos.AddRange(pos.ExtraVideos);

            List<int> chs = videos.OrderBy(v => v.ID).Select(v => v.Channel).Distinct().ToList();

            CameraLayout layout = GetAdaptLayout(chs);

            try
            {
                HIKNVRClient.Switch(layout, chs);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public static CameraLayout GetAdaptLayout(List<int> chs)
        {
            CameraLayout layout = CameraLayout.I;
            //Dictionary<string, CameraLayout> layouts = new Dictionary<string, CameraLayout>();
            var items = Enum.GetValues(typeof(CameraLayout));
            foreach (var item in items)
            {
                string range = EnumUtil.GetDescription(typeof(CameraLayout), item.ToString());
                var ss = range.Split('~');
                if (ss.Length == 1)
                {
                    if (chs.Count == int.Parse(ss[0]))
                    {
                        layout = (CameraLayout)item;
                        break;
                    }
                }
                else
                {
                    if (int.Parse(ss[0]) <= chs.Count && int.Parse(ss[1]) >= chs.Count)
                    {
                        layout = (CameraLayout)item;
                        break;
                    }
                }

                //layouts.Add(range, (CameraLayout)item);
            }
            return layout;
        }
    }
}
