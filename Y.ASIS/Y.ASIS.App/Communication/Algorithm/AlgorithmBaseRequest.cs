using Y.ASIS.Common.Manager;

namespace Y.ASIS.App.Communication
{
    abstract class AlgorithmBaseRequest : BaseRequest
    {
        private readonly static string baseUrl;
        protected readonly static string Authkey;

        static AlgorithmBaseRequest()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("Algorithm.Server.Host");
            Authkey = LocalConfigManager.GetAppSettingValue("Algorithm.Server.Authkey");
        }

        public AlgorithmBaseRequest(string path)
            : base(baseUrl, path)
        {

        }
    }
}
