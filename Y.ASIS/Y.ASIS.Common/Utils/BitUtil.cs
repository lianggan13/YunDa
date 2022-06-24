using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Utils
{
    /// <summary>
    /// 位运算工具类
    /// </summary>
    public class BitUtil
    {
        /// <summary>
        /// 获取一个字节某一位的值是否为1
        /// </summary>
        /// <param name="value">给定字节</param>
        /// <param name="index">要获取的位 范围0-7</param>
        /// <returns>该字节指定位的值是否为1</returns>
        public static bool GetBit(byte value, int index)
        {
            if (index < 0 || index > 7)
            {
                throw new ArgumentOutOfRangeException();
            }
            return ((value >> index) & 1) == 1;
        }

        /// <summary>
        /// 设置一个字节的某一位的值
        /// </summary>
        /// <param name="value">给定字节</param>
        /// <param name="index">要设置的位 范围0-7</param>
        /// <param name="flag">设置的值</param>
        /// <returns>设置后的值</returns>
        public static byte SetBit(byte value, int index, bool flag)
        {
            if (index < 0 || index > 7)
            {
                throw new ArgumentOutOfRangeException();
            }
            int i = index < 1 ? 1 : (2 << index - 1);
            return flag ? (byte)(value | i) : (byte)(value & ~i);
        }
    }
}
