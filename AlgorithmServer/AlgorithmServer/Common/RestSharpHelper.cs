using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgorithmServer
{
    public class RestSharpHelper
    {
        private static RestSharpHelper instance;

        private readonly Regex regex;
        private readonly Dictionary<string, RestClient> clients;

        private RestSharpHelper()
        {
            regex = new Regex(@"(http:\/\/[^ \/]+)");
            clients = new Dictionary<string, RestClient>();
        }

        public static RestSharpHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RestSharpHelper();
                }
                return instance;
            }
        }

        public RestRequest GetRestRequest(string url, out RestClient client)
        {
            Match match = regex.Match(url);
            if (!match.Success)
            {
                throw new Exception("Can't get baseUrl from: " + url);
            }
            string baseUrl = match.Value;
            if (!clients.ContainsKey(baseUrl))
            {
                clients[baseUrl] = new RestClient(baseUrl);
            }
            client = clients[baseUrl];
            return new RestRequest(url);
        }
    }
}
