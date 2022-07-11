using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Y.ASIS.App.Communication.Algorithm;
using Y.ASIS.App.Models;
using Y.ASIS.App.Services.CameraService;
using Y.ASIS.App.Utils;
using Y.ASIS.Common.MVVMFoundation;
using Y.HIKNVR.SDK;

namespace Y.ASIS.App.Services
{
    public static class PositionService
    {
        /// <summary>
        /// Judge if no elec
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool NoElec(Position pos)
        {
            bool state = pos.State.Isolation == 2
                         &&
                         pos.State.ElecResult == 2;
            return state;
        }

        public const string PlatformDoorsChanged = nameof(PlatformDoorsChanged);

        public static Messenger Messenger { get; private set; } = new Messenger();

        static PositionService()
        {
            Messenger.Register<object>(PlatformDoorsChanged, OnPlatformDoorsChanged);
        }

        /// <summary>
        /// DoorState:
        /// 00=未知通信失败等异常状态 
        /// 01=使能状态下开 
        /// 02=使能状态下关 
        /// 03=未使能状态下开 
        /// 04=未使能状态下关
        /// </summary>
        /// <param name="param"></param>
        private static void OnPlatformDoorsChanged(dynamic param)
        {
            Position pos = param.Position;
            int doorIndex = param.DoorIndex;
            IEnumerable<int> doorStates = param.DoorStates;

            HIKNVRService.LinkDoorVideo(pos, doorIndex, doorStates.ToList());
        }

        /// <summary>
        ///  Distribute a condition from safeconfirm to a video
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="safecon"></param>
        public static void ApplyVideoCondition(Position pos, SafeConfirm safecon)
        {
            List<VideoStream> videos = new List<VideoStream>();
            videos.AddRange(pos.Videos);
            videos.AddRange(pos.ExtraVideos);

            foreach (VideoStream video in videos)
            {
                if (ParseSafeConfirmIndex(video.Extension, out int index))
                {
                    video.Condition = safecon.Conditions.FirstOrDefault(i => i.Type == video.Type && i.Index == index);
                }
                else
                {
                    video.Condition = safecon.Conditions.FirstOrDefault(i => i.Type == video.Type);
                }
            }
        }



        private static bool ParseSafeConfirmIndex(string extension, out int index)
        {
            bool pass = false;
            index = -1;
            try
            {
                if (!string.IsNullOrEmpty(extension))
                {
                    JObject jobj = JObject.Parse(extension);
                    if (jobj.TryGetValue("SafeConfirmIndex", out JToken jt2))
                    {
                        index = jt2.ToObject<int>();
                        pass = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }

            return pass;
        }


        /// <summary>
        /// Use condition of video for safeconfirm
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="outPhotos"></param>
        /// <returns></returns>
        public static List<Task<DetectResult>> UseVideoCondition(Position pos, Dictionary<string, BitmapImage> outPhotos = null)
        {
            List<Task<DetectResult>> tasks = new List<Task<DetectResult>>();
            if (pos.SafeConfirm == null)
                return tasks;

            List<VideoStream> videos = new List<VideoStream>();
            videos.AddRange(pos.Videos);
            videos.AddRange(pos.ExtraVideos);

            foreach (var video in videos)
            {
                if (video.Condition == null)
                    continue;

                var condition = video.Condition;
                var task = Task.Run(() =>
                {
                    BitmapImage img = null;
                    DetectResult result = null;
                    outPhotos[condition.Text] = null;
                    try
                    {
                        var heart = AlgorithmService.Heartbeat(); // cheak heartbeat before detecting

                        if (heart != null)
                        {
                            result = AlgorithmService.DetectSafety(video.Channel, video.Extension);
                            if (result != null)
                            {
                                condition.RecognizeValues = new List<int>() { int.Parse(result.Result) }; // will trigger property IsSafe
                                img = ImageUtil.Base64ToImage(result.Photo);
                            }
                        }

                        if (img == null)
                        {
                            // add photo by myself
                            var buffer = HIKNVRClient.CaptureCache(video.Channel);
                            img = ImageUtil.BufferToImage(buffer);
                        }

                        if (img != null)
                        {
                            outPhotos[condition.Text] = img;
                            var name = $"{video.Name}_{condition.Text}_{DateTime.Now:HHmmss)}";
                            string path = AppGlobal.CreatePhotoPath("安全确认", $"{name}.png");
                            ImageUtil.SaveImage(img, path);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message, ex);
                    }

                    return result;
                });

                tasks.Add(task);
            }

            return tasks;
        }


    }
}
