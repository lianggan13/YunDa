using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Y.ASIS.App.Common;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Services.CameraService
{
    public abstract class Camera
    {
        public VideoStream VideoStream;
        public abstract void Play();

        public abstract void Stop();

        public abstract void FullScreenVideo(object sender, MouseButtonEventArgs e);

        public virtual void LinkVideo(int value)
        {
            var curWin = WindowManager.FindWindwByType(typeof(ViewVideoWindow));

            List<LinkVideoModel> videos = new List<LinkVideoModel>()
            {
                new LinkVideoModel(){ VideoType = VideoType.Isolation,State = 01,Name = "合闸位置",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Isolation,State = 02,Name = "分闸位置",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Isolation,State = 03,Name =  "合闸中",CanExecute = true},
                new LinkVideoModel(){ VideoType = VideoType.Isolation,State = 04,Name =  "分闸中",CanExecute = true},

                new LinkVideoModel(){ VideoType = VideoType.Grounding,State = 01,Name = "挂接地位置",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Grounding,State = 02,Name = "撤接地位置",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Grounding,State = 03,Name =  "挂接地中",CanExecute = true},
                new LinkVideoModel(){ VideoType = VideoType.Grounding,State = 04,Name =  "撤接地中",CanExecute = true},

                new LinkVideoModel(){ VideoType = VideoType.Elec,State = 01,Name = "验电位置",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Elec,State = 02,Name = "验电回位",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Elec,State = 03,Name =  "挂验电中",CanExecute = true},
                new LinkVideoModel(){ VideoType = VideoType.Elec,State = 04,Name =  "撤验电中",CanExecute = true},

                new LinkVideoModel(){ VideoType = VideoType.Door,State = 01,Name =  "使能状态下开",CanExecute = true},
                new LinkVideoModel(){ VideoType = VideoType.Door,State = 02,Name =  "使能状态下关",CanExecute = false},
                new LinkVideoModel(){ VideoType = VideoType.Door,State = 03,Name =  "未使能状态下开",CanExecute = true},
                new LinkVideoModel(){ VideoType = VideoType.Door,State = 04,Name =  "未使能状态下关",CanExecute = false},
            };

            var linkvm = videos.FirstOrDefault(z => z.VideoType == VideoStream.Type && z.State == value);

            if (linkvm == null)
                return;

            if (linkvm.CanExecute)
            {
                curWin?.Close();
                if (curWin == null)
                {
                    FullScreenVideo(null, null);
                    HIKNVRService.Switch(VideoStream.Channel);

                    SaveCapture(linkvm.Name);
                }
            }
            else
            {
                if (curWin != null)
                {
                    curWin?.Close();
                    SaveCapture(linkvm.Name);
                }
                HIKNVRService.SwitchAll();
            }
        }

        public abstract void SaveCapture(string name);
    }


    public class LinkVideoModel
    {
        public VideoType VideoType { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public bool CanExecute { get; set; }
    }
}
