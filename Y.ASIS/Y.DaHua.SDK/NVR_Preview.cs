using NetSDKCS;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
namespace Y.DaHua.NVR
{
    //预览调用，主要涉及3个外部调用函数
    //   RealPlay_Load()
    /// <summary>
    /// NVR预览调用，主要涉及3个外部调用函数，NVR_Load():登录NVR。Preview():打开预览 。StopPreview()：关闭预览。
    /// 注：打开预览后，必须关闭预览后才能开启下一次预览，否则会报错。打开预览后，不能马上关闭预览，至少需要等1s再关闭。
    /// </summary>
    [Obsolete("弃用")]
    public class NVR_Preview
    {
        //public NVR_Preview Preview1;
        private const int m_WaitTime = 5000;
        private const int SyncFileSize = 5 * 1024 * 1204;
        private static fDisConnectCallBack m_DisConnectCallBack;
        private static fHaveReConnectCallBack m_ReConnectCallBack;
        private static fRealDataCallBackEx2 m_RealDataCallBackEx2;
        private static fSnapRevCallBack m_SnapRevCallBack;

        private IntPtr m_LoginID = IntPtr.Zero;
        private NET_DEVICEINFO_Ex m_DeviceInfo;
        private IntPtr m_RealPlayID = IntPtr.Zero;
        //private uint m_SnapSerialNum = 1;
        //private int SpeedValue = 4;
        private const int MaxSpeed = 8;
        private const int MinSpeed = 1;
        /// <summary>
        /// 初始化及登录NVR
        /// </summary>
        public void NVR_Load(string Ip = "192.168.1.117", ushort Port = 37777, string UserName = "admin", string Password = "yunda123")
        {
            //Preview1=new NVR_Preview();
            m_DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
            m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
            m_RealDataCallBackEx2 = new fRealDataCallBackEx2(RealDataCallBackEx);
            m_SnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack);
            try
            {
                NETClient.Init(m_DisConnectCallBack, IntPtr.Zero, null);
                NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero);
                NETClient.SetSnapRevCallBack(m_SnapRevCallBack, IntPtr.Zero);
                Login(Ip, Port, UserName, Password);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            Console.WriteLine("PreView(实时预览) --- Offline(离线)");
            //this.BeginInvoke((Action)UpdateDisConnectUI);
        }
        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {

            Console.WriteLine("PreView(实时预览) --- Online(在线)");

        }
        private void RealDataCallBackEx(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr param, IntPtr dwUser)
        {

            //do something such as save data,send data,change to YUV. 比如保存数据，发送数据，转成YUV等.

        }
        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)//远程抓图回调
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "capture";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (EncodeType == 10) //.jpg
            {
                DateTime now = DateTime.Now;
                string fileName = "async" + CmdSerial.ToString() + ".jpg";
                string filePath = path + "\\" + fileName;
                byte[] data = new byte[RevLen];
                Marshal.Copy(pBuf, data, 0, (int)RevLen);
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    stream.Write(data, 0, (int)RevLen);
                    stream.Flush();
                    stream.Dispose();
                }
            }
        }
        /// <summary>
        /// 登录NVR设备
        /// </summary>
        private void Login(string Ip = "192.168.1.117", ushort Port = 37777, string UserName = "admin", string Password = "yunda123")
        {
            if (IntPtr.Zero == m_LoginID)
            {
                m_DeviceInfo = new NET_DEVICEINFO_Ex();
                m_LoginID = NETClient.LoginWithHighLevelSecurity(Ip, Port, UserName, Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref m_DeviceInfo);//需写配置文件
                if (IntPtr.Zero == m_LoginID)
                {
                    Debug.Print(NETClient.GetLastError());
                    return;
                }
                else
                {

                    Console.WriteLine("高级别登录成功");
                }
                //LoginUI();//登录后界面更改
            }
            else
            {
                bool result = NETClient.Logout(m_LoginID);
                if (!result)
                {
                    Debug.Print(NETClient.GetLastError());
                    return;
                }
                m_LoginID = IntPtr.Zero;
                //InitOrLogoutUI();
            }
        }

        public bool IsPreviewing()
        {
            return m_RealPlayID != IntPtr.Zero;
        }

        public EM_RealPlayType CurrentPlayType { get; private set; }

        public void PreviewTimeout(NetSDKCS.EM_RealPlayType type, int channel, int Timeout)
        {
            Preview(type, channel);
            System.Threading.Thread.Sleep(Timeout);
            StopPreview();
        }

        /// <summary>
        /// 打开预览显示
        /// </summary>
        /// <param name="PlayType"> 监视类型,调用NetSDKCS.EM_RealPlayType.选择类型</param>
        /// <param name="StartChannle">起始通道</param>
        public void Preview(EM_RealPlayType PlayType, int StartChannle)//切换预览
        {
            if (IntPtr.Zero == m_RealPlayID)
            {
                CurrentPlayType = PlayType;
                m_RealPlayID = NETClient.RealPlay(m_LoginID, StartChannle, IntPtr.Zero, PlayType);
                if (IntPtr.Zero == m_RealPlayID)
                {
                    Console.WriteLine("预览打开失败");
                    Console.WriteLine(NETClient.GetLastError());
                    return;
                }
                else
                {

                    Console.WriteLine("预览打开成功");
                    Console.WriteLine(DateTime.Now);
                }
                NETClient.SetRealDataCallBack(m_RealPlayID, m_RealDataCallBackEx2, IntPtr.Zero, EM_REALDATA_FLAG.DATA_WITH_FRAME_INFO | EM_REALDATA_FLAG.PCM_AUDIO_DATA | EM_REALDATA_FLAG.RAW_DATA | EM_REALDATA_FLAG.YUV_DATA);
            }
            else
            {
                Console.WriteLine("错误，RealPlayID不为零，请先关闭预览");
            }
        }
        /// <summary>
        /// 关闭预览
        /// </summary>
        public void StopPreview()
        {
            if (m_RealPlayID != IntPtr.Zero)
            {
                // stop realplay 关闭监视
                bool ret = NETClient.StopRealPlay(m_RealPlayID);
                if (!ret)
                {
                    Console.WriteLine("关闭预览失败！");
                    Console.WriteLine(NETClient.GetLastError());

                    return;
                }
                else
                {
                    Console.WriteLine("关闭预览成功！");
                }
                m_RealPlayID = IntPtr.Zero;
            }
            else
            {
                Console.WriteLine("已关闭预览！");
            }
        }
    }
}
