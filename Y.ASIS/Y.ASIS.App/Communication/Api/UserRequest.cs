using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;

namespace Y.ASIS.App.Communication
{
    class UserListRequest : AuthRequest
    {
        private const string Path = "/api/users";

        public UserListRequest()
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

    class UpdateUserPhotoRequest : AuthRequest
    {
        private const string Path = "/api/user/photo";

        private readonly UpdateUserPhotoParams parameter;
        public UpdateUserPhotoRequest(int userId, string photoString)
            : base(Path)
        {
            parameter = new UpdateUserPhotoParams()
            {
                UserId = userId,
                PhotoString = photoString
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json;charset=utf-8", parameter.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class AddOrUpdateUserRequest : AuthRequest
    {
        private const string Path = "/api/user";

        private readonly User user;
        public AddOrUpdateUserRequest(User user)
            : base(Path)
        {
            this.user = user;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("application/json;charset=utf-8", user.JsonSerialize(), ParameterType.RequestBody);
            return request;
        }
    }

    class DeleteUsersRequest : AuthRequest
    {
        private const string Path = "/api/users/delete";

        private readonly IEnumerable<int> userIds;
        public DeleteUsersRequest(IEnumerable<int> userIds)
            : base(Path)
        {
            this.userIds = userIds;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddParameter("UserIds", string.Join("|", userIds));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            return request;
        }
    }
}
