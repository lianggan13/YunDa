using NetSDKCS;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Y.ASIS.App.Controls;
using Y.ASIS.App.Models;
using Y.ASIS.App.Windows;

namespace Y.ASIS.App.Services.CameraService
{
    public class DaHuaCamera : Camera
    {
        public static ConcurrentDictionary<string, IntPtr> loginUserIds = new ConcurrentDictionary<string, IntPtr>();
        public static ConcurrentDictionary<int, IntPtr> playerIds = new ConcurrentDictionary<int, IntPtr>();

        static DaHuaCamera()
        {
            fHaveReConnectCallBack m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
            fDisConnectCallBack m_DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
            MultiPlatformDllLoad.LoadDll(OriginalSDK.LIBRARYNETSDK);
            MultiPlatformDllLoad.LoadDll(OriginalSDK.LIBRARYCONFIGSDK);
            NETClient.InitWithDefaultSetting(m_DisConnectCallBack, m_ReConnectCallBack, IntPtr.Zero, null);
        }

        private static void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            Debug.WriteLine("[%s] Port[%d] 断开!", pchDVRIP, nDVRPort);
        }

        private static void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            Debug.WriteLine("[%s] Port[%d] 重连!", pchDVRIP, nDVRPort);
        }

        private HwndRender render;

        public DaHuaCamera(VideoStream vs, HwndRender render)
        {
            this.VideoStream = vs;
            this.render = render;
        }

        public override void Play()
        {
            IntPtr loginId = Login(VideoStream); // first playing needs login once

            IntPtr playId = NETClient.RealPlay(loginId, 0, render.Handle);

            playerIds[VideoStream.ID] = playId;
        }

        public override void Stop()
        {
            playerIds.TryGetValue(VideoStream.ID, out IntPtr playId);
            NETClient.StopRealPlay(playId);

            render.InvalidateVisual();
        }

        public IntPtr Login(VideoStream vs)
        {
            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
            string key = $"{vs.Ip}:{vs.Port}";
            IntPtr loginId;
            if (loginUserIds.TryGetValue(key, out IntPtr lg))
            {
                loginId = lg;
            }
            else
            {
                // one deivce(ip:port) just login once
                loginId = NETClient.Login(vs.Ip, (ushort)vs.Port, vs.UserName, vs.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                if (loginId != IntPtr.Zero)
                    loginUserIds.TryAdd(key, loginId);
            }

            return loginId;
        }

        public override void FullScreenVideo(object sender, MouseButtonEventArgs e)
        {
            ViewVideoWindow window = new ViewVideoWindow()
            {
                Owner = Application.Current.MainWindow
            };
            window.render.Visibility = Visibility.Visible;

            IntPtr winPlayId = IntPtr.Zero;
            window.Loaded += (s, a) =>
            {
                //window.render.Visibility = Visibility.Visible;
                IntPtr loginId = Login(VideoStream); // first playing needs login once
                winPlayId = NETClient.RealPlay(loginId, 0, window.render.Handle);
            };

            window.Closing += (s, a) =>
            {
                NETClient.StopRealPlay(winPlayId);
            };

            window.Show();
        }

        public override void SaveCapture(string name)
        {
            // TODO: save
        }
    }
}
