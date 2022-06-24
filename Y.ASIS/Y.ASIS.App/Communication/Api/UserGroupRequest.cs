using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;

namespace Y.ASIS.App.Communication
{
    class UserGroupListRequest : AuthRequest
    {
        private const string Path = "/api/groups";

        public UserGroupListRequest()
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

    class AddOrUpdateUserGroupRequest : AuthRequest
    {
        private const string Path = "/api/group";

        private readonly UserGroup userGroup;
        public AddOrUpdateUserGroupRequest(UserGroup userGroup)
            : base(Path)
        {
            this.userGroup = userGroup;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json", userGroup.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class DeleteUserGroupsRequest : AuthRequest
    {
        private const string Path = "/api/groups/delete";

        private readonly IEnumerable<int> ids;
        public DeleteUserGroupsRequest(IEnumerable<int> ids)
            : base(Path)
        {
            this.ids = ids;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("GroupIds", string.Join("|", ids));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            return request;
        }
    }
}
