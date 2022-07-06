using AlgorithmServer.Algorithm.DetectMethod;
using AlgorithmServer.Model;
using Nancy;

namespace AlgorithmServer.Algorithm
{
    public abstract class Algorithm
    {
        protected IDetectMethod detect;

        public void SetDetectMethod(Methods methods)
        {
            switch (methods)
            {
                case Methods.SDK:
                    detect = new SdkDetectMethod();
                    break;
                case Methods.Stream:
                    detect = new StreamDetectMethod();
                    break;
                case Methods.Image:
                    detect = new ImageDetectMethod();
                    break;
            }
        }

        public virtual MethodParam GetDetectParameter(Request request)
        {
            return detect.GetMethodParam(request);
        }

        public abstract RecognizeResult Detect(Request request);

        protected string GetVersion(string dllPath)
        {
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(dllPath).FileVersion;
        }

        public abstract string GetVersion();
    }
}
