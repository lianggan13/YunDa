using RestSharp;

namespace Y.ASIS.App.Communication
{
    class TrackListRequest : AuthRequest
    {
        private const string Path = "/api/tracks";

        public TrackListRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Timeout = 10 * 1000;
            request.Method = Method.GET;
            return request;
        }
    }
}
