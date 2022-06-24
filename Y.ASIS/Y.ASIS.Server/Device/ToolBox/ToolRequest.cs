using RestSharp;

namespace Y.ASIS.Server.Device.ToolBox
{
    /// <summary>
    /// 工具信息新增、修改、删除时，需要将工具信息推送给工具柜
    /// </summary>
    class ToolSaveRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/tools/save";
        private object tool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">工具 id</param>
        /// <param name="code">工具编号</param>
        /// <param name="type">工具类型</param>
        /// <param name="rfid">标签号</param>
        /// <param name="cabinetId">柜子 id</param>
        public ToolSaveRequest(string id, string code, string type, string rfid, string cabinetId)
            : base(Path)
        {
            tool = new
            {
                id = id,
                code = code,
                type = type,
                rfid = rfid,
                cabinetId = cabinetId,
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            request.AddJsonBody(tool);
            return request;
        }
    }

    /// <summary>
    /// 工具信息删除时，需要将工具信息推送给工具柜
    /// </summary>
    class ToolDeleteRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/tools/delete/batch";
        string[] ids;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">待删除 Id 列表</param>
        public ToolDeleteRequest(params string[] ids)
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
