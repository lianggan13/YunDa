using System.Drawing;
using AlgorithmServer.Common;
using AlgorithmServer.Model;
using Nancy;

namespace AlgorithmServer.Algorithm.DetectMethod
{
    public class ImageDetectMethod : IDetectMethod
    {
        public MethodParam GetMethodParam(Request request)
        {
            string base64 = request.Form.ToDictionary()["Image"];
            MethodParam param = new MethodParam();
            Bitmap image = ImageUtil.Base64StringToImage(base64);
            param.Mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
            param.Image = image;
            return param;
        }
    }
}
