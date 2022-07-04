using AlgorithmServer.Model;
using Nancy;
using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;

namespace AlgorithmServer.Algorithm.DetectMethod
{
    public class SdkDetectMethod : IDetectMethod
    {
        public MethodParam GetMethodParam(Request request)
        {
            MethodParam param = new MethodParam();
            try
            {
                string channel = request.Form.ToDictionary()["Channel"];
                byte[] data = HIKNVRClient.CaptureCache(Convert.ToInt32(channel));

                string path = ImageHelper.SaveImage(data, $"Channel_{channel}.png");
                Console.WriteLine(path);

                using (MemoryStream stream = new MemoryStream(data))
                {
                    param.Mat = Mat.FromStream(stream, ImreadModes.AnyColor);
                    param.Image = Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return param;
        }
    }
}
