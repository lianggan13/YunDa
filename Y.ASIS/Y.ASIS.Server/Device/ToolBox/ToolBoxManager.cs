using Nancy;
using Nancy.Hosting.Self;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Models;
using Y.ASIS.Server.ToolBox.BLL;

namespace Y.ASIS.Server.Device.ToolBox
{
    public class ToolBoxManager
    {


        private NancyHost listener;
        private ToolBoxDbService service;   // 工具柜数据库服务
        public static ToolBoxManager Instance { get; } = new ToolBoxManager();
        public List<Position> Poistions { get; } = new List<Position>();
        public Dictionary<string, bool> ToolStates { get; } = new Dictionary<string, bool>();


        private bool running;

        public bool Running
        {
            get { return running; }
            set
            {
                if (running != value)
                {
                    running = value;
                    if (running)
                    {
                        StartListen();
                    }
                    else
                    {
                        StopListen();
                    }
                }
            }
        }


        public void Register(Position pos)
        {
            Poistions.Add(pos);
        }

        public ToolBoxManager()
        {
            service = new ToolBoxDbService();
        }

        private void StartListen()
        {
            string address = LocalConfigManager.GetAppSettingValue("ToolBox.Server.Slave");
            Url url = new Url(address);
            HostConfiguration hostConfiguration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations()
                {
                    CreateAutomatically = true
                }
            };
            try
            {
                listener = new NancyHost(hostConfiguration, url);
                listener.Start();
                LogHelper.Info("ToolBox.Server.Slave is listening on " + url);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ToolBox.Server.Slave listening fail", ex);
                throw ex;
            }
        }

        private void StopListen()
        {
            listener.Stop();
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <param name="staffCode"></param>
        /// <param name="faceStr"></param>
        /// <returns></returns>
        public bool AddUser(string userId, string username, string nickname, string staffCode)
        {
            UserAddRequest userAdd = new UserAddRequest(userId, username, nickname, status: "1", staffCode: staffCode);
            var response = userAdd.Request<ToolResponseData>();
            return response?.Success == true;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="workers"></param>
        /// <returns></returns>
        public bool DeleteUser(ISet<int> workers)
        {
            var ids = workers.Select(w => w.ToString()).ToArray();
            return DeleteUser(ids);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteUser(params string[] ids)
        {
            UserDeleteRequest request = new UserDeleteRequest(ids);
            return request.Request<ToolResponseData>()?.Success == true;
        }

        /// <summary>
        /// 授权多个人员
        /// </summary>
        /// <param name="workersSet"></param>
        /// <returns></returns>
        public bool AuthorizeUser(ISet<User> workers)
        {
            if (!running || workers.Count == 0)
            {
                return true;
            }
            bool success = false;
            // 工具柜 授权
            foreach (var user in workers)
            {
                // TODO:可能需要等待延时 发送请求 
                success = AuthorizeUser(userId: user.No.ToString(),
                                        username: user.No.ToString(),
                                        nickname: user.Name,
                                        staffCode: user.CardNo?.ToString(),
                                        faceStr: user.Photo);
                // TDOD:暂时让异常抛出,授权失败该怎么处理
                if (!success)
                {
                    throw new Exception($"User No: {user.No} 工具柜授权失败!");
                }
            }
            return success;
        }

        /// <summary>
        /// 授权单个人员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AuthorizeUser(string userId)
        {
            UserAddRequest userAdd = new UserAddRequest(userId, username: "", nickname: "", status: "0");
            return userAdd.Request<ToolResponseData>()?.Success == true;
        }

        /// <summary>
        /// 授权单个人员(同时同步人员)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="nickname"></param>
        /// <param name="staffCode"></param>
        /// <param name="faceStr"></param>
        /// <returns></returns>
        public bool AuthorizeUser(string userId, string username, string nickname, string staffCode, string faceStr)
        {
            // 请求: 同步人员
            UserAddRequest userAdd = new UserAddRequest(userId, username, nickname, status: "0", staffCode);
            var response = userAdd.Request<ToolResponseData>();
            if (response == null || !response.Success)
            {
                return false;
            }

            // 检测人脸是否存在
            if (service.UserFaceHasExsit(userId))
            {
                return true;   // 直接返回
            }

            // 请求: 同步人脸
            UserFaceUploadRequest userFaceUpload = new UserFaceUploadRequest(userId, faceStr);
            response = userFaceUpload.Request<ToolResponseData>();
            if (response == null || !response.Success)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 销权多个人员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ProhibitKey(ISet<int> workers)
        {
            if (!running || workers.Count == 0)
            {
                return true;
            }
            var ids = workers.Select(w => w.ToString()).ToArray();
            bool success = false;
            foreach (var id in ids)
            {
                success = ProhibitUser(id);
                // TDOD:暂时让异常抛出
                if (!success)
                {
                    throw new Exception($"User No: {id} 工具柜销权失败!");
                }
            }
            return success;
        }

        /// <summary>
        /// 销权
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ProhibitUser(string userId)
        {
            UserAddRequest userAdd = new UserAddRequest(userId, username: "", nickname: "", status: "1");
            return userAdd.Request<ToolResponseData>()?.Success == true;
        }
    }


    public class ToolModule : NancyModule
    {
        public ToolModule()
        {
            Post("/KeyServer/app/tools/bindRfid", GetBindRfid);
            Post("/KeyServer/app//tools/bindRfid", GetBindRfid);
        }

        /// <summary>
        /// 工具-标签绑定(工具出入柜时,工具柜上报的请求)
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Response GetBindRfid(dynamic _)
        {
            var body = GetRequestBody<ToolBindRfidRequestData>();
            // toolId --> tool --> poistions
            int toolId = int.Parse(body.ToolsId);
            var tool = DataProvider.Instance.ToolList.FirstOrDefault(t => t.ID == toolId);
            var positions = ToolBoxManager.Instance.Poistions.Where(p => tool.PositionIndexes.Contains(p.Index));
            //  positionIndex --> track --> record
            int posIndex = tool.PositionIndexes.First();
            var track = DataProvider.Instance.TrackList.First(t => t.Positions.Any(p => p.Index == posIndex));
            var record = DataProvider.Instance.GetToolAuthRecord(track.ID, int.Parse(body.UserId));


            if (record == null)
            {
                //return ResponseData.Error(ResponseCode.ParameterIsNullOrMissing).ToString();
                return ResponseData.Error(ResponseCode.Success).ToString();
            }

            ToolBoxManager.Instance.ToolStates[body.ToolsId] = body.Return;
            var toolCount = ToolBoxManager.Instance.ToolStates.Values.Where(v => !v).Count().ToString() ?? "0";

            // 更新 Position.State.Tool
            foreach (var pos in positions)
            {
                pos.State.Tool = int.Parse(toolCount);
            }

            // 更新 Tool Record
            if (body.Return)
            {
                // 入柜(归还)
                tool.Using = false;
                record.Return();
            }
            else
            {
                tool.Using = true;
                if (!record.ToolIds.Contains(toolId))
                {
                    record.ToolIds.Add(toolId);
                }
                record.Take();
            }

            DataProvider.Instance.AddOrUpdateTool(tool);
            DataProvider.Instance.AddOrUpdateToolAuthRecord(record);

            return ResponseData.Success(nameof(GetBindRfid)).ToString();
        }

        public T GetRequestBody<T>()
        {
            byte[] data = new byte[Request.Body.Length];
            Request.Body.Read(data, 0, data.Length);
            string json = Encoding.Default.GetString(data);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    class ToolResponseData
    {
        public string Message { get; set; }
        public string ApiName { get; set; }
        public int Code { get; set; }
        public bool Success
        {
            get => Code == 200 && Message == "成功";
        }
    }


    class ToolBindRfidRequestData
    {
        public string Remark { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public string ToolsId { get; set; }

        public bool Return
        {
            get => Type == "归还";
        }
    }
}
