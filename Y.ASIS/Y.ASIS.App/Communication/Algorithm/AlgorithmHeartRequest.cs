using RestSharp;

namespace Y.ASIS.App.Communication.Algorithm
{
    class AlgorithmHeartRequest : AlgorithmBaseRequest
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
    }
}
