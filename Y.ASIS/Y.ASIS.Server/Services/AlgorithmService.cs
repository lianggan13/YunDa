using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Services
{
    public class AlgorithmService
    {
        private static AlgorithmService instance;

        public static AlgorithmService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AlgorithmService();
                }
                return instance;
            }
        }

        public bool First = true;

        private AlgorithmService()
        {

        }

        public string DetectTrainNo(int id, int channel)
        {
            string no = "";
            Track track = DataProvider.Instance.GetTrackByPosId(id);

            AlgorithmDetectRequest request = new AlgorithmDetectRequest(DetectType.Train, MethodType.SDK, channel);
            #region Request Async
            //request.RequestAsync<ResponseData<DetectData>>(resp =>
            //{
            //    if (resp != null && resp.IsSuccess)
            //    {
            //        no = $"{resp.Data.Result}";
            //        TrainNumberRecord record = new TrainNumberRecord()
            //        {
            //            TrackId = track.No,
            //            TrainNumber = no,
            //            Time = DateTime.Now
            //        };
            //        DataProvider.Instance.AddTrainNumberRecord(record);
            //    }
            //}, exp => LogHelper.Error(exp.Message, exp)); 
            #endregion
            var response = request.Request<ResponseData<DetectData>>();
            if (response != null && response.IsSuccess)
            {
                no = $"{response.Data.Result}";
                TrainNumberRecord record = new TrainNumberRecord()
                {
                    TrackId = track.No,
                    TrainNumber = no,
                    Time = DateTime.Now
                };
                DataProvider.Instance.AddTrainNumberRecord(record);
            }

            return no;
        }
    }

    class RecognizeResult
    {
        public int Result { get; set; }

        public string Photo { get; set; }
    }

    #region 算法接口，暂放于此，后期封装
    public class DetectData
    {
        public string Result { get; set; }
        public string Photo { get; set; }
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

    class AlgorithmDetectRequest : AlgorithmRequest
    {
        private const string Path = "/api/detect";

        private readonly string Authkey;

        private readonly Dictionary<string, object> reqParams;

        public AlgorithmDetectRequest(string detect, string method, int channel = default, string streamUrl = "", string image = "", string extendedParams = "")
            : base(Path)
        {
            Authkey = LocalConfigManager.GetAppSettingValue("Algorithm.Server.AuthKey");
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
            request.Method = Method.POST;
            //request.Timeout = 10000;
            request.AddHeader("Authkey", Authkey);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var key in reqParams.Keys)
            {
                request.AddParameter(key, reqParams[key]);
            }
            return request;
        }
    }

    abstract class AlgorithmRequest : BaseRequest
    {
        private readonly static string baseUrl;

        static AlgorithmRequest()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("Algorithm.Server.Host");
        }

        public AlgorithmRequest(string path)
            : base(baseUrl, path)
        {

        }
    }

    abstract class BaseRequest
    {
        private RestClient client;

        protected IDictionary<string, string> headers;
        protected IDictionary<string, string> cookies;
        protected int timeout = 3 * 1000;

        private string url;
        public string Url
        {
            get { return url; }
            private set
            {
                url = value;
                if (client == null)
                {
                    RestSharpHelper.Instance.GetRestRequest(Url, out client);
                }
            }
        }

        public BaseRequest(string baseUrl, string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            Url = baseUrl + path;
            headers = new Dictionary<string, string>();
            cookies = new Dictionary<string, string>();
        }

        protected virtual RestRequest CreateRequest()
        {
            RestRequest request = new RestRequest(Url);
            if (headers.Any())
            {
                foreach (KeyValuePair<string, string> pair in headers)
                {
                    request.AddHeader(pair.Key, pair.Value);
                }
            }
            if (cookies.Any())
            {
                foreach (KeyValuePair<string, string> pair in cookies)
                {
                    request.AddCookie(pair.Key, pair.Value);
                }
            }
            return request;
        }

        public T Request<T>()
        {
            RestRequest request = CreateRequest();
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                string json = response.Content;
                try
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
            return default;
        }

        public void RequestAsync<T>(Action<T> callback, Action<Exception> exceptionHandler = null)
        {
            RestRequest request = CreateRequest();
            Task.Factory.StartNew(() =>
            {
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    string json = response.Content;
                    try
                    {
                        T data = JsonConvert.DeserializeObject<T>(json);
                        callback?.Invoke(data);
                    }
                    catch (Exception e)
                    {
                        exceptionHandler?.Invoke(e);
                    }
                }
            });
        }
    }

    #endregion
}
