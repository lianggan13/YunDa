using AlgorithmServer.Common;
using AlgorithmServer.Model;
using Nancy;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace AlgorithmServer.Algorithm.DetectAlgorithm
{
    class SafetyChainAlgorithm : Algorithm
    {
        //private const string DllPath = "anquanliansuo_yolov3.dll";
        private const string DllPath = "yolov5_tensorrt.dll";

        [DllImport(DllPath, EntryPoint = "Init", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void Init();

        [DllImport(DllPath, EntryPoint = "Recognition", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Recognition(IntPtr ptr, string json);

        static SafetyChainAlgorithm()
        {
            Init();
        }

        public override RecognizeResult Detect(Request request)
        {
            var param = GetDetectParameter(request);
            string extensionParams = request.Form.ToDictionary()["ExtendedParameters"];

            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(extensionParams);
            BoxInfo boxInfo = new BoxInfo(values);
            int result = Recognition(param.Mat.CvPtr, boxInfo.ToString());
            RecognizeResult res = new RecognizeResult()
            {
                Result = result.ToString(),
                Photo = ImageUtil.ImageToBase64(param.Image).Trim(),
            };
            return res;
        }

        public override string GetVersion()
        {
            return base.GetVersion(DllPath);
        }
        public class BoxInfo
        {
            public BoxInfo(IEnumerable<Point> points)
            {
                Name = "";
                Num = points.Count();
                X = points.Select(i => i.X);
                Y = points.Select(i => i.Y);
            }

            public string Name { get; set; }

            public int Num { get; set; }

            public IEnumerable<int> X { get; private set; }

            public IEnumerable<int> Y { get; private set; }

            public override string ToString()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }
    }
}
