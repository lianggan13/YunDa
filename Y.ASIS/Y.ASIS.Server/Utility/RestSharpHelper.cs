using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Y.ASIS.Server.Utility
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
            lock (this)
            {
                if (!clients.ContainsKey(baseUrl))
                {
                    clients[baseUrl] = new RestClient(baseUrl);
                }
                client = clients[baseUrl];
            }
            return new RestRequest(url);
        }

        public bool RegisterClient(string url)
        {
            Match match = regex.Match(url);
            if (!match.Success)
            {
                return false;
            }
            string baseUrl = match.Value;

            RestClient client = new RestClient(baseUrl);
            clients[baseUrl] = client;

            return true;
        }
    }
}
