using RestSharp;
using Y.ASIS.App.Models;

namespace Y.ASIS.App.Communication.Api
{
    class HandleWarningRequest : AuthRequest
    {
        private const string Path = "/api/warning/handle";

        private readonly HandleWarningParams parameter;
        public HandleWarningRequest(int warningId, int userId, string remarks) :
            base(Path)
        {
            parameter = new HandleWarningParams()
            {
                WarningId = warningId,
                UserId = userId,
                Remarks = remarks
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.Timeout = 5000;
            request.AddJsonBody(parameter);
            request.AddHeader("ContentType", "application/json;charset=utf-8");
            return request;
        }
    }

    class GetUnhandleWarningsCountRequest : AuthRequest
    {
        private const string Path = "/api/warnings/unhandle/count";

        public GetUnhandleWarningsCountRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.GET;
            return request;
        }
    }
}
