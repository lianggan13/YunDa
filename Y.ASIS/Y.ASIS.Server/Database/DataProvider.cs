using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.Communication;
using Y.ASIS.Common.Manager;
using Y.ASIS.Server.Models;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Database
{
    class DataProvider
    {
        public const string UserCollectionName = "ASIS_User";
        private const string ExternalSystemCollectionName = "ASIS_ExternalSystem";
        private const string TrackCollectionName = "ASIS_Track";
        private const string SwitcherCollectionName = "ASIS_Switcher";
        private const string AttendanceCollectionName = "ASIS_Attendance";
        private const string SpeakerCollectionName = "ASIS_Speaker";
        private const string ASIS_DeviceInfo = nameof(ASIS_DeviceInfo);
        private const string ASIS_VideoStream = nameof(ASIS_VideoStream);


        private const string RoleCollectionName = "ASIS_Role";
        private const string FunctionCollectionName = "ASIS_Function";
        private const string UserGroupCollectionName = "ASIS_UserGroup";
        private const string ToolCollectionName = "ASIS_Tool";

        private const string TrackViewName = "ASIS_TrackView";

        public const string FaultRecordCollectionName = "ASIS_FaultRecord";

        public const string OperationRecordCollectionName = "ASIS_OperationRecord";
        private const string AccessRecordCollectionName = "ASIS_AccessRecord";
        public const string ToolAuthRecordCollectionName = "ASIS_ToolAuthRecord";
        public const string TrainNumberRecordCollectionName = "ASIS_TrainNumberRecord";
        public const string UserAuthRecordCollectionName = "ASIS_UserAuthRecord";


        static DataProvider()
        {
        }

        private static DataProvider instance;
        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataProvider();
                }
                return instance;
            }
        }

        private readonly MongodbDataConnection connection;
        private readonly IMongoDatabase database;
        public DataProvider()
        {
            string ip = LocalConfigManager.GetAppSettingValue("Database.Ip");
            string port = LocalConfigManager.GetAppSettingValue("Database.Port");
            string dbName = LocalConfigManager.GetAppSettingValue("Database.Name");
            connection = new MongodbDataConnection();
            connection.Connect(ip, port, dbName);
            database = connection.GetConnect();
        }

        public IMongoCollection<T> GetAggregate<T>(string aggregateName)
        {
            return database.GetCollection<T>(aggregateName);
        }

        #region track
        private List<Track> trackList;
        public List<Track> TrackList
        {
            get
            {
                if (trackList == null)
                {
                    trackList = GetAggregate<Track>(TrackViewName).Find(Builders<Track>.Filter.Empty).ToList();
                }
                return trackList;
            }
        }

        public Position GetPosition(int positionId)
        {
            var pos = TrackList.SelectMany(t => t.Positions).FirstOrDefault(p => p.ID == positionId);
            return pos;
        }

        public Track GetTrackByPosIndex(int index)
        {
            var track = TrackList.FirstOrDefault(t => t.Positions.Any(p => p.Index == index));
            return track;
        }

        public Track GetTrackByPosId(int positionId)
        {
            var track = TrackList.FirstOrDefault(t => t.Positions.Any(p => p.ID == positionId));
            return track;
        }

        #endregion


        private List<DeviceInfo> deviceInfos;
        public List<DeviceInfo> DeviceInfos
        {
            get
            {
                if (deviceInfos == null)
                {
                    deviceInfos = GetAggregate<DeviceInfo>(ASIS_DeviceInfo).Find(d => true).ToList();
                }
                return deviceInfos;
            }
        }

        private List<VideoStream> videoStreams;
        public List<VideoStream> VideoStreams
        {
            get
            {
                if (videoStreams == null)
                {
                    videoStreams = GetAggregate<VideoStream>(ASIS_VideoStream).Find(d => true).ToList();
                }
                return videoStreams;
            }
        }

        #region user
        private List<User> userList;
        public List<User> UserList
        {
            get
            {
                if (userList == null)
                {
                    userList = GetAggregate<User>(UserCollectionName).Find(Builders<User>.Filter.Empty).ToList();
                }
                return userList;
            }
        }

        public void AddOrUpdateUser(User user)
        {
            int index = UserList.FindIndex(i => i.ID == user.ID);
            if (index == -1)
            {
                long count = GetAggregate<User>(UserCollectionName).Find(i => i.Name == user.Name).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("新增人员失败: 名称重复");
                }

                count = GetAggregate<User>(UserCollectionName).Find(i => i.No == user.No).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("新增人员失败: 工号重复");
                }

                if (user.CardNo != 0)
                {
                    count = GetAggregate<User>(UserCollectionName).Find(i => i.ID != user.ID && i.CardNo == user.CardNo).CountDocuments();
                    if (count > 0)
                    {
                        throw new Exception("新增人员失败: 卡已被使用");
                    }
                }
                User max = GetAggregate<User>(UserCollectionName).Find(Builders<User>.Filter.Empty).Sort(Builders<User>.Sort.Descending(i => i.ID)).FirstOrDefault();

                if (user.ID <= 0)
                {
                    user.ID = max == null ? 1 : max.ID + 1; // 指定 User ID
                }

                if (!user.NewPassword.IsNullOrEmptyOrWhiteSpace())
                {
                    user.Password = user.NewPassword;
                    // user.NewPassword = ""; 导致新增用户后,无法编辑用户 by zhangliang 2021.11.22
                    user.NewPassword = null;
                    user.AllowUpdate = true;
                }

                GetAggregate<User>(UserCollectionName).InsertOne(user);
                UserList.Add(user);
            }
            else
            {
                long count = GetAggregate<User>(UserCollectionName).Find(i => i.ID != user.ID && i.No == user.No).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("更新人员失败: 工号重复");
                }

                count = GetAggregate<User>(UserCollectionName).Find(i => i.ID == user.ID && i.No == user.No
                                                                    && !i.AllowUpdate).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("更新人员失败: 该员工处于作业状态");
                }

                if (user.CardNo != 0)
                {
                    count = GetAggregate<User>(UserCollectionName).Find(i => i.ID != user.ID && i.CardNo == user.CardNo).CountDocuments();
                    if (count > 0)
                    {
                        throw new Exception("更新人员失败: 卡已被使用");
                    }
                }
                User get = UserList[index];
                if (!user.OldPassword.IsNullOrEmptyOrWhiteSpace() && get.Password != user.OldPassword)
                {
                    throw new Exception("更新人员失败: 旧密码错误");
                }
                else if (!user.NewPassword.IsNullOrEmptyOrWhiteSpace())
                {
                    get.Password = user.NewPassword;
                }
                get.Name = user.Name;
                get.No = user.No;
                get.CardNo = user.CardNo;
                get.RoleId = user.RoleId;
                get.Photo = user.Photo;
                get.UserGroupId = user.UserGroupId;
                get.AllowUpdate = user.AllowUpdate;
                GetAggregate<User>(UserCollectionName).ReplaceOne(i => i.ID == get.ID, get);
            }
        }

        public bool UpsertUser(User newUser, JObject obj)
        {
            var oldUser = UserList.FirstOrDefault(u => u.ID == newUser.ID);
            if (string.IsNullOrEmpty(oldUser?.Photo))
            {
                newUser.Photo = $"{obj["Photo"]}";
            }
            if (!string.IsNullOrWhiteSpace(newUser.NewPassword))
            {
                newUser.Password = newUser.NewPassword;
                // user.NewPassword = ""; 导致新增用户后,无法编辑用户 by zhangliang 2021.11.22
                newUser.NewPassword = null;
            }

            long count = GetAggregate<User>(UserCollectionName).Find(i => i.ID != newUser.ID && i.No == newUser.No).CountDocuments();
            if (count > 0)
            {
                return false;
            }

            newUser.AllowUpdate = true;

            // upsert db
            ReplaceOptions opt = new ReplaceOptions()
            {
                IsUpsert = true // 没有，就插入
            };
            ReplaceOneResult reuslt = GetAggregate<User>(UserCollectionName).ReplaceOne(i => i.ID == newUser.ID, newUser, opt);

            // updert list
            if (oldUser == null)
            {
                UserList.Add(newUser);  // insert
            }
            else
            {
                int index = UserList.IndexOf(oldUser);
                UserList[index] = newUser; // update
            }

            return true;
        }

        public void UploadUserPhoto(int id, string photoString)
        {
            User user = UserList.FirstOrDefault(i => i.ID == id);
            if (user == null)
            {
                return;
            }
            bool flag = ImageUtil.TryFindFaceFromBase64String(photoString, out string faceString);
            if (!flag)
            {
                throw new Exception("照片没有满足要求的人脸信息");
            }
            user.Photo = faceString;
            AddOrUpdateUser(user);
        }

        public void DeleteUsers(IEnumerable<int> ids)
        {
            long count = GetAggregate<User>(UserCollectionName).Find(i => ids.Contains(i.ID)
                                                                    && !i.AllowUpdate).CountDocuments();
            if (count > 0)
            {
                throw new Exception("删除人员失败: 该员工处于作业状态");
            }

            GetAggregate<User>(UserCollectionName).DeleteMany(i => ids.Contains(i.ID));
            UserList.RemoveAll(i => ids.Contains(i.ID));
        }


        #endregion

        #region user group
        private List<UserGroup> userGroupList;
        public List<UserGroup> UserGroupList
        {
            get
            {
                if (userGroupList == null)
                {
                    userGroupList = GetAggregate<UserGroup>(UserGroupCollectionName).Find(Builders<UserGroup>.Filter.Empty).ToList();
                }
                return userGroupList;
            }
        }

        public void AddOrUpdateUserGroup(UserGroup group)
        {
            int index = UserGroupList.FindIndex(i => i.ID == group.ID);
            if (index == -1)
            {
                long count = GetAggregate<UserGroup>(UserGroupCollectionName).Find(i => i.Name == group.Name).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("新增班组失败: 名称重复");
                }
                UserGroup max = GetAggregate<UserGroup>(UserGroupCollectionName).Find(Builders<UserGroup>.Filter.Empty).Sort(Builders<UserGroup>.Sort.Descending(i => i.ID)).FirstOrDefault();
                group.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<UserGroup>(UserGroupCollectionName).InsertOne(group);
                UserGroupList.Add(group);
            }
            else
            {
                long count = GetAggregate<UserGroup>(UserGroupCollectionName).Find(i => i.Name == group.Name).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("更新班组失败: 名称重复");
                }
                UserGroup get = UserGroupList[index];
                GetAggregate<UserGroup>(UserGroupCollectionName).ReplaceOne(i => i.ID == group.ID, group);
                get.Name = group.Name;
            }
        }

        public void DeleteUserGroups(IEnumerable<int> ids)
        {
            if (UserList.Any(i => ids.Contains(i.UserGroupId)))
            {
                throw new Exception("删除班组失败: 存在正在使用的班组");
            }

            GetAggregate<UserGroup>(UserGroupCollectionName).DeleteMany(i => ids.Contains(i.ID));
            UserGroupList.RemoveAll(i => ids.Contains(i.ID));
        }
        #endregion

        #region external system
        private List<ExternalSystem> externalSystemList;
        public List<ExternalSystem> ExternalSystemList
        {
            get
            {
                if (externalSystemList == null)
                {
                    externalSystemList = GetAggregate<ExternalSystem>(ExternalSystemCollectionName).Find(i => i.Enable).ToList();
                }
                return externalSystemList;
            }
        }

        public ExternalSystem GetExternalSystem(string authKey)
        {
            return ExternalSystemList.FirstOrDefault(i => i.AuthKey == authKey);
        }

        public void AddOrUpdateExternalSystem(ExternalSystem es)
        {
            if (es.ID == 0)
            {
                es.ID = externalSystemList.Max(i => i.ID) + 1;
                GetAggregate<ExternalSystem>(ExternalSystemCollectionName).InsertOne(es);
                ExternalSystemList.Add(es);
            }
            else
            {
                int index = ExternalSystemList.FindIndex(i => i.ID == es.ID);
                if (index != -1)
                {
                    ExternalSystemList[index] = es;
                    GetAggregate<ExternalSystem>(ExternalSystemCollectionName).ReplaceOneAsync(i => i.ID == es.ID, es);
                }

            }
        }

        public void DeleteExternalSystem(int id)
        {
            int index = ExternalSystemList.FindIndex(i => i.ID == id);
            if (index != -1)
            {
                ExternalSystemList.RemoveAt(index);
                GetAggregate<ExternalSystem>(ExternalSystemCollectionName).DeleteOneAsync(i => i.ID == id);
            }
        }


        #endregion

        #region role
        private List<Role> roleList;
        public List<Role> RoleList
        {
            get
            {
                if (roleList == null)
                {
                    roleList = GetAggregate<Role>(RoleCollectionName).Find(Builders<Role>.Filter.Empty).ToList();
                }
                return roleList;
            }
        }

        public void AddOrUpdateRole(Role role)
        {
            int index = RoleList.FindIndex(i => i.ID == role.ID);
            if (index == -1)
            {
                long count = GetAggregate<Role>(RoleCollectionName).Find(i => i.Name == role.Name).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("新增角色失败: 名称重复");
                }
                Role max = GetAggregate<Role>(RoleCollectionName).Find(Builders<Role>.Filter.Empty).Sort(Builders<Role>.Sort.Descending(i => i.ID)).FirstOrDefault();
                role.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<Role>(RoleCollectionName).InsertOne(role);
                RoleList.Add(role);
            }
            else
            {
                long count = GetAggregate<Role>(RoleCollectionName).Find(i => i.ID != role.ID && i.Name == role.Name).CountDocuments();
                if (count > 0)
                {
                    throw new Exception("更新角色失败: 名称重复");
                }
                Role get = RoleList[index];
                GetAggregate<Role>(RoleCollectionName).ReplaceOne(i => i.ID == role.ID, role);
                get.Name = role.Name;
                get.Functions = role.Functions;
                get.Describe = role.Describe;
            }
        }

        public void DeleteRoles(IEnumerable<int> ids)
        {
            if (UserList.Any(i => ids.Contains(i.RoleId)))
            {
                throw new Exception("删除角色失败: 存在已分配的角色");
            }

            GetAggregate<Role>(RoleCollectionName).DeleteMany(i => ids.Contains(i.ID));
            RoleList.RemoveAll(i => ids.Contains(i.ID));
        }
        #endregion

        #region function
        private List<Function> functionList;
        public List<Function> FunctionList
        {
            get
            {
                if (functionList == null)
                {
                    functionList = GetAggregate<Function>(FunctionCollectionName).Find(Builders<Function>.Filter.Empty).ToList();
                }
                return functionList;
            }
        }
        #endregion

        #region fault
        public void AddFaultRecord(FaultRecord record)
        {
            if (record.ID == 0)
            {
                FaultRecord max = GetAggregate<FaultRecord>(FaultRecordCollectionName).
                    Find(Builders<FaultRecord>.Filter.Empty).Sort(Builders<FaultRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();

                record.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<FaultRecord>(FaultRecordCollectionName).InsertOneAsync(record);
            }
            else
            {
                GetAggregate<FaultRecord>(FaultRecordCollectionName).ReplaceOne(i => i.ID == record.ID, record);
            }

        }

        public void HandleWarning(int id, string handleBy, string remarks)
        {
            List<UpdateDefinition<FaultMessage>> updates = new List<UpdateDefinition<FaultMessage>>();
            UpdateDefinitionBuilder<FaultMessage> builder = new UpdateDefinitionBuilder<FaultMessage>();
            updates.Add(builder.Set(i => i.HandledBy, handleBy));
            updates.Add(builder.Set(i => i.HandledTime, DateTime.Now));
            updates.Add(builder.Set(i => i.Remarks, remarks));
            GetAggregate<FaultMessage>(FaultRecordCollectionName).UpdateOne(i => i.ID == id, builder.Combine(updates));
        }

        public long GetUnhandleWarningsCount()
        {
            return GetAggregate<FaultMessage>(FaultRecordCollectionName).Find(i => i.HandledBy == null).CountDocuments();
        }

        #endregion

        #region operation
        public void AddOrUpdateOperationRecord(OperationRecord record)
        {
            if (record.ID == 0)
            {
                OperationRecord max = GetAggregate<OperationRecord>(OperationRecordCollectionName).
                    Find(Builders<OperationRecord>.Filter.Empty).Sort(Builders<OperationRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();
                record.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<OperationRecord>(OperationRecordCollectionName).InsertOneAsync(record);
            }
            else
            {
                GetAggregate<OperationRecord>(OperationRecordCollectionName).ReplaceOne(i => i.ID == record.ID, record);
            }

        }
        #endregion

        #region access
        public void AddAccessRecord(AccessRecord record)
        {
            GetAggregate<AccessRecord>(AccessRecordCollectionName).InsertOneAsync(record);
        }

        public List<AccessRecord> GetAccessRecords()
        {
            //return GetAggregate<AccessRecord>(AccessRecordCollectionName).Find(Builders<AccessRecord>.Filter.Empty).ToList();
            // 按照 Time 字段降序排序 by zhangliang 2021-12-08
            var sortdef = Builders<AccessRecord>.Sort.Descending(s => s.Time);
            return GetAggregate<AccessRecord>(AccessRecordCollectionName).Find(Builders<AccessRecord>.Filter.Empty).Sort(sortdef).ToList();
        }
        #endregion

        #region user auth record

        public void AddOrUpdateUserAuthRecord(int positionId, ISet<User> workerUser, ISet<User> operaterUser)
        {
            List<User> users = new List<User>(workerUser);
            users.AddRange(operaterUser);

            var track = GetTrackByPosId(positionId);
            var pos = GetPosition(positionId);
            if (track == null || pos == null)
                return;

            users.ForEach(u =>
            {
                UserAuthRecord record = GetUserAuthRecord(track.ID, u.No);
                if (record != null)
                {
                    record.IssueType = u.IssueType;
                }
                else
                {
                    record = new UserAuthRecord(track.ID, new List<int>() { positionId }, u.No, u.IssueType);
                }
                AddOrUpdateUserAuthRecord(record);
            });
        }

        public void AddOrUpdateUserAuthRecord(UserAuthRecord record)
        {
            if (record.ID == 0)
            {
                UserAuthRecord max = GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).Find(Builders<UserAuthRecord>.Filter.Empty).Sort(Builders<UserAuthRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();
                record.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).InsertOne(record);
            }
            else
            {
                GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).ReplaceOne(i => i.ID == record.ID, record);
            }
        }

        public void UpdateUserAllowUpdateState(ISet<User> workerUser, ISet<User> operaterUser, bool state)
        {
            List<User> users = new List<User>();
            if (workerUser?.Count > 0)
                users.AddRange(workerUser);
            if (operaterUser?.Count > 0)
                users.AddRange(operaterUser);

            users.ForEach(i =>
            {
                int index = UserList.FindIndex(j => j.ID == i.ID);

                User get = UserList[index];

                get.AllowUpdate = state;

                GetAggregate<User>(UserCollectionName).ReplaceOne(j => j.ID == get.ID, get);
            });
        }


        public void UpdateUserAllowUpdateState(IEnumerable<int> userNos, bool state)
        {
            foreach (var no in userNos)
            {
                int index = UserList.FindIndex(j => j.No == no);
                if (index == -1) continue;

                User get = UserList[index];
                get.AllowUpdate = state;
                GetAggregate<User>(UserCollectionName).ReplaceOne(j => j.ID == get.ID, get);
            }
        }


        public void UpdateUserAllowUpdateState(User user)
        {
            bool s = user.PositionIds.Count > 0;
            user.AllowUpdate = !s;
            GetAggregate<User>(UserCollectionName).ReplaceOne(j => j.ID == user.ID, user);
        }


        public UserAuthRecord GetUserAuthRecordsByUserNo(int userNo, bool revoked = false)
        {
            var sortdef = Builders<UserAuthRecord>.Sort.Descending(s => s.IssueTime); // 根据授权时间降序排序
            var record = GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName)
                                                .Find(
                                                   i => i.UserNo == userNo
                                                   && i.Revoked == revoked)
                                                .Sort(sortdef).FirstOrDefault();

            return record;
        }

        public List<UserAuthRecord> GetUserAuthRecordsByRegionId(int trackId, bool revoked = false)
        {
            return GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).Find(
                i => i.TrackId == trackId
                     && i.Revoked == revoked
            ).ToList();
        }

        public UserAuthRecord GetUserAuthRecord(int trackId, int userNo, bool revoked = false)
        {
            return GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).Find(
                i => i.TrackId == trackId
                     && i.UserNo == userNo
                     && i.Revoked == revoked
            ).FirstOrDefault();
        }

        public List<UserAuthRecord> GetUserAuthRecords(int positionId, IEnumerable<int> userNos, bool revoked = false)
        {
            var track = TrackList.Find(t => t.Positions.Any(p => p.Index == positionId));
            var result = GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).Find(
                                                                  r => r.TrackId == track.No
                                                                  && userNos.Contains(r.UserNo)
                                                                  && r.Revoked == revoked).ToList();
            return result ?? null;
        }

        public List<UserAuthRecord> GetUserAuthRecords(IEnumerable<int> userNos, bool revoked = false)
        {
            var result = GetAggregate<UserAuthRecord>(UserAuthRecordCollectionName).Find(
                                                                     r => userNos.Contains(r.UserNo)
                                                                     && r.Revoked == revoked).ToList();
            return result ?? null;
        }

        #endregion

        #region TrainNumber
        public void AddTrainNumberRecord(TrainNumberRecord record)
        {
            if (record.ID == 0)
            {
                TrainNumberRecord max = GetAggregate<TrainNumberRecord>(TrainNumberRecordCollectionName).Find(Builders<TrainNumberRecord>.Filter.Empty).Sort(Builders<TrainNumberRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();
                record.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<TrainNumberRecord>(TrainNumberRecordCollectionName).InsertOneAsync(record);
            }
            else
            {
                GetAggregate<TrainNumberRecord>(TrainNumberRecordCollectionName).ReplaceOne(i => i.ID == record.ID, record);
            }

        }
        #endregion

        #region tool auth record

        public void AddToolAuthRecord(int posIndex, ISet<User> workerUser)
        {
            var tracks = TrackList.Where(t => t.Positions.Any(p => p.Index == posIndex));
            var track = tracks?.FirstOrDefault();
            foreach (var k in workerUser)
            {
                var record = new ToolAuthRecord(track.No, k.No);
                AddOrUpdateToolAuthRecord(record);
            }
        }

        public void AddOrUpdateToolAuthRecord(ToolAuthRecord record)
        {
            if (record.ID == 0)
            {
                ToolAuthRecord max = GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).Find(Builders<ToolAuthRecord>.Filter.Empty).Sort(Builders<ToolAuthRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();
                record.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).InsertOne(record);
            }
            else
            {
                GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).ReplaceOne(i => i.ID == record.ID, record);
            }
        }


        public ToolAuthRecord GetToolAuthRecord(int trackId, int userNo, bool revoked = false)
        {
            return GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).Find(
                i => i.TrackId == trackId
                     && i.UserNo == userNo
                     && i.Revoked == revoked
            ).Sort(Builders<ToolAuthRecord>.Sort.Descending(i => i.ID)).FirstOrDefault();
        }


        public List<ToolAuthRecord> GetToolAuthRecords(int positionId, IEnumerable<int> userNos, bool revoked = false)
        {
            List<ToolAuthRecord> result = new List<ToolAuthRecord>();
            TrackList.ForEach(i =>
            {
                i.Positions.ForEach(j =>
                {
                    if (j.Index == positionId)
                    {
                        result = GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).Find(
                                                                k => k.TrackId == i.No
                                                                && userNos.Contains(k.UserNo)
                                                                && k.Revoked == revoked
                                                                ).ToList();
                    }
                });
            });
            return result ?? null;
        }

        public List<ToolAuthRecord> GetToolAuthRecords(IEnumerable<int> userNos, bool revoked = false)
        {
            List<ToolAuthRecord> result = new List<ToolAuthRecord>();

            result = GetAggregate<ToolAuthRecord>(ToolAuthRecordCollectionName).Find(
                                                                    k => userNos.Contains(k.UserNo)
                                                                    && k.Revoked == revoked
                                                                    ).ToList();
            return result ?? null;
        }
        #endregion

        #region Tool
        private List<Tool> toolList;
        public List<Tool> ToolList
        {
            get
            {
                if (toolList == null)
                {
                    toolList = GetAggregate<Tool>(ToolCollectionName).Find(Builders<Tool>.Filter.Empty).ToList();
                }
                return toolList;
            }
        }

        public void AddOrUpdateTool(Tool tool)
        {
            int index = ToolList.FindIndex(i => i.ID == tool.ID);
            if (index == -1)
            {
                Tool max = GetAggregate<Tool>(ToolCollectionName).Find(Builders<Tool>.Filter.Empty).Sort(Builders<Tool>.Sort.Descending(i => i.ID)).FirstOrDefault();
                tool.ID = max == null ? 1 : max.ID + 1;
                GetAggregate<Tool>(ToolCollectionName).InsertOne(tool);
                ToolList.Add(tool);
            }
            else
            {
                Tool get = ToolList[index];
                GetAggregate<Tool>(ToolCollectionName).ReplaceOne(i => i.ID == tool.ID, tool);
                get.Name = tool.Name;
            }
        }
        #endregion
    }
}
