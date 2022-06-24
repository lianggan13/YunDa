using System.IO;
using System.IO.Compression;
using System.Text;

namespace Y.ASIS.Common.Utils
{
    /// <summary>
    /// 压缩工具类
    /// </summary>
    public class CompressUtil
    {
        public static byte[] Compress(string s, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] data = encoding.GetBytes(s);

            using (MemoryStream compressStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Compress, true))
                {

                    deflateStream.Write(data, 0, data.Length);
                }
                return compressStream.ToArray();
            }
        }

        public static string Decompress(byte[] data, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            using (MemoryStream compressStream = new MemoryStream(data))
            {
                using (DeflateStream deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
                {
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        deflateStream.CopyTo(resultStream);
                        byte[] result = resultStream.ToArray();
                        return encoding.GetString(result);
                    }
                }
            }
        }
    }
}
