using System;
using System.Runtime.InteropServices;
using AlgorithmServer.Common;
using AlgorithmServer.Model;
using Nancy;

namespace AlgorithmServer.Algorithm.DetectAlgorithm
{
    class ClothCheckAlgorithm : Algorithm
    {
        //private const string DllPath = "diaoyong.dll";

        // TODO: GetVersion dll 获取无效
        private const string DllPath = "cloth_detect_yolo.dll";

        [DllImport(DllPath, EntryPoint = "Init", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void Init();

        [DllImport(DllPath, EntryPoint = "Recognition", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Recognition(IntPtr ptr);

        static ClothCheckAlgorithm()
        {
            Init();
        }

        public override RecognizeResult Detect(Request request)
        {
            var param = GetDetectParameter(request);
            int result = Recognition(param.Mat.CvPtr);
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
    }
}
