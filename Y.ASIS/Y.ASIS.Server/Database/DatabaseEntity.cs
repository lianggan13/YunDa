using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Y.ASIS.Common.Communication;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Models;
using Y.ASIS.Server.Utility;

namespace Y.ASIS.Server.Database
{

    public abstract class DynamicBson
    {
        [JsonIgnore]
        public virtual bool CanUpdate { get; set; }

        protected abstract void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null);
    }

    ///// <summary>
    ///// 描述一个人脸机
    ///// </summary>
    //[BsonIgnoreExtraElements]
    //public class AttendanceInfo
    //{
    //    public int ID { get; set; }

    //    public string SN { get; set; }

    //    public string Name { get; set; }

    //    public string Ip { get; set; }

    //    public string Type { get; set; }
    //}



    /// <summary>
    /// opc配置信息
    /// </summary>
    public class OpcConfig
    {
        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    /// <summary>
    /// 描述一个股道
    /// </summary>
    [BsonIgnoreExtraElements]
    class Track
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 股道号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 股道名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 股道类型
        /// </summary>
        public TrackType Type { get; set; }

        /// <summary>
        /// 列位
        /// </summary>
        public List<Position> Positions { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonIgnore]
        public TrackState State
        {
            get
            {
                return TrackState.FromTrack(this);
            }
        }

        #region inner classess
        public class TrackState
        {
            public int TrackId { get; set; }

            public IEnumerable<PositionState> PositionStates { get; set; }

            public static TrackState FromTrack(Track track)
            {
                TrackState state = new TrackState()
                {
                    TrackId = track.ID,
                    PositionStates = track.Positions.Select(i => i.State)
                };
                return state;
            }
        }
        #endregion
    }


    /// <summary>
    /// 描述一个用户
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : DynamicBson
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 获取或设置用户工号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 获取或设置用户卡号
        /// </summary>
        public int? CardNo { get; set; }

        /// <summary>
        /// 获取或设置用户名称
        /// </summary>
        public string Name { get; set; }

        private string photo;
        /// <summary>
        /// 获取或设置用户照片
        /// </summary>
        [JsonIgnore]
        public string Photo
        {
            get { return photo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (photo != value)
                {
                    photo = value;
                    PhotoUrl = ImageUtil.Base64StringToUrl(value);
                }
            }
        }

        private string photoUrl;
        /// <summary>
        /// 获取用户的照片地址
        /// </summary>
        [BsonIgnore]
        public string PhotoUrl
        {
            get
            {
                if (photoUrl.IsNullOrEmptyOrWhiteSpace()
                    && !photo.IsNullOrEmptyOrWhiteSpace())
                {
                    photoUrl = ImageUtil.Base64StringToUrl(photo);
                }
                return photoUrl;
            }
            set
            {
                if (photoUrl != value)
                {
                    photoUrl = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置用户的登录密码
        /// </summary>
        //[JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置用户设置的旧密码
        /// </summary>
        [BsonIgnore]
        public string OldPassword { get; set; }

        /// <summary>
        /// 获取或设置用户设置的新密码
        /// </summary>
        [BsonIgnore]
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取或设置用户的角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 是否允许更新(用户下发权限后不允许更新)
        /// </summary>
        public bool AllowUpdate { get; set; }

        /// <summary>
        /// 获取或设置用户班组编号
        /// </summary>
        public int UserGroupId { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public IssueType IssueType { get; set; }


        private ObservableCollection<int> positionIds = new ObservableCollection<int>();
        [JsonIgnore]
        public ObservableCollection<int> PositionIds
        {
            get { return positionIds; }
            set
            {
                if (positionIds != value)
                {
                    if (positionIds != null)
                    {
                        positionIds.CollectionChanged -= OnPositionIdsChanged;
                    }
                    positionIds = value;
                    positionIds.CollectionChanged += OnPositionIdsChanged;
                    UpdateProperty(value);
                }
            }
        }

        private void OnPositionIdsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateProperty(positionIds, nameof(PositionIds));
        }


        protected override void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            //if (!CanUpdate)
            //{
            //    return;
            //}
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(i => i.ID, ID);
            FieldDefinition<User, T> field = propertyName;
            UpdateDefinition<User> update = Builders<User>.Update.Set(field, value);
            DataProvider.Instance.GetAggregate<User>(DataProvider.UserCollectionName).UpdateOne(filter, update);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User user))
            {
                return false;
            }
            return ID == user.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public SimpleUser ToSimpleUser()
        {
            return new SimpleUser()
            {
                Id = ID,
                No = No,
                CardNo = CardNo,
                Name = Name,
                UserGroupId = UserGroupId
            };
        }
    }

    [BsonIgnoreExtraElements]
    public class UserGroup
    {
        [JsonProperty("Id")]
        public int ID { get; set; }

        public string Name { get; set; }
    }

    /// <summary>
    /// 描述一个角色
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Role
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 获取或设置角色名称
        /// </summary>
        [JsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置角色可用功能列表
        /// </summary>
        [JsonRequired]
        public List<int> Functions { get; set; }

        /// <summary>
        /// 获取或设置描述信息
        /// </summary>
        [JsonRequired]
        public string Describe { get; set; }
    }

    /// <summary>
    /// 描述一个功能
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Function
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 描述一个第三方系统(接入本服务的系统)
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ExternalSystem
    {
        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("Id")]
        public int ID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 授权通信秘钥
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 推送地址
        /// </summary>
        public string PushAddress { get; set; }

        /// <summary>
        /// 上一次通信的时间
        /// </summary>
        [BsonIgnore]
        [JsonIgnore]
        public DateTime LastTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 是否处于连接状态
        /// </summary>
        [BsonIgnore]
        [JsonIgnore]
        public bool Connected
        {
            get { return (DateTime.Now - LastTime) < TimeSpan.FromSeconds(10d); }
        }
    }

    [BsonIgnoreExtraElements]
    class OperationRecord
    {
        [JsonProperty("Id")]
        public long ID { get; set; }

        public int TrackNo { get; set; }

        public string Track
        {
            get
            {
                Track track = DataProvider.Instance.TrackList.FirstOrDefault(i => i.ID == Index);
                return track?.Name;
            }
        }

        public int Index { get; set; }

        public string WorkerNo { get; set; }

        [BsonIgnore]
        public SimpleUser User
        {
            get
            {
                if (!WorkerNo.IsNullOrEmptyOrWhiteSpace())
                {
                    User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.No == Convert.ToInt32(WorkerNo));
                    return user?.ToSimpleUser();
                }
                else
                {
                    return null;
                }
            }
        }

        public string OperationCode { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }
    }

    [BsonIgnoreExtraElements]
    class FaultRecord
    {
        [JsonProperty("Id")]
        public long ID { get; set; }

        public int TrackNo { get; set; }

        public int Index { get; set; }

        public string DeviceNo { get; set; }

        public string FaultCode { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        [JsonIgnore]
        public int? HandledBy { get; set; }

        [BsonIgnore]
        [JsonProperty("HandledBy")]
        public string HandledUser
        {
            get
            {
                User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.ID == HandledBy);
                return user?.Name;
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? HandledTime { get; set; }

        public string Remarks { get; set; }

    }

    [BsonIgnoreExtraElements]
    class AccessRecord
    {
        public int TrackNo { get; set; }

        public int Index { get; set; }

        public string WorkerNo { get; set; }

        public int DoorId { get; set; }

        public bool IsValid { get; set; }

        public AccessType Type { get; set; }

        public AccessMethod Method { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        public enum AccessType
        {
            Enter,
            Leave,
            Unknown = 3
        }

        public enum AccessMethod
        {
            Card,
            Button,
            Warning,
            Unknown
        }
    }

    /// <summary>
    /// 工具授权记录
    /// </summary>
    [BsonIgnoreExtraElements]
    class ToolAuthRecord
    {
        public ToolAuthRecord(int trackId, int userNo)
        {
            TrackId = trackId;
            UserNo = userNo;
            IssueTime = DateTime.Now;
            ToolIds = new List<int>();
        }

        [JsonProperty("Id")]
        public long ID { get; set; }

        [JsonIgnore]
        public int TrackId { get; set; }

        [BsonIgnore]
        public string Track
        {
            get
            {
                Track track = DataProvider.Instance.TrackList.FirstOrDefault(i => i.ID == TrackId);
                return track?.Name;
            }
        }

        [JsonIgnore]
        public int UserNo { get; set; }

        [BsonIgnore]
        public SimpleUser User
        {
            get
            {
                User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.No == UserNo);
                return user?.ToSimpleUser();
            }
        }

        [JsonIgnore]
        public List<int> ToolIds { get; set; }

        [BsonIgnore]
        public List<string> Tools
        {
            get
            {
                List<string> tools = new List<string>(ToolIds.Count);
                DataProvider provider = DataProvider.Instance;
                ToolIds.ForEach(i =>
                {
                    Tool tool = provider.ToolList.FirstOrDefault(j => j.ID == i);
                    if (tool != null)
                    {
                        tools.Add(tool.Name);
                    }
                });
                return tools;
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime IssueTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TakeTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ReturnTime { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime? RevokeTime { get; set; }


        /// <summary>
        /// 借出
        /// </summary>
        public void Take()
        {
            TakeTime = DateTime.Now;
        }

        /// <summary>
        /// 归还
        /// </summary>
        public void Return()
        {
            ReturnTime = DateTime.Now;
        }

        public bool Revoked { get; set; }

        /// <summary>
        /// 销权
        /// </summary>
        public void Revoke()
        {
            Revoked = true;
            //RevokeTime = DateTime.Now;
        }
    }

    [BsonIgnoreExtraElements]
    abstract class KeyOrToolBase
    {
        [JsonProperty("Id")]
        public int ID { get; set; }

        public string Name { get; set; }

        public List<int> PositionIndexes { get; set; }
    }

    [BsonIgnoreExtraElements]
    class Tool : KeyOrToolBase
    {
        //private bool hasLent;
        //public bool HasLent
        //{
        //    get { return hasLent; }
        //    set
        //    {
        //        if (hasLent != value)
        //        {
        //            hasLent = value;
        //            //更新工具柜
        //            //DataProvider.Instance.UpdateTool(this);
        //        }
        //    }
        //}

        private bool @using;
        public bool Using
        {
            get { return @using; }
            set
            {
                if (@using != value)
                {
                    @using = value;
                    // 更新工具柜
                    // TODO: 容易造成 死循环 
                    //DataProvider.Instance.AddOrUpdateTool(this);
                }
            }
        }

        public int CabinetId { get; set; }
    }

    public class SimpleUser
    {
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置用户工号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 获取或设置用户卡号
        /// </summary>
        public int? CardNo { get; set; }

        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
        public string Name { get; set; }

        public int UserGroupId { get; set; }

        public string UserGroup
        {
            get
            {
                UserGroup group = DataProvider.Instance.UserGroupList.FirstOrDefault(i => i.ID == UserGroupId);
                return group?.Name;
            }
        }
    }

    /// <summary>
    /// 用户授权记录
    /// </summary>
    [BsonIgnoreExtraElements]
    class UserAuthRecord
    {
        public UserAuthRecord()
        {

        }

        public UserAuthRecord(int trackid, IEnumerable<int> positionIds, int userNo, IssueType type)
        {
            PositionIds = new List<int>();
            if (positionIds != null)
            {
                PositionIds.AddRange(positionIds);
            }
            TrackId = trackid;
            UserNo = userNo;
            IssueType = type;
            IssueTime = DateTime.Now;
        }

        [JsonProperty("Id")]
        public long ID { get; set; }

        [JsonIgnore]
        public int TrackId { get; set; }

        [BsonIgnore]
        public string Track
        {
            get
            {
                Track track = DataProvider.Instance.TrackList.FirstOrDefault(i => i.ID == TrackId);
                return track?.Name;
            }
        }

        [JsonIgnore]
        public List<int> PositionIds { get; set; }

        [BsonIgnore]
        public List<string> Positions
        {
            get
            {
                List<string> positions = new List<string>(PositionIds.Count);
                DataProvider provider = DataProvider.Instance;
                PositionIds.ForEach(i =>
                {
                    Position position = provider.GetPosition(i);
                    if (position != null)
                    {
                        positions.Add(position.Index.ToString());
                    }
                });
                return positions;
            }
        }

        [JsonIgnore]
        public int UserNo { get; set; }

        [BsonIgnore]
        public SimpleUser User
        {
            get
            {
                User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.No == UserNo);
                return user?.ToSimpleUser();
            }
        }

        public IssueType IssueType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? IssueTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WorkTime { get; set; }

        public bool Revoked { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? RevokeTime { get; set; }

        public void Revoke()
        {
            Revoked = true;
            RevokeTime = DateTime.Now;
        }
    }

    [BsonIgnoreExtraElements]
    class FaultMessage
    {
        [JsonProperty("Id")]
        public long ID { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

        [BsonIgnore]
        public string Track
        {
            get
            {
                Track track = DataProvider.Instance.TrackList.FirstOrDefault(i => i.ID == Index);
                return track?.Name;
            }
        }

        public string FaultCode = null;// { get; set; }

        public bool Handled { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        public string HandledBy = null;

        [BsonIgnore]
        public string HandledUser
        {
            get
            {
                User user = DataProvider.Instance.UserList.FirstOrDefault(i => i.No == Convert.ToInt32(HandledBy));
                return user?.Name;
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? HandledTime = null;

        public string Remarks { get; set; }

    }

    [BsonIgnoreExtraElements]
    class TrainNumberRecord
    {
        [JsonProperty("Id")]
        public long ID { get; set; }

        [JsonIgnore]
        public int TrackId { get; set; }

        [BsonIgnore]
        public string Track
        {
            get
            {
                Track track = DataProvider.Instance.TrackList.FirstOrDefault(i => i.ID == TrackId);
                return track?.Name;
            }
        }

        public string TrainNumber;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }
    }
}
