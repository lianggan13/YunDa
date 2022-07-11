
using AlgorithmServer.Model;
using Nancy;
using System;
using System.Runtime.InteropServices;

namespace AlgorithmServer.Algorithm.DetectAlgorithm
{
    public class TrainCheckAlgorithm : Algorithm
    {
        //private const string DllPath = "orc_system.dll";
        private const string DllPath = "ocr_system.dll";

        [DllImport(DllPath, EntryPoint = "Init", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void Init();

        [DllImport(DllPath, EntryPoint = "detect", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr Recognize(IntPtr ptr);

        static TrainCheckAlgorithm()
        {
            Init();
        }

        public override RecognizeResult Detect(Request request)
        {
            var param = GetDetectParameter(request);
            var result = Recognize(param.Mat.CvPtr);
            RecognizeResult res = new RecognizeResult()
            {
                Result = Marshal.PtrToStringAnsi(result),
                Photo = ImageHelper.ImageToBase64(param.Image).Trim(),
            };
            return res;
        }

        public override string GetVersion()
        {
            return base.GetVersion(DllPath);
        }
    }
}
