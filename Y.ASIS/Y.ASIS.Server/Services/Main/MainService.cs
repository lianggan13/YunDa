using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Common;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Device.ToolBox;
using Y.ASIS.Server.Services.CameraService;
using static Y.ASIS.Server.Database.Track;

namespace Y.ASIS.Server.Services.Main
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        AppExceptionHandle appExceptionHandle = new AppExceptionHandle();
        public void Start()
        {
            AppDomain.CurrentDomain.UnhandledException += Domain_UnhandledException;

            HIKNVRService.Login();

            DataProvider.Instance.TrackList.ForEach(i =>
            {
                i.Positions.ForEach(j =>
                {
                    PLC plc = new PLC(j);
                    PLCManager.Instance.Register(plc);
                    ToolBoxManager.Instance.Register(j);
                });
            });

            DataProvider.Instance.DeviceInfos.ForEach(d =>
            {
                DeviceService.BuildDevice(d);
            });

            var poss = DataProvider.Instance.TrackList.SelectMany(t => t.Positions);
            foreach (var pos in poss)
            {
                var speakers = SpeakerManager.Instance.Devices.Values?.Where(s => pos.DeviceIds.Contains(s.Info.ID));
                pos.SpeakerIds = speakers?.Select(s => s.TerminalId)?.ToList();
            }

            Url url = new Url(ServerGlobal.ServerHostUrl);
            HostConfiguration hostConfig = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true },
            };
            NancyHost host = new NancyHost(hostConfig, url);
            host.Start();
            LogHelper.Info($"Now listening on: {url}");

            PushTaskService.Instance.Start();

            StartPushJob();
        }

        private void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Task.Delay(100);
            if (e.ExceptionObject is Exception ex)
            {
                LogHelper.Fatal($"{ex.Message}", ex);
            }
        }

        private void StartPushJob()
        {
            TimerManager.Instance.AddSchedule(
               "TrackStateSchedule",
               () => PushTrackState(),
               TimeSpan.FromMilliseconds(500d)
            );
        }

        public static void PushTrackState()
        {
            try
            {
                List<TrackState> states = new List<TrackState>();
                DataProvider.Instance.TrackList.ForEach(i =>
                {
                    states.Add(i.State);
                });

                if (states.Any())
                {
                    var ss = Newtonsoft.Json.JsonConvert.SerializeObject(states);
                    PushMessage message = new PushMessage(PushDataType.TrackState, states);
                    PushTaskService.Instance.Push(message, 1);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
    }
}
