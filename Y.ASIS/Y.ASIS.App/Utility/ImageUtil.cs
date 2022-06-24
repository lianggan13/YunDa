using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Y.ASIS.App.Utils
{
    class ImageUtil
    {


        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap ImageOriginalBase = new Bitmap(bitmap);
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                ImageOriginalBase.Save(ms, ImageFormat.Png);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            Bitmap bmp = new Bitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new Rectangle(System.Drawing.Point.Empty, bmp.Size),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            bitmapImage.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static string BitmapToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        public static string BitmapImageToBase64String(BitmapImage image)
        {
            try
            {
                Bitmap bmp = BitmapImageToBitmap(image);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage Base64ToImage(string base64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return BitmapToBitmapImage(bmp);
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage BufferToImage(byte[] buffer)
        {
            try
            {
                MemoryStream ms = new MemoryStream(buffer);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return BitmapToBitmapImage(bmp);
            }
            catch
            {
                return null;
            }
        }


        public static void SaveImage(byte[] data, string path)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                Bitmap bmp = new Bitmap(stream);
                bmp.Save(path, ImageFormat.Png);
            }
        }

        public static void SaveImage(BitmapSource bitmapImage, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static string BytesToBase64String(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}
