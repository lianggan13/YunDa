using Y.ASIS.Common.Manager;

namespace Y.ASIS.App.Communication
{
    /// <summary>
    /// 访问API接口的基类
    /// </summary>
    public abstract class ApiRequest : BaseRequest
    {
        private static readonly string baseUrl;

        static ApiRequest()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("Y.ASIS.Server.Host");
        }


        public ApiRequest(string path)
            : base(baseUrl, path)
        {
        }
    }
}
