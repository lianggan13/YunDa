using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y.ASIS.App.Communication
{
    class LoginRequest : AuthRequest
    {
        private const string Path = "/api/login";

        private readonly int no;
        private readonly string password;
        public LoginRequest(int no, string password)
            : base(Path)
        {
            this.no = no;
            this.password = password;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("No", no);
            request.AddParameter("Password", password);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            return request;
        }
    }
}
