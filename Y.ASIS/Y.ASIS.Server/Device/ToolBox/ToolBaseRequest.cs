using Y.ASIS.Common.Manager;
using Y.ASIS.Server.Services;

namespace Y.ASIS.Server.Device.ToolBox
{
    abstract class ToolBaseRequest : BaseRequest
    {
        private readonly static string baseUrl;

        static ToolBaseRequest()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("ToolBox.Server.Host");
        }

        public ToolBaseRequest(string path)
            : base(baseUrl, path)
        {

        }
    }
}
