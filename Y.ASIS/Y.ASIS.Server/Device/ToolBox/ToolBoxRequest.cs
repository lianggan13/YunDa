using RestSharp;

namespace Y.ASIS.Server.Device.ToolBox
{
    /// <summary>
    /// 运达平台建立柜子信息，并将柜子信息推送给工具柜
    /// </summary>
    class ToolBoxSaveRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/cabinet/save";
        private object cabinet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">柜子 id</param>
        /// <param name="name">柜子名称</param>
        /// <param name="code">柜子编号</param>
        /// <param name="status">状态0正常1禁用</param>
        public ToolBoxSaveRequest(string id, string name, string code, string status)
            : base(Path)
        {
            cabinet = new
            {
                id = id,
                name = name,
                code = code,
                status = status,
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            request.AddJsonBody(cabinet);
            return request;
        }
    }

    /// <summary>
    /// 运达平台删除柜子信息，并将柜子信息推送给工具柜
    /// </summary>
    class ToolBoxDeleteRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/cabinet/delete";
        string[] ids;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">待删除 Id 列表</param>
        public ToolBoxDeleteRequest(params string[] ids)
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
