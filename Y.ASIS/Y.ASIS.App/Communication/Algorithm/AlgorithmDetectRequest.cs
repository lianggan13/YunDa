using RestSharp;
using System.Collections.Generic;

namespace Y.ASIS.App.Communication.Algorithm
{
    class AlgorithmDetectRequest : AlgorithmBaseRequest
    {
        private const string Path = "/api/detect";

        private readonly Dictionary<string, object> reqParams;

        public AlgorithmDetectRequest(string detect, string method, int channel = default, string streamUrl = "", string image = "", string extendedParams = "")
            : base(Path)
        {
            reqParams = new Dictionary<string, object>();
            #region Check Parameter
            switch (detect)
            {
                case DetectType.Personnel:
                    break;
                case DetectType.Safety:
                    break;
                default:
                    break;
            }

            switch (method)
            {
                case MethodType.SDK:
                    break;
                case MethodType.Stream:
                    break;
                case MethodType.Image:
                    break;
                default:
                    break;
            }
            #endregion
            reqParams["Detect"] = detect;
            reqParams["Method"] = method;
            reqParams["Channel"] = channel;
            reqParams["StreamUrl"] = streamUrl;
            reqParams["Image"] = image;
            reqParams["ExtendedParameters"] = extendedParams;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Timeout = 6000;
            request.Method = Method.POST;
            request.AddHeader("Authkey", Authkey);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var key in reqParams.Keys)
            {
                request.AddParameter(key, reqParams[key]);
            }
            return request;
        }
    }


    class DetectType
    {
        public const string Cloth = nameof(Cloth);
        public const string Train = nameof(Train);
        public const string Safety = nameof(Safety);
        public const string Personnel = nameof(Personnel);
    }

    class MethodType
    {
        public const string SDK = nameof(SDK);
        public const string Stream = nameof(Stream);
        public const string Image = nameof(Image);
    }

    /// <summary>
    /// 算法识别结果
    /// </summary>
    public class DetectResult
    {
        public string Result { get; set; }
        public string Photo { get; set; }
    }
}
