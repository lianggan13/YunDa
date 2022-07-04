using MongoDB.Driver;
using Nancy;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Common;
using Y.ASIS.Server.Database;
using Y.ASIS.Server.Device;
using Y.ASIS.Server.Device.Attendance;
using Y.ASIS.Server.Device.Speaker;
using Y.ASIS.Server.Device.ToolBox;
using Y.ASIS.Server.MainThread.Components;
using Y.ASIS.Server.MainThread.Exceptions;
using Y.ASIS.Server.Services.Main;
using Response = Nancy.Response;

namespace Y.ASIS.Server.Services
{
    public class NancyService : NancyModule
    {
        private const string AuthKey = nameof(AuthKey);

        public NancyService()
        {
            Before += BeforeCall;
            After += AfterCall;
            OnError += ThrownException;

            Get("/api/heart", Heart);

            Get("/api/functions", GetFunctions);

            Get("/api/roles", GetRoles);
            Get("/api/role/{id}", GetRole);
            Post("/api/role", AddOrUpdateRole);
            Post("/api/roles/delete", DeleteRole);

            Get("/api/interfaces", GetExteranlSystems);
            Post("/api/interfaces", AddOrUpdateExternalSystem);
            Delete("/api/interfaces/{id}", DeleteExternalSystem);

            Post("/api/login", Login);

            Get("/api/user/photo/{name}", GetUserPhoto);
            Get("/api/users", GetUsers);
            Post("/api/user", AddOrUpdateUser);
            Post("/api/user/upsert", UpsertUser);
            Post("/api/user/photo", UploadUserPhoto);
            Post("/api/users/delete", DeleteUsers);

            Get("/api/groups", GetUserGroups);
            Post("/api/group", AddOrUpdateUserGroup);
            Post("/api/groups/delete", DeleteUserGroups);

            Get("/api/tracks", GetTracks);
            Post("/api/tracks", AddOrUpdateTrack);
            Delete("/api/tracks/{id}", DeleteTrack);

            Get("/api/position/switch/{index}", PositionSwitch);
            Get("/api/position/signal/switch/{positionId}/{index}/{command}", PositionSignalLightSwitch);
            Get("/api/position/gate/switch/{positionId}/{index}/{command}", PositionGateSwitch);
            Get("/api/position/command/{positionId}/{command}/{no}", PositionCommand);
            Get("/api/position/apply/{positionId}/{apply}/{no}", PositionApply);
            Get("/api/position/reset/{positionId}/{command}/{no}", PositionReset);
            Get("/api/position/confirm/{positionId}/{command}/{no}", PositionConfirm);
            Post("/api/position/confirm/algorithm", PosAlgorithmConfirm);

            Post("/api/position/issue/user", PositionIssueUser);
            Post("/api/position/revoke/user", PositionRevokeUser);
            Get("/api/position/issued/workers/{positionId}", GetIssuedWorkers);
            Get("/api/position/issued/operators/{positionId}", GetIssuedOperators);

            Post("/api/query/issue/users", QueryUserAuthRecords);
            Post("/api/query/faultsAndwarnings", QueryFaultsAndWarnings);
            Post("/api/query/operations", QueryOperations);
            Post("/api/query/issue/tools", QueryTools);
            Post("/api/query/trainnumbers", QueryTrainNumbers);

            Post("/api/warning/handle", HandleWarning);
            Get("/api/warnings/unhandle/count", GetUnhandleWarningsCount);
            Get("/api/tools/{id}", GetTools);

            Post("/api/project", Project);
            Post("/api/trackstate", GetTrackState);
        }

        #region common components (aop)
        private Response BeforeCall(NancyContext ctx)
        {
            if (!ctx.Request.Url.Path.EndsWith("/api/heart"))
                //Task.Run(() => LogHelper.Info($">> [{DateTime.Now}] {ctx.Request.UserHostAddress} {ctx.Request.Method} {ctx.Request.Url}"));
                Console.WriteLine($">> [{DateTime.Now}] {ctx.Request.UserHostAddress} {ctx.Request.Method} {ctx.Request.Url}");
            if (ctx.ResolvedRoute.Description.Path == "/api/user/photo/{name}")
            {
                return null;
            }
            if (!ctx.Request.Headers.Keys.Contains(AuthKey))
            {
                throw new AuthKeyNotFoundException();
            }
            ExternalSystem es = DataProvider.Instance.GetExternalSystem(ctx.Request.Headers[AuthKey].First());
            if (es == null)
            {
                throw new AuthKeyNotFoundException();
            }
            if (!es.Enable)
            {
                throw new AuthKeyIsDisableException();
            }
            es.LastTime = DateTime.Now;
            return null;
        }

        private void AfterCall(NancyContext ctx)
        {

        }

        private Response ThrownException(NancyContext ctx, Exception ex)
        {
            LogHelper.Error("Nancy Exception.", ex);
            if (ex is AuthKeyNotFoundException)
            {
                return ResponseData.Error(ResponseCode.AuthKeyNotFoundOrError).ToString();
            }
            else if (ex is AuthKeyIsDisableException)
            {
                return ResponseData.Error(ResponseCode.AuthKeyIsDisable).ToString();
            }
            else if (ex is ParameterIsNullOrMissingException parameterIsNullOrMissingException)
            {
                return ResponseData.Error(
                    ResponseCode.ParameterIsNullOrMissing,
                    $"parameter \'{parameterIsNullOrMissingException.ParameterName}\' is null or missing."
                    ).ToString();
            }
            else if (ex is ParameterDataFormatOrValueException parameterDataFormatOrValueException)
            {
                return ResponseData.Error(
                    ResponseCode.ParameterDataFormatOrValueError,
                    $"parameter \'{parameterDataFormatOrValueException.ParameterName}\' value \'{parameterDataFormatOrValueException.ParameterValue}\' data format or value error."
                    ).ToString();
            }
            else if (ex is ContentCastException contentCastException)
            {
                return ResponseData.Error(
                    ResponseCode.ContentCastError,
                    $"content \'{contentCastException.Content}\' can not cast to designative object."
                    ).ToString();
            }
            return ResponseData.Error(
                ResponseCode.UnhandleError,
                $"{ex.Message}"
                ).ToString();
        }
        #endregion

        private Response HandleWarning(dynamic _)
        {
            HandleWarningParams parameter = GetContent<HandleWarningParams>();
            DataProvider.Instance.HandleWarning(parameter.WarningId, parameter.UserId.ToString(), parameter.Remarks);
            return ResponseData.Success().ToString();
        }

        private Response GetUnhandleWarningsCount(dynamic _)
        {
            long count = DataProvider.Instance.GetUnhandleWarningsCount();
            return ResponseData.Success(count).ToString();
        }

        private Response GetTools(dynamic parameter)
        {
            int posIndex = parameter.id;  // 列位 唯一 Key
            return ResponseData.Success(DataProvider.Instance.ToolList.Where(i => i.PositionIndexes.Contains(posIndex))).ToString();
        }

        private Response GetUserGroups(dynamic _)
        {
            var userGroups = DataProvider.Instance.UserGroupList;
            return ResponseData.Success(userGroups).ToString();
        }

        private Response AddOrUpdateUserGroup(dynamic _)
        {
            UserGroup group = GetContent<UserGroup>();
            DataProvider.Instance.AddOrUpdateUserGroup(group);
            return ResponseData.Success().ToString();
        }

        private Response DeleteUserGroups(dynamic _)
        {
            string ids = GetForm("GroupIds");
            IEnumerable<int> list = ids.Split('|').Select(i => Convert.ToInt32(i));
            DataProvider.Instance.DeleteUserGroups(list);
            return ResponseData.Success().ToString();
        }

        private Response GetUserPhoto(dynamic parameters)
        {
            string name = parameters.name;
            string path = Path.Combine(ServerGlobal.PhotoDirectory, name);
            return Response.AsFile(path, "image/png");
        }

        private Response PositionIssueUser(dynamic _)
        {
            PositionIssueUsersParams @params = GetContent<PositionIssueUsersParams>();

            if (!PLCManager.Instance.GetPlcByPositionId(@params.PositionId, out PLC plc))
            {
                throw new ParameterDataFormatOrValueException("PositionId", @params.PositionId);
            }

            DataProvider provider = DataProvider.Instance;

            var pos = provider.GetPosition(@params.PositionId);
            if (!pos.State.Connected)
            {
                throw new Exception("PLC disconnected.");
            }

            IEnumerable<User> workers = @params.WorkerNos.Select(i =>
            {
                int no = Convert.ToInt32(i);
                User user = provider.UserList.FirstOrDefault(j => j.No == no);
                if (user == null)
                    throw new ParameterDataFormatOrValueException("WorkerNos", i);
                return user;
            });

            IEnumerable<User> operators = @params.OperatorNos.Select(i =>
            {
                int no = Convert.ToInt32(i);
                User user = provider.UserList.FirstOrDefault(j => j.No == no);
                if (user == null)
                    throw new ParameterDataFormatOrValueException("OperatorNos", i);
                return user;
            });

            ISet<User> workerSet = new HashSet<User>(workers);
            ISet<User> operatorSet = new HashSet<User>(operators);
            List<int> userNos = new List<int>();
            userNos.AddRange(workers.Select(u => u.No));
            userNos.AddRange(operators.Select(u => u.No));

            bool success1 = plc.IssueUsers(workerSet, operatorSet, @params.IsInspect);
            bool success2 = AttendanceManager.Instance.AddOrUpdateUsers(workerSet);
            bool success3 = ToolBoxManager.Instance.AuthorizeUser(workerSet);
            bool success = success1 && success2 && success3;

            if (success)
            {
                //DataProvider.Instance.UpdateUserAllowUpdateState(workersSet, operatorsSet, false);
                PositionService.AddPositionToUsers(pos.ID, userNos);
                DataProvider.Instance.AddOrUpdateUserAuthRecord(@params.PositionId, workerSet, operatorSet);
                DataProvider.Instance.AddToolAuthRecord(@params.PositionId, workerSet); // 工具柜授权后 新增一条记录
            }

            return ResponseData.Success(success).ToString();
        }

        private Response PositionRevokeUser(dynamic _)
        {
            PositionIssueUsersParams @params = GetContent<PositionIssueUsersParams>();

            if (!PLCManager.Instance.GetPlcByPositionId(@params.PositionId, out PLC plc))
            {
                throw new ParameterDataFormatOrValueException("PositionId", @params.PositionId);
            }

            DataProvider provider = DataProvider.Instance;

            var pos = provider.GetPosition(@params.PositionId);
            if (!pos.State.Connected)
            {
                throw new Exception("PLC disconnected.");
            }

            List<int> userNos = new List<int>();
            ISet<int> workerNos = new HashSet<int>(@params.WorkerNos);
            ISet<int> operatorNos = new HashSet<int>(@params.OperatorNos);
            userNos.AddRange(workerNos);
            userNos.AddRange(operatorNos);

            bool success1 = plc.RevokeUsers(workerNos, operatorNos);
            bool success2 = AttendanceManager.Instance.DeleteUsers(workerNos);
            bool success3 = ToolBoxManager.Instance.ProhibitKey(workerNos); //  ToolBoxManager.Instance.DeleteUser(workersSet);
            bool success = success1 && success2 && success3;

            IEnumerable<User> workers = @params.WorkerNos.Select(i =>
            {
                int no = Convert.ToInt32(i);
                User user = provider.UserList.FirstOrDefault(j => j.No == no);
                if (user == null)
                    throw new ParameterDataFormatOrValueException("WorkerNos", i);
                return user;
            });
            ISet<User> workerSet = new HashSet<User>(workers);

            IEnumerable<User> operators = @params.OperatorNos.Select(i =>
            {
                int no = Convert.ToInt32(i);
                User user = provider.UserList.FirstOrDefault(j => j.No == no);
                if (user == null)
                    throw new ParameterDataFormatOrValueException("OperatorNos", i);
                return user;
            });
            ISet<User> operatorSet = new HashSet<User>(operators);

            //DataProvider.Instance.UpdateUserAllowUpdateState(workerSet, operatorSet, true);
            PositionService.RemovePositionToUsers(pos.ID, userNos);

            List<UserAuthRecord> records = provider.GetUserAuthRecords(@params.PositionId, userNos);
            records.ForEach(i =>
            {
                i.Revoke();
                provider.AddOrUpdateUserAuthRecord(i);
            });

            List<ToolAuthRecord> toolRecords = provider.GetToolAuthRecords(@params.PositionId, userNos);
            toolRecords.ForEach(i =>
            {
                i.Revoke();
                provider.AddOrUpdateToolAuthRecord(i);
            });


            OperationRecord record = new OperationRecord()
            {
                Index = pos.Index,
                Time = DateTime.Now,
                TrackNo = DataProvider.Instance.GetTrackByPosId(pos.ID).No,
                WorkerNo = ServerGlobal.CurrentUser.No.ToString(),
                OperationCode = $"{(int)PLCOperateCode.手动消除权限}",
            };
            DataProvider.Instance.AddOrUpdateOperationRecord(record);

            return ResponseData.Success(success).ToString();
        }

        private Response GetIssuedWorkers(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }
            return ResponseData.Success(plc.GetIssuedWorkers()).ToString();
        }

        private Response GetIssuedOperators(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }
            return ResponseData.Success(plc.GetIssuedOperators()).ToString();
        }

        private Response PositionSwitch(dynamic parameters)
        {
            int index = parameters.index;
            bool success = SwitcherManager.Instance.Switch(index);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionSignalLightSwitch(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }
            int index = parameters.index;
            int command = parameters.command;
            if (index < 0 || index > 5)
            {
                throw new ParameterDataFormatOrValueException("index", index);
            }
            if (command != 101 && command != 102)
            {
                throw new ParameterDataFormatOrValueException("command", command);
            }
            success = plc.SetSignalLightCommand(index, command);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionGateSwitch(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }
            int index = parameters.index;
            int command = parameters.command;
            if (index < 0 || index > 5)
            {
                throw new ParameterDataFormatOrValueException("index", index);
            }
            if (command != 101 && command != 102)
            {
                throw new ParameterDataFormatOrValueException("command", command);
            }
            success = plc.SetGateCommand(index, command);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionCommand(dynamic parameters)
        {
            DataProvider provider = DataProvider.Instance;
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }

            int command = parameters.command;

            #region IP音柱联动
            switch (command)
            {
                case 10101:
                    SpeakerManager.Instance.SwitchOff(provider.GetPosition(positionId).SpeakerIds, positionId.ToString());
                    break;
                case 10105:
                    SpeakerManager.Instance.Evacuate(provider.GetPosition(positionId).SpeakerIds, positionId.ToString());
                    break;
                case 10108:
                    SpeakerManager.Instance.SwitchOn(provider.GetPosition(positionId).SpeakerIds, positionId.ToString());
                    break;
                default:
                    break;
            }
            #endregion

            int no = parameters.no;
            success = plc.SetCommand(command - 10000);//PLC提供的操作记录码与命令通讯码有重复，加10000给它们区分开
            OperationRecord record = new OperationRecord()
            {
                Index = plc.Position.Index,
                //TrackNo = plc.Position.Index,
                TrackNo = DataProvider.Instance.GetTrackByPosIndex(plc.Position.Index).No,
                WorkerNo = no.ToString(),
                OperationCode = command.ToString(),
                Time = DateTime.Now
            };
            DataProvider.Instance.AddOrUpdateOperationRecord(record);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionApply(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }

            int apply = parameters.apply;
            success = plc.SetApply(apply);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionReset(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }

            int command = parameters.command;
            int no = parameters.no;
            success = plc.SetReset(command);

            // another channel: Plc node ::AsGlobalPV:Record.Operation --> OnOperationRecordChanged
            OperationRecord record = new OperationRecord()
            {
                Index = plc.Position.Index,
                //TrackNo = DataProvider.Instance.GetTrackByPosIndex(plc.Position.Index).No,
                TrackNo = DataProvider.Instance.GetTrackByPosId(plc.Position.ID).No,
                WorkerNo = no.ToString(),
                OperationCode = $"{(int)PLCOperateCode.人数清零}",// command.ToString(),
                Time = DateTime.Now
            };
            DataProvider.Instance.AddOrUpdateOperationRecord(record);
            return ResponseData.Success(success).ToString();
        }

        private Response PositionConfirm(dynamic parameters)
        {
            int positionId = parameters.positionId;
            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }

            int command = parameters.command;
            int no = parameters.no;
            success = plc.SafeConfirm(command - 20000); // PLC提供的操作记录码与安全确认命令通讯码有重复，加20000给它们区分开
            OperationRecord record = new OperationRecord()
            {
                Index = plc.Position.Index,
                //TrackNo = plc.Position.Index,
                TrackNo = DataProvider.Instance.GetTrackByPosIndex(plc.Position.Index).No,
                WorkerNo = no.ToString(),
                OperationCode = command.ToString(),
                Time = DateTime.Now
            };
            DataProvider.Instance.AddOrUpdateOperationRecord(record);
            return ResponseData.Success(success).ToString();
        }

        private Response PosAlgorithmConfirm(dynamic _)
        {
            int positionId = int.Parse(GetForm("positionId"));
            //int no = int.Parse(GetForm("no"));
            int command = int.Parse(GetForm("command"));
            string msg = GetForm("msg");

            bool success = PLCManager.Instance.PLCDictionary.TryGetValue(positionId, out PLC plc);
            if (!success)
            {
                throw new ParameterDataFormatOrValueException("positionId", positionId);
            }

            success = plc.SetAlgorithmResult(command - 20000, msg);
            return ResponseData.Success(success).ToString();
        }


        private Response Heart(dynamic _)
        {
            return ResponseData.Success().ToString();
        }

        #region Query
        private Response QueryUserAuthRecords(dynamic _)
        {
            List<FilterDefinition<UserAuthRecord>> filters = new List<FilterDefinition<UserAuthRecord>>();
            FilterDefinitionBuilder<UserAuthRecord> builder = Builders<UserAuthRecord>.Filter;
            if (TryGetForm("TrackId", out string trackIdString))
            {
                int trackId = Convert.ToInt32(trackIdString);
                filters.Add(builder.Eq(i => i.TrackId, trackId));
            }
            if (TryGetForm("UserNo", out string userNoString))
            {
                int userNo = Convert.ToInt32(userNoString);
                filters.Add(builder.Eq(i => i.UserNo, userNo));
            }
            if (TryGetForm("PositionId", out string positionIdString))
            {
                int positionId = Convert.ToInt32(positionIdString);
                filters.Add(builder.AnyEq(i => i.PositionIds, positionId));
            }
            if (TryGetForm("IssueType", out string issueTypeString))
            {
                IssueType type = (IssueType)Convert.ToInt32(issueTypeString);
                filters.Add(builder.Eq(i => i.IssueType, type));
            }
            if (TryGetForm("Revoked", out string revokedString))
            {
                bool revoked = Convert.ToBoolean(revokedString);
                filters.Add(builder.Eq(i => i.Revoked, revoked));
            }
            if (TryGetForm("StartTime", out string startTimeString))
            {
                DateTime start = DateTime.Parse(startTimeString);
                filters.Add(builder.Gte(i => i.IssueTime, start));
            }
            if (TryGetForm("EndTime", out string endTimeString))
            {
                DateTime end = DateTime.Parse(endTimeString);
                filters.Add(builder.Lte(i => i.IssueTime, end));
            }
            int index = 1;
            if (TryGetForm("Index", out string indexString))
            {
                index = Convert.ToInt32(indexString);
                if (index < 1)
                {
                    index = 1;
                }
            }
            int count = 10;
            if (TryGetForm("Count", out string countString))
            {
                count = Convert.ToInt32(countString);
                if (count < 0)
                {
                    count = 0;
                }
            }

            FilterDefinition<UserAuthRecord> filter = filters.Any() ? builder.And(filters) : FilterDefinition<UserAuthRecord>.Empty;

            var query = DataProvider.Instance.GetAggregate<UserAuthRecord>(DataProvider.UserAuthRecordCollectionName).Find(filter);
            long total = query.CountDocuments();
            IEnumerable<UserAuthRecord> result = query.Skip((index - 1) * count).Limit(count).ToEnumerable();
            List<UserAuthRecord> test = result.ToList();
            PageData data = new PageData(result, total, count, index);
            return ResponseData.Success(data).ToString();
        }

        private Response QueryFaultsAndWarnings(dynamic _)
        {
            List<FilterDefinition<FaultMessage>> filters = new List<FilterDefinition<FaultMessage>>();
            FilterDefinitionBuilder<FaultMessage> builder = Builders<FaultMessage>.Filter;
            if (TryGetForm("TrackId", out string trackIdString))
            {
                int trackId = Convert.ToInt32(trackIdString);
                filters.Add(builder.Eq(i => i.Index, trackId));
            }

            if (TryGetForm("Handled", out string HandledString))
            {

                bool handled = Convert.ToBoolean(HandledString);
                if (!handled)
                {
                    filters.Add(builder.Eq(i => i.HandledBy, null));
                }
                else
                {
                    filters.Add(builder.Where(i => i.HandledBy != null));
                }
            }

            if (TryGetForm("HandledBy", out string HandledByString))
            {
                if (!HandledByString.IsNullOrEmptyOrWhiteSpace())
                {
                    string handleby = Convert.ToInt32(HandledByString).ToString();
                    filters.Add(builder.Eq(i => i.HandledBy, handleby));
                }

            }

            if (TryGetForm("FaultCode", out string faultCodeString))
            {
                // FaultCode 应该补0,否认查不到数据  by zhangliang 2021.11.17
                //string faultcode = Convert.ToInt32(faultCodeString).ToString();
                string faultcode = Convert.ToInt32(faultCodeString).ToString().PadLeft(3, '0');
                filters.Add(builder.Eq(i => i.FaultCode, faultcode));
            }
            if (TryGetForm("StartTime", out string startTimeString))
            {
                DateTime start = DateTime.Parse(startTimeString);
                filters.Add(builder.Gte(i => i.Time, start));
            }
            if (TryGetForm("EndTime", out string endTimeString))
            {
                DateTime end = DateTime.Parse(endTimeString);
                filters.Add(builder.Lte(i => i.Time, end));
            }
            int index = 1;
            if (TryGetForm("Index", out string indexString))
            {
                index = Convert.ToInt32(indexString);
                if (index < 1)
                {
                    index = 1;
                }
            }
            int count = 10;
            if (TryGetForm("Count", out string countString))
            {
                count = Convert.ToInt32(countString);
                if (count < 0)
                {
                    count = 0;
                }
            }
            FilterDefinition<FaultMessage> filter = filters.Any() ? builder.And(filters) : FilterDefinition<FaultMessage>.Empty;

            var query = DataProvider.Instance.GetAggregate<FaultMessage>(DataProvider.FaultRecordCollectionName)
                       .Find(filter)
                       .Sort(Builders<FaultMessage>.Sort.Descending(i => i.Time));
            long total = query.CountDocuments();
            IEnumerable<FaultMessage> result = query.Skip((index - 1) * count).Limit(count).ToEnumerable();
            PageData data = new PageData(result, total, count, index);

            return ResponseData.Success(data).ToString();
        }

        private Response QueryOperations(dynamic _)
        {
            List<FilterDefinition<OperationRecord>> filters = new List<FilterDefinition<OperationRecord>>();
            FilterDefinitionBuilder<OperationRecord> builder = Builders<OperationRecord>.Filter;
            if (TryGetForm("TrackId", out string trackIdString))
            {
                int trackId = Convert.ToInt32(trackIdString);
                filters.Add(builder.Eq(i => i.Index, trackId));
            }

            if (TryGetForm("OperationCode", out string operationCodeString))
            {
                if (Convert.ToInt32(operationCodeString) < 100)
                {
                    if (Convert.ToInt32(operationCodeString) < 10)
                    {
                        operationCodeString = "0" + operationCodeString;
                    }
                    operationCodeString = "0" + operationCodeString;
                }

                filters.Add(builder.Eq(i => i.OperationCode, operationCodeString));
            }
            if (TryGetForm("UserNo", out string workNoString))
            {
                string workNo = Convert.ToInt32(workNoString).ToString();
                filters.Add(builder.Eq(i => i.WorkerNo, workNo));
            }
            if (TryGetForm("StartTime", out string startTimeString))
            {
                DateTime start = DateTime.Parse(startTimeString);
                filters.Add(builder.Gte(i => i.Time, start));
            }
            if (TryGetForm("EndTime", out string endTimeString))
            {
                DateTime end = DateTime.Parse(endTimeString);
                filters.Add(builder.Lte(i => i.Time, end));
            }
            int index = 1;
            if (TryGetForm("Index", out string indexString))
            {
                index = Convert.ToInt32(indexString);
                if (index < 1)
                {
                    index = 1;
                }
            }
            int count = 10;
            if (TryGetForm("Count", out string countString))
            {
                count = Convert.ToInt32(countString);
                if (count < 0)
                {
                    count = 0;
                }
            }
            FilterDefinition<OperationRecord> filter = filters.Any() ? builder.And(filters) : FilterDefinition<OperationRecord>.Empty;

            var query = DataProvider.Instance.GetAggregate<OperationRecord>(DataProvider.OperationRecordCollectionName)
                        .Find(filter)
                        .Sort(Builders<OperationRecord>.Sort.Descending(i => i.Time));
            long total = query.CountDocuments();
            IEnumerable<OperationRecord> result = query.Skip((index - 1) * count).Limit(count).ToEnumerable();
            PageData data = new PageData(result, total, count, index);

            return ResponseData.Success(data).ToString();
        }

        private Response QueryTools(dynamic _)
        {
            List<FilterDefinition<ToolAuthRecord>> filters = new List<FilterDefinition<ToolAuthRecord>>();
            FilterDefinitionBuilder<ToolAuthRecord> builder = Builders<ToolAuthRecord>.Filter;
            if (TryGetForm("TrackId", out string trackIdString))
            {
                int trackId = Convert.ToInt32(trackIdString);
                filters.Add(builder.Eq(i => i.TrackId, trackId));
            }
            if (TryGetForm("UserNo", out string UserNoString))
            {
                int userNo = Convert.ToInt32(UserNoString);
                filters.Add(builder.Eq(i => i.UserNo, userNo));
            }
            if (TryGetForm("ToolId", out string ToolIdString))
            {
                int toolId = Convert.ToInt32(ToolIdString);
                filters.Add(builder.AnyEq(i => i.ToolIds, toolId));
            }
            if (TryGetForm("Revoked", out string RevokedString))
            {
                bool revoked = Convert.ToBoolean(RevokedString);
                filters.Add(builder.Eq(i => i.Revoked, revoked));
            }
            if (TryGetForm("StartTime", out string startTimeString))
            {
                DateTime start = DateTime.Parse(startTimeString);
                filters.Add(builder.Gte(i => i.IssueTime, start));
            }
            if (TryGetForm("EndTime", out string endTimeString))
            {
                DateTime end = DateTime.Parse(endTimeString);
                filters.Add(builder.Lte(i => i.IssueTime, end));
            }
            int index = 1;
            if (TryGetForm("Index", out string indexString))
            {
                index = Convert.ToInt32(indexString);
                if (index < 1)
                {
                    index = 1;
                }
            }
            int count = 10;
            if (TryGetForm("Count", out string countString))
            {
                count = Convert.ToInt32(countString);
                if (count < 0)
                {
                    count = 0;
                }
            }
            FilterDefinition<ToolAuthRecord> filter = filters.Any() ? builder.And(filters) : FilterDefinition<ToolAuthRecord>.Empty;

            var query = DataProvider.Instance.GetAggregate<ToolAuthRecord>(DataProvider.ToolAuthRecordCollectionName)
                        .Find(filter)
                        .Sort(Builders<ToolAuthRecord>.Sort.Descending(i => i.IssueTime));
            long total = query.CountDocuments();
            IEnumerable<ToolAuthRecord> result = query.Skip((index - 1) * count).Limit(count).ToEnumerable();
            PageData data = new PageData(result, total, count, index);

            return ResponseData.Success(data).ToString();
        }

        private Response QueryTrainNumbers(dynamic _)
        {
            List<FilterDefinition<TrainNumberRecord>> filters = new List<FilterDefinition<TrainNumberRecord>>();
            FilterDefinitionBuilder<TrainNumberRecord> builder = Builders<TrainNumberRecord>.Filter;
            if (TryGetForm("TrackId", out string trackIdString))
            {
                int trackId = Convert.ToInt32(trackIdString);
                filters.Add(builder.Eq(i => i.TrackId, trackId));
            }

            if (TryGetForm("StartTime", out string startTimeString))
            {
                DateTime start = DateTime.Parse(startTimeString);
                filters.Add(builder.Gte(i => i.Time, start));
            }
            if (TryGetForm("EndTime", out string endTimeString))
            {
                DateTime end = DateTime.Parse(endTimeString);
                filters.Add(builder.Lte(i => i.Time, end));
            }
            int index = 1;
            if (TryGetForm("Index", out string indexString))
            {
                index = Convert.ToInt32(indexString);
                if (index < 1)
                {
                    index = 1;
                }
            }
            int count = 10;
            if (TryGetForm("Count", out string countString))
            {
                count = Convert.ToInt32(countString);
                if (count < 0)
                {
                    count = 0;
                }
            }
            FilterDefinition<TrainNumberRecord> filter = filters.Any() ? builder.And(filters) : FilterDefinition<TrainNumberRecord>.Empty;

            var query = DataProvider.Instance.GetAggregate<TrainNumberRecord>(DataProvider.TrainNumberRecordCollectionName)
                        .Find(filter)
                        .Sort(Builders<TrainNumberRecord>.Sort.Descending(i => i.Time));
            long total = query.CountDocuments();
            IEnumerable<TrainNumberRecord> result = query.Skip((index - 1) * count).Limit(count).ToEnumerable();
            PageData data = new PageData(result, total, count, index);

            return ResponseData.Success(data).ToString();
        }
        #endregion

        #region user
        private Response Login(dynamic _)
        {
            string noString = GetForm("No");
            int no = Convert.ToInt32(noString);
            string password = GetForm("Password");
            User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.No == no);
            if (user == null)
            {
                throw new Exception("工号输入有误");
            }
            if (user.Password != password)
            {
                throw new Exception("密码输入错误");
            }

            // 记录当前登录用户
            ServerGlobal.CurrentUser = user;

            // 推送
            MainService.PushTrackState();

            return ResponseData.Success(user).ToString();
        }

        private object GetUsers(dynamic _)
        {
            return ResponseData<List<User>>.Success(DataProvider.Instance.UserList).ToString();
        }

        private Response AddOrUpdateUser(dynamic _)
        {
            User user = GetContent<User>();
            DataProvider.Instance.AddOrUpdateUser(user);
            return ResponseData.Success().ToString();
        }

        private Response UpsertUser(dynamic _)
        {
            User user = GetContent<User>(out string json);
            JObject obj = JObject.Parse(json);
            bool result = DataProvider.Instance.UpsertUser(user, obj);
            return result ? ResponseData.Success().ToString() : ResponseData.Error(ResponseCode.ParameterDataFormatOrValueError).ToString();
        }

        private Response DeleteUsers(dynamic _)
        {
            string ids = GetForm("UserIds");
            IEnumerable<int> list = ids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i =>
             {
                 var id = Convert.ToInt32(i);
                 return id;
             });
            var ss = list.ToList();
            DataProvider.Instance.DeleteUsers(list);
            return ResponseData.Success().ToString();
        }

        private object UploadUserPhoto(dynamic parameter)
        {
            UpdateUserPhotoParams parameters = GetContent<UpdateUserPhotoParams>();
            DataProvider.Instance.UploadUserPhoto(parameters.UserId, parameters.PhotoString);
            return ResponseData.Success().ToString();
        }


        #endregion

        #region function
        private Response GetFunctions(dynamic _)
        {
            return ResponseData<List<Function>>.Success(DataProvider.Instance.FunctionList).ToString();
        }
        #endregion

        #region role
        private Response GetRoles(dynamic _)
        {
            return ResponseData<List<Role>>.Success(DataProvider.Instance.RoleList).ToString();
        }

        private Response GetRole(dynamic parmaters)
        {
            int id = parmaters.id;
            return ResponseData<Role>.Success(DataProvider.Instance.RoleList.FirstOrDefault(i => i.ID == id)).ToString();
        }

        private Response AddOrUpdateRole(dynamic _)
        {
            Role role = GetContent<Role>();
            DataProvider.Instance.AddOrUpdateRole(role);
            return ResponseData.Success().ToString();
        }

        private Response DeleteRole(dynamic _)
        {
            string ids = GetForm("RoleIds");
            IEnumerable<int> list = ids.Split('|').Select(i => Convert.ToInt32(i));
            DataProvider.Instance.DeleteRoles(list);
            return ResponseData.Success().ToString();
        }
        #endregion

        #region external system
        private object GetExteranlSystems(dynamic _)
        {
            return ResponseData<List<ExternalSystem>>.Success(DataProvider.Instance.ExternalSystemList).ToString();
        }

        private object AddOrUpdateExternalSystem(dynamic _)
        {
            string json = GetContent();
            ExternalSystem es = json.JsonDeserialize<ExternalSystem>();
            if (es == null)
            {
                throw new ContentCastException(json);
            }
            DataProvider.Instance.AddOrUpdateExternalSystem(es);
            return ResponseData.Success().ToString();
        }

        private object DeleteExternalSystem(dynamic parameter)
        {
            int id = parameter.id;
            DataProvider.Instance.DeleteExternalSystem(id);
            return ResponseData.Success().ToString();
        }
        #endregion

        #region track
        private Response GetTracks(dynamic _)
        {
            return ResponseData<List<Track>>.Success(DataProvider.Instance.TrackList).ToString();
        }

        private Response AddOrUpdateTrack(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private Response DeleteTrack(dynamic arg)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region useful method
        private string GetForm(string key)
        {
            if (!Request.Form.ContainsKey(key))
            {
                throw new ParameterIsNullOrMissingException(key);
            }
            return Request.Form.ToDictionary()[key];
        }

        private bool TryGetForm(string key, out string value)
        {
            value = null;
            bool flag = Request.Form.ContainsKey(key);
            if (flag)
            {
                value = Request.Form.ToDictionary()[key];
            }
            return flag;
        }

        private string GetContent(Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] data = new byte[Request.Body.Length];
            Request.Body.Read(data, 0, data.Length);
            return encoding.GetString(data);
        }

        private T GetContent<T>(Encoding encoding = null)
        {
            string json = GetContent(encoding);
            //JObject.Parse(json)

            T t = json.JsonDeserialize<T>();
            if (t == null)
            {
                throw new ContentCastException(json);
            }
            return t;
        }

        private T GetContent<T>(out string json, Encoding encoding = null)
        {
            json = GetContent(encoding);
            T t = default;
            try
            {
                t = json.JsonDeserialize<T>();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }

            return t;
        }

        #endregion

        #region Project
        private Response Project(dynamic _)
        {
            if (Enum.TryParse(GetForm("Project"), out ProjectType project)
                    &&
                    project != ServerGlobal.Project)
            {
                ServerGlobal.Project = project;
                switch (project)
                {
                    case ProjectType.NationalRailway:
                        ToolBoxService.Instance.Start();
                        break;
                    case ProjectType.NationalRailway_BaiSe:
                        break;
                    case ProjectType.CityRailway_1:
                    case ProjectType.CityRailway_2:
                        ToolBoxService.Instance.Stop();
                        break;
                    case ProjectType.Shenzhen12:
                        //ToolBoxProcess.Instance.Start();  暂时不启用 工具柜
                        break;
                }
            }

            // 在获取客户端类型时就同步一次底层设备状态，防止PLC未发生变化导致界面始终未有信号上传
            //Main.PushTrackState();
            return ResponseData.Success(project).ToString();
        }
        #endregion

        private object GetTrackState(dynamic arg)
        {
            var states = DataProvider.Instance.TrackList.Select(t => t.State).ToList();
            //var message = states?.JsonSerialize();
            return ResponseData.Success(states);
        }

    }
}
