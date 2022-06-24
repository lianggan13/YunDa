using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Y.ASIS.Server.Device.Attendance
{
    public enum AttendanceType
    {
        /// <summary>
        /// 进门
        /// </summary>
        In,

        /// <summary>
        /// 出门
        /// </summary>
        Out,

        /// <summary>
        /// 销权
        /// </summary>
        Revoke
    }

    enum AttendanceCommandType
    {
        AKE,
        GetRequest,
        RecogniseResult,
        Return,

        GetDeviceInfo,
        GetEmployeeID,
        SetEmployee,
        DeleteEmployee,
        SyncTime
    }

    class AttendanceCommand
    {
        public AttendanceResponse Command { get; set; }

        public bool Executing { get; set; }
    }

    class AttendanceRequest
    {
        [JsonProperty("COMMAND")]
        public AttendanceCommandType Command { get; set; }

        [JsonProperty("PARAM")]
        public JObject Data { get; set; }
    }

    class AttendanceRecord
    {
        /// <summary>
        /// 设备标识
        /// </summary>
        [JsonProperty("sn")]
        public string SN { get; set; }

        /// <summary>
        /// 身份号码
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// true人员模板 false比对记录
        /// </summary>
        [JsonProperty("isUserUpdate")]
        public string IsUserUpdate { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 人证机类型 eg:R007
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 人证机人脸识别算法版本号
        /// </summary>
        [JsonProperty("alg_edition")]
        public string AlgEdition { get; set; }

        /// <summary>
        /// 人员照片 base64码
        /// </summary>
        [JsonProperty("capturejpg")]
        public string Capturejpg { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        [JsonProperty("score")]
        public string Score { get; set; }

        /// <summary>
        /// 1/0(通过/不通过)
        /// </summary>
        [JsonProperty("pass")]
        public string Pass { get; set; }

        /// <summary>
        /// 阈值
        /// </summary>
        [JsonProperty("threshold")]
        public string Threshold { get; set; }

        /// <summary>
        /// 刷卡时间
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }

        /// <summary>
        /// 认证方式 0=仅人脸 1=人脸+IC卡 2=人脸或IC卡 3=仅IC卡 4=人脸+ID卡 
        /// 5=人脸或ID卡 6=IC卡+ID卡 7=IC卡或ID卡 8=设备认证（设置中可动态选择，即设备设置中选的是哪一个，就按照哪一个）
        /// </summary>
        [JsonProperty("recogType")]
        public string RecogType { get; set; }

        /// <summary>
        /// 识别方式 0=指纹 1=1:1 2=云端1:N 3=本地1:N
        /// </summary>
        [JsonProperty("identifyType")]
        public string IdentifyType { get; set; }

        /// <summary>
        /// 体温
        /// </summary>
        [JsonProperty("animalHeat")]
        public string AnimalHeat { get; set; }

        /// <summary>
        /// 是否带口罩 (1:佩戴，2:未佩戴)
        /// </summary>
        [JsonProperty("wearMask")]
        public string WearMask { get; set; }
    }

    class AttendanceResponse
    {
        public AttendanceResponse(AttendanceCommandType command)
        {
            Command = command.ToString();
        }

        [JsonProperty("RETURN")]
        public string Command { get; private set; }

        [JsonProperty("PARAM")]
        public object Data { get; set; }
    }

    class AttendanceResponseData
    {
        public AttendanceResponseData()
        {
            Result = "success";
        }

        [JsonProperty("result")]
        public string Result { get; set; }
    }

    class AttendanceHeartbeatData : AttendanceResponseData
    {
        public AttendanceHeartbeatData()
            : base()
        {
            Key = new string[] { "123456" };
        }

        [JsonProperty("key")]
        public string[] Key { get; private set; }

        [JsonProperty("sn")]
        public string SN { get; set; }
    }

    class AttendanceDeviceInfoCommandData : AttendanceResponseData
    {
        public AttendanceDeviceInfoCommandData()
            : base()
        {
            Command = AttendanceCommandType.GetDeviceInfo.ToString();
        }

        [JsonProperty("command")]
        public string Command { get; private set; }
    }

    class AttendanceSyncTimeCommandData : AttendanceResponseData
    {
        public AttendanceSyncTimeCommandData()
            : base()
        {
            Command = AttendanceCommandType.SyncTime.ToString();
        }

        [JsonProperty("command")]
        public string Command { get; private set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

    class AttendanceAddOrUpdateUserCommandData : AttendanceResponseData
    {
        public AttendanceAddOrUpdateUserCommandData()
            : base()
        {
            Id = "";
            Capturejpg = "";
            RecogPermission = "2";
            //RecogPermission = "8";
            ThroughStartTime = "";
            ThroughFinisthTime = "";
            AccessCard = "";
            Command = AttendanceCommandType.SetEmployee.ToString();
            UserType = "1";
            FaceData = new string[] { "" };
        }

        [JsonProperty("command")]
        public string Command { get; private set; }

        /// <summary>
        /// 人员身份证号
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 照片地址
        /// </summary>
        [JsonProperty("photourl")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// base64 string的照片
        /// </summary>
        [JsonProperty("capturejpg")]
        public string Capturejpg { get; private set; }

        /// <summary>
        /// 人脸模板数组
        /// </summary>
        [JsonProperty("face_data")]
        public string[] FaceData { get; private set; }

        /// <summary>
        /// 用户类型 0= 全部， 1=员工，2=访客，3=黑名单，4=业主
        /// </summary>
        [JsonProperty("userType")]
        public string UserType { get; private set; }

        /// <summary>
        /// 人员工号
        /// </summary>
        [JsonProperty("job_num")]
        public string WorkNo { get; set; }

        /// <summary>
        /// 设备权限 0=仅人脸 1=人脸+IC卡 2=人脸或IC卡 3=仅IC卡 4=人脸+ID卡 
        /// 5=人脸或ID卡 6=IC卡+ID卡 7=IC卡或ID卡 8=设备认证（设置中可动态选择，即设备设置中选的是哪一个，就按照哪一个）
        /// </summary>
        [JsonProperty("recogPermission")]
        public string RecogPermission { get; set; }

        /// <summary>
        /// IC卡号
        /// </summary>
        [JsonProperty("icCard")]
        public string IcCard { get; set; }

        /// <summary>
        /// 权限持有开始时间
        /// </summary>
        [JsonProperty("throughStartTime")]
        public string ThroughStartTime { get; }

        /// <summary>
        /// 权限持有结束时间
        /// </summary>
        [JsonProperty("throughFinisthTime")]
        public string ThroughFinisthTime { get; }

        /// <summary>
        /// 门禁号
        /// </summary>
        [JsonProperty("accessCard")]
        public string AccessCard { get; }
    }

    class AttendanceDeleteUserCommandData : AttendanceResponseData
    {
        public AttendanceDeleteUserCommandData()
            : base()
        {
            Command = AttendanceCommandType.DeleteEmployee.ToString();
        }

        [JsonProperty("command")]
        public string Command { get; private set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("id")]
        public string WorkNo { get; set; }
    }
}
