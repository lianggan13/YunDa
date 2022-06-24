using RestSharp;

namespace Y.ASIS.App.Communication.Algorithm
{
    class ServiceVersionRequest : AlgorithmBaseRequest
    {
        private const string Path = "/api/version";

        public ServiceVersionRequest()
            : base(Path)
        {
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddHeader("Authkey", Authkey);
            return request;
        }
    }
}
