using System;
using System.Collections.Generic;
using RestSharp;

namespace Y.ASIS.Server.Device.ToolBox
{
    /// <summary>
    /// 人员和工具的授权(绑定)
    /// </summary>
    class ToolUserSaveRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/toolsUser/save/batch";
        private object toolUsers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toolsId">工具 id</param>
        /// <param name="userIds">人员 id 列表</param>
        public ToolUserSaveRequest(string toolsId, string[] userIds)
            : base(Path)
        {
            toolUsers = new
            {
                toolsId = toolsId,
                userIds = userIds,
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            //toolsId
            request.AddJsonBody(toolUsers);
            return request;
        }
    }

    /// <summary>
    /// 人员和工具的销权(解绑)
    /// </summary>
    class ToolUserDeleteRequest : ToolBaseRequest
    {
        private const string Path = "/KeyServer/app/toolsUser/delete/batch";
        object idParis; //= new List<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPairs">待删除 Id 列表</param>
        public ToolUserDeleteRequest(List<Tuple<string, string>> idPairs)
            : base(Path)
        {
            List<object> list = new List<object>();
            foreach (var p in idPairs)
            {
                list.Add(new { toolsId = p.Item1, userId = p.Item2 });
            }

            this.idParis = new
            {
                ids = list
            };
        }

        protected override RestRequest CreateRequest()
        {
            RestRequest request = base.CreateRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json;charset=UTF-8");
            request.RequestFormat = DataFormat.Json;
            //var json = JsonConvert.SerializeObject(idParis);
            //request.AddParameter("ids", json, ParameterType.RequestBody);
            request.AddJsonBody(idParis);
            var ss = request.Resource;
            return request;
        }
    }

}

