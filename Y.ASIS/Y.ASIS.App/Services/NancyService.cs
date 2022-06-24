using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y.ASIS.App.Models;
using Y.ASIS.App.ViewModels;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.App.Services
{
    public class NancyService : NancyModule
    {
        private static MainViewModel ViewModel;

        public NancyService()
        {
            Post("/push", Push);
        }

        private Response Push(dynamic _)
        {
            byte[] data = new byte[Request.Body.Length]; // Request: 来自 Server 的定时 Post 请求
            Request.Body.Read(data, 0, data.Length);
            string json = Encoding.UTF8.GetString(data);
            PushMessage message = json.JsonDeserialize<PushMessage>();
            if (message != null)
            {
                try
                {
                    switch (message.Type)
                    {
                        case PushDataType.TrackState:
                            var trackStates = json.JsonDeserialize<PushMessage<IEnumerable<TrackState>>>().Data.ToList();
                            ViewModel.UpdateTrackState(trackStates);
                            break;
                        case PushDataType.Device:
                            var deviceState = json.JsonDeserialize<PushMessage<DeviceStateMessage>>().Data;
                            ViewModel.UpdateDevice(deviceState);
                            break;
                        case PushDataType.Fault:
                            ViewModel.AddFaultRecord(json.JsonDeserialize<PushMessage<FaultRecord>>().Data);
                            ViewModel.RefreshUnhandleWarningsCount();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }

                return "OK";
            }
            return "ERROR";
        }

        public static void Start(MainViewModel vm)
        {
            ViewModel = vm;
            string address = LocalConfigManager.GetAppSettingValue("PushAddress");

            Url url = new Url(address);
            HostConfiguration hostConfig = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true },
            };
            try
            {
                NancyHost host = new NancyHost(hostConfig, url);
                host.Start();
                LogHelper.Info("push service is running on " + url);
            }
            catch (Exception e)
            {
                LogHelper.Info("push service startup fail. error: " + e.Message);
                Windows.MessageWindow.Show("NancyHost 启动失败");
                //Environment.Exit(0);
            }
        }
    }
}
