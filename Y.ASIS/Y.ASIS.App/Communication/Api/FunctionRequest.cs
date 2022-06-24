using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Communication
{
    class FunctionListRequest : AuthRequest
    {
        private const string Path = "/api/functions";

        public FunctionListRequest()
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
