using System;
using System.Runtime.InteropServices;

namespace AlgorithmServer
{

    class HIKNVRClient
    {
        private static int userId;

        static string address = LocalConfigManager.GetAppSettingValue("HK.Address");
        static int port = Convert.ToInt32(LocalConfigManager.GetAppSettingValue("Port.Address"));
        static string username = LocalConfigManager.GetAppSettingValue("User.Address");
        static string password = LocalConfigManager.GetAppSettingValue("Pwd.Address");


        public static void Init()
        {
            bool success = CHCNetSDK.NET_DVR_Init();
            if (!success)
            {
                int code = (int)CHCNetSDK.NET_DVR_GetLastError();
                throw new Exception("HK sdk init error. Error code:" + code);
            }
            Console.WriteLine("HK sdk init success.");
            Login();
        }

        private static void Login()
        {
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = default;
            userId = CHCNetSDK.NET_DVR_Login_V30(address, port, username, password, ref deviceInfo);
            if (userId < 0)
            {
                throw new Exception("NVR login error.");
            }
            Console.WriteLine("NVR login success.");
        }

        public static byte[] CaptureCache(int channel)
        {
            CHCNetSDK.NET_DVR_JPEGPARA jpeg = new CHCNetSDK.NET_DVR_JPEGPARA();
            jpeg.wPicQuality = 0;
            jpeg.wPicSize = 0xff;
            uint bufferSize = 800000;
            byte[] buffer = new byte[bufferSize];
            uint readSize = 0;
            bool success = CHCNetSDK.NET_DVR_CaptureJPEGPicture_NEW(userId, channel, ref jpeg, buffer, bufferSize, ref readSize);
            if (!success)
            {
                throw new Exception("Capture error.");
            }
            return buffer;
        }
    }

    /// <summary>
    /// CHCNetSDK 的摘要说明。
    /// </summary>
    public class CHCNetSDK
    {
        public CHCNetSDK()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_Init();

        [DllImport(@"HCNetSDK.dll")]
        public static extern uint NET_DVR_GetLastError();

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_JPEGPARA
        {
            /*注意：当图像压缩分辨率为VGA时，支持0=CIF, 1=QCIF, 2=D1抓图，
            当分辨率为3=UXGA(1600x1200), 4=SVGA(800x600), 5=HD720p(1280x720),6=VGA,7=XVGA, 8=HD900p
            仅支持当前分辨率的抓图*/
            public ushort wPicSize;/* 0=CIF, 1=QCIF, 2=D1 3=UXGA(1600x1200), 4=SVGA(800x600), 5=HD720p(1280x720),6=VGA*/
            public ushort wPicQuality;/* 图片质量系数 0-最好 1-较好 2-一般 */
        }

        //JPEG抓图到内存
        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, ref NET_DVR_JPEGPARA lpJpegPara, byte[] sJpegPicBuffer, uint dwPicSize, ref uint lpSizeReturned);

        //NET_DVR_Login_V30()参数结构
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_DEVICEINFO_V30
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.I1)]
            public byte[] sSerialNumber;  //序列号
            public byte byAlarmInPortNum;               //报警输入个数
            public byte byAlarmOutPortNum;              //报警输出个数
            public byte byDiskNum;                  //硬盘个数
            public byte byDVRType;                  //设备类型, 1:DVR 2:ATM DVR 3:DVS ......
            public byte byChanNum;                  //模拟通道个数
            public byte byStartChan;                    //起始通道号,例如DVS-1,DVR - 1
            public byte byAudioChanNum;                //语音通道数
            public byte byIPChanNum;                    //最大数字通道个数，低位  
            public byte byZeroChanNum;          //零通道编码个数 //2010-01-16
            public byte byMainProto;            //主码流传输协议类型 0-private, 1-rtsp,2-同时支持private和rtsp
            public byte bySubProto;             //子码流传输协议类型0-private, 1-rtsp,2-同时支持private和rtsp
            public byte bySupport;        //能力，位与结果为0表示不支持，1表示支持，
                                          //bySupport & 0x1, 表示是否支持智能搜索
                                          //bySupport & 0x2, 表示是否支持备份
                                          //bySupport & 0x4, 表示是否支持压缩参数能力获取
                                          //bySupport & 0x8, 表示是否支持多网卡
                                          //bySupport & 0x10, 表示支持远程SADP
                                          //bySupport & 0x20, 表示支持Raid卡功能
                                          //bySupport & 0x40, 表示支持IPSAN 目录查找
                                          //bySupport & 0x80, 表示支持rtp over rtsp
            public byte bySupport1;        // 能力集扩充，位与结果为0表示不支持，1表示支持
                                           //bySupport1 & 0x1, 表示是否支持snmp v30
                                           //bySupport1 & 0x2, 支持区分回放和下载
                                           //bySupport1 & 0x4, 是否支持布防优先级	
                                           //bySupport1 & 0x8, 智能设备是否支持布防时间段扩展
                                           //bySupport1 & 0x10, 表示是否支持多磁盘数（超过33个）
                                           //bySupport1 & 0x20, 表示是否支持rtsp over http	
                                           //bySupport1 & 0x80, 表示是否支持车牌新报警信息2012-9-28, 且还表示是否支持NET_DVR_IPPARACFG_V40结构体
            public byte bySupport2; /*能力，位与结果为0表示不支持，非0表示支持							
							bySupport2 & 0x1, 表示解码器是否支持通过URL取流解码
							bySupport2 & 0x2,  表示支持FTPV40
							bySupport2 & 0x4,  表示支持ANR
							bySupport2 & 0x8,  表示支持CCD的通道参数配置
							bySupport2 & 0x10,  表示支持布防报警回传信息（仅支持抓拍机报警 新老报警结构）
							bySupport2 & 0x20,  表示是否支持单独获取设备状态子项
							bySupport2 & 0x40,  表示是否是码流加密设备*/
            public ushort wDevType;              //设备型号
            public byte bySupport3; //能力集扩展，位与结果为0表示不支持，1表示支持
                                    //bySupport3 & 0x1, 表示是否多码流
                                    // bySupport3 & 0x4 表示支持按组配置， 具体包含 通道图像参数、报警输入参数、IP报警输入、输出接入参数、
                                    // 用户参数、设备工作状态、JPEG抓图、定时和时间抓图、硬盘盘组管理 
                                    //bySupport3 & 0x8为1 表示支持使用TCP预览、UDP预览、多播预览中的"延时预览"字段来请求延时预览（后续都将使用这种方式请求延时预览）。而当bySupport3 & 0x8为0时，将使用 "私有延时预览"协议。
                                    //bySupport3 & 0x10 表示支持"获取报警主机主要状态（V40）"。
                                    //bySupport3 & 0x20 表示是否支持通过DDNS域名解析取流

            public byte byMultiStreamProto;//是否支持多码流,按位表示,0-不支持,1-支持,bit1-码流3,bit2-码流4,bit7-主码流，bit-8子码流
            public byte byStartDChan;       //起始数字通道号,0表示无效
            public byte byStartDTalkChan;   //起始数字对讲通道号，区别于模拟对讲通道号，0表示无效
            public byte byHighDChanNum;     //数字通道个数，高位
            public byte bySupport4;
            public byte byLanguageType;// 支持语种能力,按位表示,每一位0-不支持,1-支持  
                                       //  byLanguageType 等于0 表示 老设备
                                       //  byLanguageType & 0x1表示支持中文
                                       //  byLanguageType & 0x2表示支持英文
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;       //保留
        }

        [DllImport(@"HCNetSDK.dll")]
        public static extern Int32 NET_DVR_Login_V30(string sDVRIP, Int32 wDVRPort, string sUserName, string sPassword, ref NET_DVR_DEVICEINFO_V30 lpDeviceInfo);
    }
}
