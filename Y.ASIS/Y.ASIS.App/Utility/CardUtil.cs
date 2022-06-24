using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Y.ASIS.App.Utils
{
    public class CardUtil
    {
        #region Dll
        /// <summary>
        /// 打开HID设备
        /// </summary>
        /// <param name="device">[OUT] 存放读写器设备对象地址，后面的函数基本都需要用到</param>
        /// <param name="index"></param>
        /// <param name="vid">要打开设备的Vendor ID，为0x0416</param>
        /// <param name="pid">要打开设备的Product ID，为0x8020</param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_Open")]
        private static extern int SysOpen(ref IntPtr device, UInt32 index, UInt16 vid, UInt16 pid);

        /// <summary>
        /// 查询设备是否已经打开
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_IsOpen")]
        private static extern bool SysIsOpen(IntPtr device);

        /// <summary>
        /// 查询设备是否已经打开
        /// </summary>
        /// <param name="device">要操作的设备，由Sys_Open()获得</param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_Close")]
        private static extern int SysClose(ref IntPtr device);

        /// <summary>
        /// 设置设备天线状态
        /// </summary>
        /// <param name="device">要操作的设备，由Sys_Open()获得</param>
        /// <param name="mode">天线状态, 0 = 关闭天线, 1 = 开启天线</param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_SetAntenna")]
        private static extern int SysSetAntenna(IntPtr device, byte mode);

        /// <summary>
        /// 设置读写器非接触工作方式
        /// </summary>
        /// <param name="device">要操作的设备，由Sys_Open()获得</param>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_InitType")]
        private static extern int SysInitType(IntPtr device, byte type);

        /// <summary>
        /// 设置发卡器蜂鸣器蜂鸣时长
        /// </summary>
        /// <param name="device"></param>
        /// <param name="msec">时长，单位ms</param>
        /// <returns></returns>
        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "Sys_SetBuzzer")]
        private static extern int SysSetBuzzer(IntPtr device, byte msec);

        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "TyA_Request")]
        private static extern int TyARequest(IntPtr device, byte mode, ref UInt16 pTagType);

        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "TyA_Anticollision")]
        static extern int TyAAnticollision(IntPtr device,
                                            byte bcnt,
                                            byte[] pSnr,
                                            ref byte pLen);

        [DllImport("./Dlls/hfrdapi.dll", EntryPoint = "TyA_Select")]
        static extern int TyASelect(IntPtr device,
                                     byte[] pSnr,
                                     byte snrLen,
                                     ref byte pSak);
        #endregion

        #region Data

        private static IntPtr CardDevice = (IntPtr)(-1);
        private const byte Mode = 0x26;

        public enum ReturnCode
        {
            Success = 0,
            CloseFailed,
            OpenFailed,
            SetAntennaFailed,
            InitTypeFailed,
            SetBuzzerFailed,
            IsOpenFailed,
            TyARequestFailed,
            TyAAnticollisionFailed,
            TyASelectFailed
        }
        #endregion

        private static int Connect()
        {
            if (true == SysIsOpen(CardDevice))
            {

                if (0 != SysClose(ref CardDevice))
                {
                    return (int)ReturnCode.CloseFailed;
                }
            }

            if (0 != SysOpen(ref CardDevice, 0, 0x0416, 0x8020))
            {
                return (int)ReturnCode.OpenFailed;
            }

            if (0 != SysSetAntenna(CardDevice, 0))
            {
                return (int)ReturnCode.SetAntennaFailed;
            }
            Thread.Sleep(5);

            if (0 != SysInitType(CardDevice, (byte)'A'))
            {
                return (int)ReturnCode.InitTypeFailed;
            }
            Thread.Sleep(5);

            if (0 != SysSetAntenna(CardDevice, 1))
            {
                return (int)ReturnCode.SetAntennaFailed;
            }
            Thread.Sleep(5);

            if (0 != SysSetBuzzer(CardDevice, 20))
            {

                return (int)ReturnCode.SetBuzzerFailed;
            }

            return (int)ReturnCode.Success;
        }

        public static int GetCardUid()
        {

            ushort TagType = 0;
            byte bcnt = 0;
            byte[] dataBuffer = new byte[byte.MaxValue];
            byte len = 255;
            byte sak = 0;


            if (!SysIsOpen(CardDevice))
            {
                //return (int)ReturnCode.IsOpenFailed;
                int code = Connect();
                if (code != 0)
                {
                    return -1 * code;
                }
            }

            //搜寻所有的卡
            if (TyARequest(CardDevice, Mode, ref TagType) != 0)
            {
                return -1 * (int)ReturnCode.TyARequestFailed;
            }

            //返回卡的序列号
            if (TyAAnticollision(CardDevice, bcnt, dataBuffer, ref len) != 0)
            {

                return -1 * (int)ReturnCode.TyAAnticollisionFailed;
            }

            //锁定一张ISO14443-3 TYPE_A 卡
            if (TyASelect(CardDevice, dataBuffer, len, ref sak) != 0)
            {

                return -1 * (int)ReturnCode.TyASelectFailed;
            }

            if (0 != SysSetBuzzer(CardDevice, 20))
            {

                return -1 * (int)ReturnCode.SetBuzzerFailed;
            }
            return BitConverter.ToInt32(dataBuffer, 0);
        }

    }
}
