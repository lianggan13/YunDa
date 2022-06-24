using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.Manager;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Device.Speaker
{
    class SpeakerRequest
    {
        private static readonly string baseUrl;

        static SpeakerRequest()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("SpeakerServerBaseUrl");
        }

        private RestClient client;



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

        private readonly int timeout;
        private readonly IEnumerable<KeyValuePair<string, string>> parameters;

        public SpeakerRequest(string path, IEnumerable<KeyValuePair<string, string>> parameters, int timeout = 3 * 1000)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            Url = baseUrl + path;
            this.parameters = parameters;
            this.timeout = timeout;
        }

        public T Request<T>()
        {
            RestRequest request = new RestRequest(Url)
            {
                Method = Method.POST,
                Timeout = 3 * 1000
            };
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (parameters.Any())
            {
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    request.AddParameter(pair.Key, pair.Value);
                }
            }
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                string json = response.Content;
                try
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }
    }
}
