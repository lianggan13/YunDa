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
    public class PersonNumAlgorithm : Algorithm
    {
        private const string DllPath = "PersonDetect.dll";

        //[DllImport(DllPath, EntryPoint = "ModelInit", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [DllImport(DllPath, EntryPoint = "Init", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Init();

        //[DllImport(DllPath, EntryPoint = "Detect", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        [DllImport(DllPath, EntryPoint = "Recognition", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Detect(IntPtr ptr, string json);

        static PersonNumAlgorithm()
        {
            Init();
        }

        public override RecognizeResult Detect(Request request)
        {
            var param = GetDetectParameter(request);
            string extensionParams = request.Form.ToDictionary()["ExtendedParameters"];

            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(extensionParams);
            BoxInfo boxInfo = new BoxInfo(values);
            // TODO: 暂时默认 1(有人)，等后期算法组更新测试通过后，再调用
            int result = 1;// Detect(param.Mat.CvPtr, boxInfo.ToString());
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

        class BoxInfo
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
