using System;
using System.Drawing;
using System.IO;
using AlgorithmServer.Model;
using Nancy;
using OpenCvSharp;

namespace AlgorithmServer.Algorithm.DetectMethod
{
    public class StreamDetectMethod : IDetectMethod
    {
        public MethodParam GetMethodParam(Request request)
        {
            MethodParam param = new MethodParam();
            try
            {
                string url = request.Form.ToDictionary()["StreamUrl"];
                VideoCapture capture = new VideoCapture(url);
                Mat mat = new Mat();
                capture.Read(mat);
                using (MemoryStream stream = mat.ToMemoryStream())
                {
                    param.Mat = Mat.FromStream(stream, ImreadModes.AnyColor);
                    param.Image = Image.FromStream(stream);
                }
                capture.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return param;
        }
    }
}
