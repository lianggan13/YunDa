using RestSharp;

namespace Y.ASIS.App.Communication.Algorithm
{
    class AlgorithmVersion
    {
        public const string Cloth = nameof(Cloth);
        public const string Train = nameof(Train);
        public const string Safety = nameof(Safety);
        public const string Personnel = nameof(Personnel);
    }

    class AlgorithmVersionRequest : AlgorithmBaseRequest
    {
        private const string Path = "/version/{name}";

        private readonly string Name;

        public AlgorithmVersionRequest(string name)
            : base(Path)
        {
            Name = name;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            request.AddHeader("Authkey", Authkey);
            request.AddUrlSegment("name", Name);
            return request;
        }
    }
}
