using RestSharp;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.Communication
{
    public class HeartRequest : AuthRequest
    {
        private const string Path = "/api/heart";

        public HeartRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            //request.Timeout = 5000;
            request.Method = Method.GET;

            return request;
        }

        public static bool Ping()
        {
            HeartRequest request = new HeartRequest();
            var resp = request.Request<ResponseData<object>>();
            return resp?.IsSuccess == true;
        }
    }
}
