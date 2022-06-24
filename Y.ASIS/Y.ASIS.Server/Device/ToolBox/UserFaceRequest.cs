using RestSharp;

namespace Y.ASIS.Server.Device.ToolBox
{
    /// <summary>
    /// 用户信息新增、变更时，需要将用户信息推送给工具柜管理系统
    /// </summary>
    class UserFaceUploadRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/user/uploadFace";
        private object userFace;

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
        public UserFaceUploadRequest(string userId, string faceStr)
            : base(Path)
        {
            userFace = new
            {
                userId = userId,
                faceStr = $"image/png;base64,{faceStr}",
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            request.AddJsonBody(userFace);
            return request;
        }
    }
}
