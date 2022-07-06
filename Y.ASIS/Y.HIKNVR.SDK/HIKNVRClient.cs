using Renderer.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Y.HIKNVR.SDK
{
    public enum CameraLayout
    {
        /// <summary>
        /// 1画面
        /// </summary>
        [Description("1")]
        I = 0,
        /// <summary>
        /// 4画面
        /// </summary>
        [Description("2~4")]
        IV,
        /// <summary>
        /// 9画面
        /// </summary>
        [Description("9")]
        IX,
        /// <summary>
        /// 16画面
        /// </summary>
        [Description("10~16")]
        XVI,
        /// <summary>
        /// 6画面
        /// </summary>
        [Description("5~6")]
        VI,
        /// <summary>
        /// 8画面
        /// </summary>
        [Description("7~8")]
        VIII,
        /// <summary>
        /// 25画面
        /// </summary>
        [Description("17~25")]
        XXV,
        /// <summary>
        /// 32画面
        /// </summary>
        [Description("26~32")]
        XXXII,
        /// <summary>
        /// 36画面
        /// </summary>
        [Description("33~36")]
        XXXVI,
        /// <summary>
        /// 64画面(上限)
        /// </summary>
        [Description("37~64")]
        MAX = 0xff
    }

    public static class HIKNVRClient
    {
        private static int userId = -1;
        private static ConcurrentDictionary<int, RenderderWrapper> renderders = new ConcurrentDictionary<int, RenderderWrapper>();

        public static bool Login(string ip, int port, string username, string password)
        {
            bool pass = NET_DVR_Init();
            if (!pass)
            {
                uint errorCode = NET_DVR_GetLastError();
                throw new Exception("HIK NVR init error. Error code:" + errorCode);
            }
            NET_DVR_DEVICEINFO_V30 deviceInfo = default;
            userId = NET_DVR_Login_V30(ip, port, username, password, ref deviceInfo);
            if (userId < 0)
            {
                throw new Exception("HIK NVR login failed.");
            }
            return pass;
        }

        public static void Preview(int channel, Image image)
        {
            if (userId < 0)
            {
                throw new Exception("HIK NVR not Login.");
            }
            if (image == null)
            {
                return;
            }
            //RenderderWrapper renderder = RenderderWrapperHelper.GetRenderder(image);
            RenderderWrapper renderder = null; // must to rebuild renerder by zhangliang 2022.04.11
            if (renderder == null)
            {
                renderder = new RenderderWrapper(image)
                {
                    Callback = new REALDATACALLBACK(PreviceCallback),
                    Decode = new DECCBFUN(Decode)
                };
                RenderderWrapperHelper.SetRenderder(image, renderder);
            }

            NET_DVR_PREVIEWINFO info = new NET_DVR_PREVIEWINFO()
            {
                hPlayWnd = IntPtr.Zero,
                lChannel = channel,
                dwStreamType = 1,   // 0主码流 1子码流 2码流3 ...
                dwLinkMode = 4,     // 0tcp 1udp 2多播 3rtp 4rtp/rtsp 5rtsp/http
                bBlocked = false,   // 是否阻塞取流
                dwDisplayBufNum = 15
            };
            GCHandle handle = GCHandle.Alloc(renderder);
            IntPtr rendererPtr = GCHandle.ToIntPtr(handle);
            renderder.RealHandle = NET_DVR_RealPlay_V40(userId, ref info, renderder.Callback, rendererPtr);

            renderder.OnDisposed += (s, e) =>
            {
                handle.Free();
            };
        }

        public static void StopPreview(Image image)
        {
            RenderderWrapper renderder = RenderderWrapperHelper.GetRenderder(image);
            if (renderder == null)
            {
                return;
            }
            // TODO 这里使用主线程去关闭预览会导致程序卡死 原因不明 暂用这种方式解决. 
            Task.Factory.StartNew(() =>
            {
                renderder.Closing = true;
                if (!NET_DVR_StopRealPlay(renderder.RealHandle))
                {
                    return;
                }
                if (!PlayM4_Stop(renderder.Port))
                {
                    return;
                }
                if (!PlayM4_CloseStream(renderder.Port))
                {
                    return;
                }
                if (!PlayM4_FreePort(renderder.Port))
                {
                    return;
                }
                bool success;
                do
                {
                    success = renderders.TryRemove(renderder.Port, out _);
                    if (success)
                    {
                        renderder.Dispose();
                    }
                    Thread.Sleep(2);
                }
                while (!success);
            });
        }

        public static byte[] CaptureCache(int channel)
        {
            NET_DVR_JPEGPARA jpeg = new NET_DVR_JPEGPARA
            {
                wPicQuality = 0,
                wPicSize = 0xff
            };
            uint bufferSize = 800000;
            byte[] buffer = new byte[bufferSize];
            uint readSize = 0;
            bool success = NET_DVR_CaptureJPEGPicture_NEW(userId, channel, ref jpeg, buffer, bufferSize, ref readSize);
            if (!success)
            {
                uint code = NET_DVR_GetLastError();
                throw new Exception($"{channel} capture error. code {code}");
            }
            return buffer;
        }


        private static void PreviceCallback(int realHandle, int dataType, IntPtr buffer, uint buffSize, IntPtr user)
        {
            GCHandle handle = GCHandle.FromIntPtr(user);
            RenderderWrapper wrapper = (RenderderWrapper)handle.Target;
            if (wrapper.Closing)
            {
                return;
            }
            switch (dataType)
            {
                case 1: // header
                    if (buffSize <= 0)
                    {
                        break;
                    }
                    int port = -1;
                    if (!PlayM4_GetPort(ref port))
                    {
                        break;
                    }
                    if (port < 0)
                    {
                        return;
                    }
                    wrapper.Port = port;
                    if (!renderders.ContainsKey(port))
                    {
                        bool success;
                        do
                        {
                            success = renderders.TryAdd(port, wrapper);
                            Console.WriteLine("Renderders Add: " + port);
                            Thread.Sleep(10);
                        }
                        while (!success);
                    }

                    if (!PlayM4_OpenStream(port, buffer, buffSize, 2 * 1024 * 1024))
                    {
                        break;
                    }
                    if (!PlayM4_SetDecCallBackEx(port, wrapper.Decode, IntPtr.Zero, 0))
                    {
                        break;
                    }
                    if (!PlayM4_Play(port, IntPtr.Zero))
                    {
                        break;
                    }
                    break;
                case 2: // stream data
                    int retry = 0;
                    while (retry < 100)
                    {
                        if (PlayM4_InputData(wrapper.Port, buffer, buffSize))
                        {
                            break;
                        }
                        Thread.Sleep(2);
                        retry++;
                    }
                    break;
                default:
                    break;
            }
        }

        private static void Decode(int port, IntPtr buff, int size, ref FRAME_INFO frameInfo, int reserved1, int reserved2)
        {
            if (frameInfo.Type != 3)
            {
                return;
            }
            if (renderders.TryGetValue(port, out RenderderWrapper wrapper))
            {
                if (wrapper.Closing || wrapper.Rendering)
                {
                    return;
                }
                if (!wrapper.HasSetupSurface)
                {
                    wrapper.Render.SetupSurface(frameInfo.Width, frameInfo.Height, FrameFormat.YV12);
                    wrapper.HasSetupSurface = true;
                }
                wrapper.Rendering = true;
                wrapper.Render.Render(buff);
                wrapper.Rendering = false;
            }
        }

        #region sdk
        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_DEVICEINFO_V30
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
            public byte[] sSerialNumber;
            public byte byAlarmInPortNum;
            public byte byAlarmOutPortNum;
            public byte byDiskNum;
            public byte byDVRType;
            public byte byChanNum;
            public byte byStartChan;
            public byte byAudioChanNum;
            public byte byIPChanNum;
            public byte byZeroChanNum;
            public byte byMainProto;
            public byte bySubProto;
            public byte bySupport;
            public byte bySupport1;
            public byte bySupport2;
            public ushort wDevType;
            public byte bySupport3;
            public byte byMultiStreamProto;
            public byte byStartDChan;
            public byte byStartDTalkChan;
            public byte byHighDChanNum;
            public byte bySupport4;
            public byte byLanguageType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_PREVIEWINFO
        {
            public int lChannel;
            public uint dwStreamType;
            public uint dwLinkMode;
            public IntPtr hPlayWnd;
            public bool bBlocked;
            public bool bPassbackRecord;
            public byte byPreviewMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byStreamID;
            public byte byProtoType;
            public byte byRes1;
            public byte byVideoCodingType;
            public uint dwDisplayBufNum;
            public byte byNPQMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 215, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        public struct FRAME_INFO
        {
            public int Width;
            public int Height;
            public int Stamp;
            public int Type;
            public int FrameRate;
            public uint FrameNum;

            public void Init()
            {
                Width = 0;
                Height = 0;
                Stamp = 0;
                Type = 0;
                FrameRate = 0;
                FrameNum = 0;
            }
        }

        public delegate void REALDATACALLBACK(int realHandle, int dataType, IntPtr puffer, uint bufSize, IntPtr user);

        public delegate void DECCBFUN(int port, IntPtr buff, int size, ref FRAME_INFO frameInfo, int reserved1, int reserved2);

        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern bool NET_DVR_Init();

        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern uint NET_DVR_GetLastError();

        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern int NET_DVR_Login_V30(string ip, int port, string username, string password, ref NET_DVR_DEVICEINFO_V30 deviceInfo);

        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern int NET_DVR_RealPlay_V40(int userId, ref NET_DVR_PREVIEWINFO previewInfo, REALDATACALLBACK callback, IntPtr user);

        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern bool NET_DVR_StopRealPlay(int realHandle);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_SetDecCallBackEx(int port, DECCBFUN DecCBFun, IntPtr dest, int destSize);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_GetPort(ref int port);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_OpenStream(int port, IntPtr fileHeadBuff, uint size, uint buffPoolSize);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_Play(int port, IntPtr hWnd);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_InputData(int port, IntPtr buffer, uint size);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_Stop(int port);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_CloseStream(int port);

        [DllImport(".\\HIK\\PlayCtrl.dll")]
        static extern bool PlayM4_FreePort(int port);
        #endregion




        private const int PASSWD_LEN = 16;//密码长度

        private const int NAME_LEN = 32;//用户名长度

        private const int MAX_DOMAIN_NAME = 64;  /* 最大域名长度 */

        private const int MAX_WINDOW_V40 = 64;


        private const int NET_DVR_GET_PREVIEW_SWITCH_CFG = 6166;
        private const int NET_DVR_SET_PREVIEW_SWITCH_CFG = 6167;

        //9000 IPC接入
        private const int MAX_ANALOG_CHANNUM = 32;//最大32个模拟通道
        private const int MAX_IP_CHANNEL = 32;//允许加入的最多IP通道数
        private const int MAX_IP_DEVICE_V40 = 64;//允许接入的最大IP设备数

        /* 最大支持的通道数 最大模拟加上最大IP支持 */
        private const int MAX_CHANNUM_V30 = MAX_ANALOG_CHANNUM + MAX_IP_CHANNEL;//64

        //IP接入配置参数 （NET_DVR_IPPARACFG_V40结构）
        private const int NET_DVR_GET_IPPARACFG_V40 = 1062; //获取IP接入配置信息 
        private const int NET_DVR_SET_IPPARACFG_V40 = 1063; //设置IP接入配置信息
        /// <summary>
        /// 根据IP获取通道号
        /// </summary>
        /// <param name="ip">传入的IP</param>
        /// <returns>通道号</returns>
        public static int GetChannel(string ip)
        {
            NET_DVR_PREVIEW_SWITCH_COND GetPreviewSwitchCond = new NET_DVR_PREVIEW_SWITCH_COND();
            uint dwSize = (uint)Marshal.SizeOf(GetPreviewSwitchCond);
            GetPreviewSwitchCond.dwSize = dwSize;
            GetPreviewSwitchCond.byGroup = 0;
            GetPreviewSwitchCond.byVideoOutType = 1;
            GetPreviewSwitchCond.byGetDefaultPreviewSet = 0;
            GetPreviewSwitchCond.byPreviewNumber = 0;
            IntPtr ptrGetPreviewSwitchCond = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(GetPreviewSwitchCond, ptrGetPreviewSwitchCond, false);

            NET_DVR_PREVIEW_SWITCH_CFG GetPreviewSwitchCfg = new NET_DVR_PREVIEW_SWITCH_CFG();
            uint Size = (uint)Marshal.SizeOf(GetPreviewSwitchCfg);
            GetPreviewSwitchCfg.wSwitchSeq = new short[MAX_WINDOW_V40];
            GetPreviewSwitchCfg.byRes = new byte[32];

            IntPtr ptrGetPreviewSwitchCfg = Marshal.AllocHGlobal((Int32)Size);
            Marshal.StructureToPtr(GetPreviewSwitchCfg, ptrGetPreviewSwitchCfg, false);

            IntPtr ptrStatusList = Marshal.AllocHGlobal(4);

            if (!NET_DVR_GetDeviceConfig(userId, NET_DVR_GET_PREVIEW_SWITCH_CFG, 0, ptrGetPreviewSwitchCond, dwSize, ptrStatusList, ptrGetPreviewSwitchCfg, Size))
            {
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrStatusList);
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                uint code = NET_DVR_GetLastError();

                throw new Exception($"GetChannel Get Device Config error. code {code}");
            }
            else
            {
                GetPreviewSwitchCfg = (NET_DVR_PREVIEW_SWITCH_CFG)Marshal.PtrToStructure(ptrGetPreviewSwitchCfg, typeof(NET_DVR_PREVIEW_SWITCH_CFG));
            }

            NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40 = new NET_DVR_IPPARACFG_V40();
            dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);
            uint dwReturn = 0;
            int iGroupNo = 0; //该方法仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取


            if (!NET_DVR_GetDVRConfig(userId, NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                uint code = NET_DVR_GetLastError();
                throw new Exception($"GetChannel Get DVR Config error. code {code}");
            }
            else
            {
                //获取通道成功
                m_struIpParaCfgV40 = (NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(NET_DVR_IPPARACFG_V40));
                for (int i = 0; i < m_struIpParaCfgV40.dwDChanNum; i++)
                {
                    switch (m_struIpParaCfgV40.struStreamMode[i].byGetStreamType)
                    {
                        case 0:
                            string struIp = System.Text.Encoding.UTF8.GetString(m_struIpParaCfgV40.struIPDevInfo[i].struIP.sIpV4).TrimEnd('\0');
                            if (!string.IsNullOrEmpty(struIp) && Convert.ToBoolean(m_struIpParaCfgV40.struIPDevInfo[i].byEnable))
                            {
                                if (struIp == ip)
                                {
                                    Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                                    Marshal.FreeHGlobal(ptrStatusList);
                                    Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                                    Marshal.FreeHGlobal(ptrIpParaCfgV40);

                                    return (int)GetPreviewSwitchCfg.wSwitchSeq[i];
                                }
                            }
                            break;
                        default: break;
                    }
                }

            }

            Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
            Marshal.FreeHGlobal(ptrStatusList);
            Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
            Marshal.FreeHGlobal(ptrIpParaCfgV40);

            return 0;
        }


        /// <summary>
        /// 获取摄像头Ip(注:第一组 最多64个) 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCameraIps()
        {
            List<string> ips = new List<string>();
            NET_DVR_PREVIEW_SWITCH_COND GetPreviewSwitchCond = new NET_DVR_PREVIEW_SWITCH_COND();
            uint dwSize = (uint)Marshal.SizeOf(GetPreviewSwitchCond);
            GetPreviewSwitchCond.dwSize = dwSize;
            GetPreviewSwitchCond.byGroup = 0;
            GetPreviewSwitchCond.byVideoOutType = 1;
            GetPreviewSwitchCond.byGetDefaultPreviewSet = 0;
            GetPreviewSwitchCond.byPreviewNumber = 0;
            IntPtr ptrGetPreviewSwitchCond = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(GetPreviewSwitchCond, ptrGetPreviewSwitchCond, false);

            NET_DVR_PREVIEW_SWITCH_CFG GetPreviewSwitchCfg = new NET_DVR_PREVIEW_SWITCH_CFG();
            uint Size = (uint)Marshal.SizeOf(GetPreviewSwitchCfg);
            GetPreviewSwitchCfg.wSwitchSeq = new short[MAX_WINDOW_V40];
            GetPreviewSwitchCfg.byRes = new byte[32];

            IntPtr ptrGetPreviewSwitchCfg = Marshal.AllocHGlobal((Int32)Size);
            Marshal.StructureToPtr(GetPreviewSwitchCfg, ptrGetPreviewSwitchCfg, false);

            IntPtr ptrStatusList = Marshal.AllocHGlobal(4);

            if (!NET_DVR_GetDeviceConfig(userId, NET_DVR_GET_PREVIEW_SWITCH_CFG, 0, ptrGetPreviewSwitchCond, dwSize, ptrStatusList, ptrGetPreviewSwitchCfg, Size))
            {
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrStatusList);
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                uint code = NET_DVR_GetLastError();

                throw new Exception($"GetChannel Get Device Config error. code {code}");
            }
            else
            {
                GetPreviewSwitchCfg = (NET_DVR_PREVIEW_SWITCH_CFG)Marshal.PtrToStructure(ptrGetPreviewSwitchCfg, typeof(NET_DVR_PREVIEW_SWITCH_CFG));
            }

            NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40 = new NET_DVR_IPPARACFG_V40();
            dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);
            uint dwReturn = 0;
            int iGroupNo = 0; //该方法仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!NET_DVR_GetDVRConfig(userId, NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                uint code = NET_DVR_GetLastError();
                throw new Exception($"GetChannel Get DVR Config error. code {code}");
            }
            else
            {
                //获取通道成功
                m_struIpParaCfgV40 = (NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(NET_DVR_IPPARACFG_V40));
                for (int i = 0; i < m_struIpParaCfgV40.struIPDevInfo.Length; i++)
                {
                    switch (m_struIpParaCfgV40.struStreamMode[i].byGetStreamType)
                    {
                        case 0:
                            NET_DVR_IPDEVINFO_V31 device = m_struIpParaCfgV40.struIPDevInfo[i];
                            string struIp = Encoding.UTF8.GetString(device.struIP.sIpV4).TrimEnd('\0');
                            bool enable = Convert.ToBoolean(device.byEnable);

                            if (!string.IsNullOrEmpty(struIp) && enable)
                            {
                                //int ch = (int)GetPreviewSwitchCfg.wSwitchSeq[i];
                                ips.Add(struIp);
                            }
                            break;
                        default: break;
                    }
                }
            }

            Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
            Marshal.FreeHGlobal(ptrStatusList);
            Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
            Marshal.FreeHGlobal(ptrIpParaCfgV40);

            return ips;
        }


        public static List<Tuple<string, int>> GetCameraIpSwitchChannels()
        {
            List<Tuple<string, int>> ipSwithChans = new List<Tuple<string, int>>();
            NET_DVR_PREVIEW_SWITCH_COND GetPreviewSwitchCond = new NET_DVR_PREVIEW_SWITCH_COND();
            uint dwSize = (uint)Marshal.SizeOf(GetPreviewSwitchCond);
            GetPreviewSwitchCond.dwSize = dwSize;
            GetPreviewSwitchCond.byGroup = 0;
            GetPreviewSwitchCond.byVideoOutType = 1;
            GetPreviewSwitchCond.byGetDefaultPreviewSet = 0;
            GetPreviewSwitchCond.byPreviewNumber = 0;
            IntPtr ptrGetPreviewSwitchCond = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(GetPreviewSwitchCond, ptrGetPreviewSwitchCond, false);

            NET_DVR_PREVIEW_SWITCH_CFG GetPreviewSwitchCfg = new NET_DVR_PREVIEW_SWITCH_CFG();
            uint Size = (uint)Marshal.SizeOf(GetPreviewSwitchCfg);
            GetPreviewSwitchCfg.wSwitchSeq = new short[MAX_WINDOW_V40];
            GetPreviewSwitchCfg.byRes = new byte[32];

            IntPtr ptrGetPreviewSwitchCfg = Marshal.AllocHGlobal((Int32)Size);
            Marshal.StructureToPtr(GetPreviewSwitchCfg, ptrGetPreviewSwitchCfg, false);

            IntPtr ptrStatusList = Marshal.AllocHGlobal(4);

            if (!NET_DVR_GetDeviceConfig(userId, NET_DVR_GET_PREVIEW_SWITCH_CFG, 0, ptrGetPreviewSwitchCond, dwSize, ptrStatusList, ptrGetPreviewSwitchCfg, Size))
            {
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrStatusList);
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                uint code = NET_DVR_GetLastError();

                throw new Exception($"GetChannel Get Device Config error. code {code}");
            }
            else
            {
                GetPreviewSwitchCfg = (NET_DVR_PREVIEW_SWITCH_CFG)Marshal.PtrToStructure(ptrGetPreviewSwitchCfg, typeof(NET_DVR_PREVIEW_SWITCH_CFG));
            }

            NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40 = new NET_DVR_IPPARACFG_V40();
            dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);
            uint dwReturn = 0;
            int iGroupNo = 0; //该方法仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!NET_DVR_GetDVRConfig(userId, NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                uint code = NET_DVR_GetLastError();
                throw new Exception($"GetChannel Get DVR Config error. code {code}");
            }
            else
            {
                //获取通道成功
                m_struIpParaCfgV40 = (NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(NET_DVR_IPPARACFG_V40));
                for (int i = 0; i < m_struIpParaCfgV40.struIPDevInfo.Length; i++)
                {
                    switch (m_struIpParaCfgV40.struStreamMode[i].byGetStreamType)
                    {
                        case 0:
                            NET_DVR_IPDEVINFO_V31 device = m_struIpParaCfgV40.struIPDevInfo[i];
                            string struIp = Encoding.UTF8.GetString(device.struIP.sIpV4).TrimEnd('\0');
                            bool enable = Convert.ToBoolean(device.byEnable);

                            if (!string.IsNullOrEmpty(struIp) && enable)
                            {
                                int ch = (int)GetPreviewSwitchCfg.wSwitchSeq[i];
                                ipSwithChans.Add(new Tuple<string, int>(struIp, ch));
                            }
                            break;
                        default: break;
                    }
                }
            }

            Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
            Marshal.FreeHGlobal(ptrStatusList);
            Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
            Marshal.FreeHGlobal(ptrIpParaCfgV40);

            return ipSwithChans;
        }

        /// <summary>
        /// 切换预览画面
        /// </summary>
        /// <param name="layout">切换的画面格式 1 4 9</param>
        /// <param name="channels">通道列表</param>
        /// <param name="needSort">是否是按NVR中顺序播放</param>
        public static void Switch(CameraLayout layout, IEnumerable<int> channels, bool needSort = false)
        {
            List<int> newChannels = channels.ToList();

            NET_DVR_PREVIEW_SWITCH_COND GetPreviewSwitchCond = new NET_DVR_PREVIEW_SWITCH_COND();
            uint dwSize = (uint)Marshal.SizeOf(GetPreviewSwitchCond);
            GetPreviewSwitchCond.dwSize = dwSize;
            GetPreviewSwitchCond.byGroup = 0;
            GetPreviewSwitchCond.byVideoOutType = 1;
            GetPreviewSwitchCond.byGetDefaultPreviewSet = 0;
            GetPreviewSwitchCond.byPreviewNumber = 0;
            IntPtr ptrGetPreviewSwitchCond = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(GetPreviewSwitchCond, ptrGetPreviewSwitchCond, false);

            NET_DVR_PREVIEW_SWITCH_CFG GetPreviewSwitchCfg = new NET_DVR_PREVIEW_SWITCH_CFG();
            uint Size = (uint)Marshal.SizeOf(GetPreviewSwitchCfg);
            GetPreviewSwitchCfg.wSwitchSeq = new short[MAX_WINDOW_V40];
            GetPreviewSwitchCfg.byRes = new byte[32];

            IntPtr ptrGetPreviewSwitchCfg = Marshal.AllocHGlobal((Int32)Size);
            Marshal.StructureToPtr(GetPreviewSwitchCfg, ptrGetPreviewSwitchCfg, false);

            IntPtr ptrStatusList = Marshal.AllocHGlobal(4);

            if (!NET_DVR_GetDeviceConfig(userId, NET_DVR_GET_PREVIEW_SWITCH_CFG, 0, ptrGetPreviewSwitchCond, dwSize, ptrStatusList, ptrGetPreviewSwitchCfg, Size))
            {
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrStatusList);
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                uint code = NET_DVR_GetLastError();
                throw new Exception($"Get Device Config error. code {code}");
            }
            else
            {
                GetPreviewSwitchCfg = (NET_DVR_PREVIEW_SWITCH_CFG)Marshal.PtrToStructure(ptrGetPreviewSwitchCfg, typeof(NET_DVR_PREVIEW_SWITCH_CFG));

                NET_DVR_PREVIEW_SWITCH_COND SetPreviewSwitchCond = new NET_DVR_PREVIEW_SWITCH_COND();
                uint sdwSize = (uint)Marshal.SizeOf(SetPreviewSwitchCond);
                SetPreviewSwitchCond.dwSize = sdwSize;
                SetPreviewSwitchCond.byGroup = 0;
                SetPreviewSwitchCond.byVideoOutType = 1;
                SetPreviewSwitchCond.byGetDefaultPreviewSet = 1;
                SetPreviewSwitchCond.byPreviewNumber = (byte)layout;
                IntPtr ptrSetPreviewSwitchCond = Marshal.AllocHGlobal((int)sdwSize);
                Marshal.StructureToPtr(SetPreviewSwitchCond, ptrSetPreviewSwitchCond, false);

                NET_DVR_PREVIEW_SWITCH_CFG SetPreviewSwitchCfg = new NET_DVR_PREVIEW_SWITCH_CFG();
                uint SSize = (uint)Marshal.SizeOf(SetPreviewSwitchCfg);
                SetPreviewSwitchCfg.wSwitchSeq = new short[MAX_WINDOW_V40];

                for (int i = 0; i < MAX_WINDOW_V40; i++)
                {
                    //此处传入的channels必须在NVR中完全声明过，否则会报错
                    if (i < newChannels.Count)
                    {
                        SetPreviewSwitchCfg.wSwitchSeq[i] = needSort ? (short)(33 + i) : Convert.ToInt16(newChannels[i]);
                    }
                    else
                    {
                        SetPreviewSwitchCfg.wSwitchSeq[i] = -1;
                    }
                }

                SetPreviewSwitchCfg.dwSize = (int)SSize;
                SetPreviewSwitchCfg.byPreviewNumber = (byte)layout;
                SetPreviewSwitchCfg.byEnableAudio = 0;
                SetPreviewSwitchCfg.bySwitchTime = 0;
                SetPreviewSwitchCfg.bySameSource = 0;

                SetPreviewSwitchCfg.byRes = new byte[32];
                IntPtr ptrSetPreviewSwitchCfg = Marshal.AllocHGlobal((int)SSize);
                Marshal.StructureToPtr(SetPreviewSwitchCfg, ptrSetPreviewSwitchCfg, false);

                if (!NET_DVR_SetDeviceConfig(userId, NET_DVR_SET_PREVIEW_SWITCH_CFG, 0, ptrSetPreviewSwitchCond, sdwSize, ptrStatusList, ptrSetPreviewSwitchCfg, SSize))
                {
                    uint code = NET_DVR_GetLastError();
                    throw new Exception($"Set Preview Picture error. code {code}");
                }

                Marshal.FreeHGlobal(ptrGetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrStatusList);
                Marshal.FreeHGlobal(ptrGetPreviewSwitchCfg);
                Marshal.FreeHGlobal(ptrSetPreviewSwitchCond);
                Marshal.FreeHGlobal(ptrSetPreviewSwitchCfg);
            }
        }


        [DllImport(".\\HIK\\HCNetSDK.dll")]
        static extern bool NET_DVR_CaptureJPEGPicture_NEW(int userID, int channel, ref NET_DVR_JPEGPARA lpJpegPara, byte[] sJpegPicBuffer, uint dwPicSize, ref uint lpSizeReturned);

        [DllImport(@".\\HIK\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpOutBuffer, uint dwOutBufferSize);

        [DllImport(@".\\HIK\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpOutBuffer, uint dwOutBufferSize, ref uint lpBytesReturned);

        [DllImport(@".\\HIK\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpOutBuffer, uint dwOutBufferSize);

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_JPEGPARA
        {
            /*注意：当图像压缩分辨率为VGA时，支持0=CIF, 1=QCIF, 2=D1抓图，
            当分辨率为3=UXGA(1600x1200), 4=SVGA(800x600), 5=HD720p(1280x720),6=VGA,7=XVGA, 8=HD900p
            仅支持当前分辨率的抓图*/
            public ushort wPicSize;/* 0=CIF, 1=QCIF, 2=D1 3=UXGA(1600x1200), 4=SVGA(800x600), 5=HD720p(1280x720),6=VGA*/
            public ushort wPicQuality;/* 图片质量系数 0-最好 1-较好 2-一般 */
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_PREVIEW_SWITCH_COND
        {
            public uint dwSize; // 当前结构体大小
            public byte byGroup;//组号，0-63
            public byte byVideoOutType; //视频输出类型 1-HDMI 2-VGA
            public byte byGetDefaultPreviewSet; //是否预设 0，1 为1时byPreviewNumber有效
            public byte byPreviewNumber;//0-1画面,1-4画面,2-9画面,3-16画面, 4-6画面, 5-8画面6-25画面,7-32画面, 8-36画面0xff:最大画面 
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_PREVIEW_SWITCH_CFG
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_WINDOW_V40, ArraySubType = UnmanagedType.U4)]
            public short[] wSwitchSeq;
            public byte byPreviewNumber;
            public byte byEnableAudio;
            public byte bySwitchTime;
            public byte bySameSource;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

        }

        /*IP地址*/
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct NET_DVR_IPADDR
        {
            /// char[16]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
            public byte[] sIpV4;

            /// BYTE[128]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
            public byte[] sIpV6;

            public void Init()
            {
                sIpV4 = new byte[16];
                sIpV6 = new byte[128];
            }
        }

        /* V40扩展IP接入配置结构 */
        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_IPPARACFG_V40
        {
            public uint dwSize;/* 结构大小 */
            public uint dwGroupNum;
            public uint dwAChanNum;
            public uint dwDChanNum;
            public uint dwStartDChan;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CHANNUM_V30, ArraySubType = UnmanagedType.I1)]
            public byte[] byAnalogChanEnable; /* 模拟通道是否启用，从低到高表示1-32通道，0表示无效 1有效 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_IP_DEVICE_V40, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_IPDEVINFO_V31[] struIPDevInfo; /* IP设备 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CHANNUM_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_STREAM_MODE[] struStreamMode;  /* 取流模式 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2; /* 模拟通道是否启用，从低到高表示1-32通道，0表示无效 1有效 */
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_IPDEVINFO_V31
        {
            public byte byEnable;//该IP设备是否有效
            public byte byProType;
            public byte byEnableQuickAdd;
            public byte byRes1;//保留字段，置0
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sUserName;//用户名
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = PASSWD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sPassword;//密码
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DOMAIN_NAME, ArraySubType = UnmanagedType.I1)]
            public byte[] byDomain;//设备域名
            public NET_DVR_IPADDR struIP;//IP地址
            public ushort wDVRPort;// 端口号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;//保留字段，置0

            public void Init()
            {
                sUserName = new byte[NAME_LEN];
                sPassword = new byte[PASSWD_LEN];
                byDomain = new byte[MAX_DOMAIN_NAME];
                byRes2 = new byte[34];
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        struct NET_DVR_GET_STREAM_UNION
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 492, ArraySubType = UnmanagedType.I1)]
            public byte[] byUnion;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NET_DVR_STREAM_MODE
        {
            public byte byGetStreamType;/*取流方式：0- 直接从设备取流；1- 从流媒体取流；2- 通过IPServer获得IP地址后取流；
                                          * 3- 通过IPServer找到设备，再通过流媒体取设备的流； 4- 通过流媒体由URL去取流；
                                          * 5- 通过hiDDNS域名连接设备然后从设备取流 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
            public NET_DVR_GET_STREAM_UNION uGetStream;
            public void Init()
            {
                byGetStreamType = 0;
                byRes = new byte[3];
                //uGetStream.Init();
            }
        }



    }

    class RenderderWrapper : IDisposable
    {
        protected bool disposed;

        public event EventHandler OnDisposed;

        public RenderderWrapper(Image image)
        {
            Image = image;
            Render = new D3DImageSource();
            image.Dispatcher.Invoke(() => { image.Source = Render.ImageSource; });
        }

        public bool HasSetupSurface { get; set; }

        public bool Rendering { get; set; }

        public bool Closing { get; set; }

        public int Port { get; set; }

        public int RealHandle { get; set; }

        public Image Image { get; set; }

        public D3DImageSource Render { get; private set; }

        public HIKNVRClient.REALDATACALLBACK Callback { get; set; }

        public HIKNVRClient.DECCBFUN Decode { get; set; }

        ~RenderderWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // release managed resources...
                    Callback = null;
                    Decode = null;
                }
                // release unmanaged resources or big object...
                //Image.Dispatcher.Invoke(() =>
                //{
                //    //Render.Dispose();
                //});

                Render.Dispose();
                Image = null;
                Render = null;
                disposed = true;
                OnDisposed?.Invoke(this, new EventArgs());
            }
        }
    }

    class RenderderWrapperHelper
    {
        public static RenderderWrapper GetRenderder(DependencyObject obj)
        {
            return (RenderderWrapper)obj.GetValue(RenderderProperty);
        }

        public static void SetRenderder(DependencyObject obj, RenderderWrapper value)
        {
            obj.SetValue(RenderderProperty, value);
        }

        public static readonly DependencyProperty RenderderProperty =
            DependencyProperty.RegisterAttached("Renderder", typeof(RenderderWrapper), typeof(RenderderWrapperHelper), new PropertyMetadata(null));
    }
}
