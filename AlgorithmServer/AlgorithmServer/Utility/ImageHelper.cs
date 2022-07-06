using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;


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


}
