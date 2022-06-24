using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Utils
{
    public class ByteUtil
    {
        public static bool[] ToBinary(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0').Select(i => i == '1').ToArray();
        }

        public static byte ToByte(string binaryString)
        {
            if (binaryString == null
                || binaryString.Length <= 0
                || binaryString.Length > 8
                || binaryString.Any(i => i != '0' && i != '1'))
            {
                throw new ArgumentException();
            }
            return Convert.ToByte(binaryString, 2);
        }
    }
}
