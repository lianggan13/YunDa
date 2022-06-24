using RestSharp;
using Y.ASIS.App.Common;

namespace Y.ASIS.App.Communication.Api
{
    class ProjectRequest : AuthRequest
    {
        private const string Path = "/api/project";

        public ProjectRequest()
            : base(Path)
        {

        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.Timeout = 5000;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Project", AppGlobal.Instance.Project.ToString());
            return request;
        }
    }
}
