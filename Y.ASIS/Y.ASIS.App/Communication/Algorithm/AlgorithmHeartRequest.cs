using RestSharp;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.Communication.Algorithm
{
    public class AlgorithmHeartRequest : AlgorithmBaseRequest
    {
        private const string Path = "/api/heart";

        public AlgorithmHeartRequest()
            : base(Path)
        {
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.Timeout = 5000;
            request.AddHeader("Authkey", Authkey);
            return request;
        }

        public static bool Ping()
        {
            AlgorithmHeartRequest algorithmHeartRequest = new AlgorithmHeartRequest();
            var algorithmresp = algorithmHeartRequest.Request<ResponseData<object>>();
            return algorithmresp != null;
        }
    }
}
