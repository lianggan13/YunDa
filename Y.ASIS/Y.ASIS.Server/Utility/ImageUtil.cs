using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Y.ASIS.Common.Utils;
using Y.ASIS.Server.Common;

namespace Y.ASIS.Server.Utility
{
    class ImageUtil
    {
        readonly static CascadeClassifier classifier;

        public readonly static string DefaultImage;


        static ImageUtil()
        {
            DefaultImage = ServerGlobal.PhotoUrlPrefix + "DEFAULT.png";

            CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;

            string fronttalfacefile = Path.Combine(ServerGlobal.ExecuteDirectory, "Config", "haarcascade_frontalface_alt.xml");
            classifier = new CascadeClassifier(fronttalfacefile);
        }

        public static Image Base64StringToImage(string base64)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    return Image.FromStream(ms, true);
                }
            }
            catch
            {
                return null;
            }
        }

        public static string Base64StringToUrl(string base64)
        {
            if (base64 == null)
            {
                return null;
            }
            string md5 = SecurityUtil.MD5FromString(base64);
            string name = md5 + ".png";
            string path = Path.Combine(ServerGlobal.PhotoDirectory, name);
            string url = ServerGlobal.PhotoUrlPrefix + name;
            if (File.Exists(path))
            {
                return url;
            }
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    using (Image image = Image.FromStream(ms, false))
                    {
                        image.Save(path);
                    }
                }
                return url;
            }
            catch
            {
                return null;
            }
        }

        public static bool TryFindFaceFromBase64String(string base64, out string faceString)
        {
            if (base64.EndsWith("Internal"))
            {
                faceString = base64.Substring(0, base64.Length - 8);
                return true;
            }
            faceString = null;
            try
            {
                Mat mat = new Mat();
                byte[] imageBytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Bitmap bitmap = (Bitmap)Image.FromStream(ms);
                    Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap);
                    CvInvoke.BitwiseAnd(image, image, mat);
                    Rectangle[] faces = classifier.DetectMultiScale(mat, 1.1, 10, new System.Drawing.Size(200, 200));
                    if (faces.Length != 1)
                    {
                        return false;
                    }
                    Rectangle rect = faces[0];
                    int offsetY = rect.Height / 3;
                    rect.Y = rect.Y > offsetY ? rect.Y - offsetY : 0;
                    rect.Height = rect.Y > offsetY ? rect.Height + offsetY : rect.Height + rect.Y;
                    rect.Height = rect.Height + rect.Y + offsetY > mat.Height ? mat.Height - rect.Y : rect.Height + offsetY;

                    Mat roi = new Mat(mat, rect);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        roi.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] arr = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(arr, 0, (int)stream.Length);
                        stream.Close();
                        faceString = Convert.ToBase64String(arr);
                        return true;
                    }
                }
            }
            catch
            {
                faceString = null;
                return false;
            }
        }

        public static string BytesToBase64String(byte[] data)
        {
            return Convert.ToBase64String(data);
        }



        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// 从bitmap转换成ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;

        }

        public static Bitmap GetBitmap(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return new Bitmap(result);
        }
    }
}
