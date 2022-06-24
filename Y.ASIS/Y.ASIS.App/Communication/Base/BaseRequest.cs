using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Y.ASIS.App.Helper;

namespace Y.ASIS.App.Communication
{
    public abstract class BaseRequest
    {
        protected RestClient client;

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
            IRestResponse response = response = client.Execute(request);
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
}
