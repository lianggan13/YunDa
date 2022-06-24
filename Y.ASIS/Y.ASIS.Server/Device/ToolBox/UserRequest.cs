using RestSharp;

namespace Y.ASIS.Server.Device.ToolBox
{
    /// <summary>
    /// 用户信息新增、变更时，需要将用户信息推送给工具柜管理系统
    /// </summary>
    class UserAddRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/user/save";
        private object user;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">用户id(数据库表中对应 remote_id)</param>
        /// <param name="username">用户名(其实是工号)</param>
        /// <param name="nickname">中文姓名</param>
        /// <param name="status">状态:0正常,1禁用</param>
        /// <param name="staffCode">员工卡卡号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">电话</param>
        public UserAddRequest(string userId, string username, string nickname, string status = "1", string staffCode = "", string idcard = "", string email = "", string mobile = "")
            : base(Path)
        {
            user = new
            {
                userId = userId,
                username = username,
                nickname = nickname,
                status = status,
                staffCode = staffCode,
                idcard = idcard,
                email = email,
                mobile = mobile,
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            request.AddJsonBody(user);
            return request;
        }
    }

    /// <summary>
    /// 用户信息删除时，需要将用户信息推送给工具柜管理系统
    /// </summary>
    class UserDeleteRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/user/delete/batch";
        string[] ids;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">待删除 Id 列表</param>
        public UserDeleteRequest(params string[] ids)
            : base(Path)
        {
            this.ids = ids;
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            //request.AddHeader("Content-Type", "multipart/form-data");

            string pvalue = "{" + string.Join(",", ids) + "}";
            request.AddParameter("ids", pvalue);
            var ss = request.Resource;
            return request;
        }
    }
}
