using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System;
using System.Drawing;
using System.IO;

public static class ImageHelper
{
    public static string SaveImage(byte[] data, string name)
    {
        string path = "";
        using (MemoryStream stream = new MemoryStream(data))
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(dir, "Log", $"{name}");
            Bitmap bmp = new Bitmap(stream);

            bmp.Save(path, ImageFormat.Png);
        }
        return path;
    }


    /// <summary>
    /// base64转图片
    /// </summary>
    /// <param name="dataURL"></param>
    /// <param name="imgName"></param>
    /// <returns></returns>
    public static Bitmap Base64StringToImage(string base64)
    {
        base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
        byte[] bytes = Convert.FromBase64String(base64);
        MemoryStream memStream = new MemoryStream(bytes);
        Bitmap mImage = (Bitmap)Image.FromStream(memStream);
        return mImage;
    }

    /// <summary>
    /// Image 转成 base64
    /// </summary>
    /// <param name="fileFullName"></param>
    public static string ImageToBase64(Image img)
    {
        try
        {
            Bitmap bmp = new Bitmap(img);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            return Convert.ToBase64String(arr);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}
