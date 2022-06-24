using RestSharp;
using System.Collections.Generic;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Communication
{
    class RoleListRequest : AuthRequest
    {
        private const string Path = "/api/roles";

        public RoleListRequest()
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

    class RoleRequest : AuthRequest
    {
        private const string Path = "/api/role/{id}";

        private readonly int id;
        public RoleRequest(int id)
            : base(Path)
        {
            this.id = id;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Timeout = 5000;
            request.Method = Method.GET;
            request.AddUrlSegment("id", id);
            return request;
        }
    }

    class AddOrUpdateRoleRequest : AuthRequest
    {
        private const string Path = "/api/role";


        private readonly Role role;
        public AddOrUpdateRoleRequest(Role role)
            : base(Path)
        {
            this.role = role;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json", role.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class DeleteRolesRequest : AuthRequest
    {
        private const string Path = "/api/roles/delete";

        private readonly IEnumerable<int> ids;
        public DeleteRolesRequest(IEnumerable<int> ids)
            : base(Path)
        {
            this.ids = ids;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("RoleIds", string.Join("|", ids));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            return request;
        }
    }
}
