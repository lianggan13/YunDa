using Y.ASIS.Common.Manager;

namespace Y.ASIS.App.Communication
{
    public abstract class AuthRequest : ApiRequest
    {
        private static readonly string AuthKey;

        static AuthRequest()
        {
            AuthKey = LocalConfigManager.GetAppSettingValue("AuthKey");
        }

        public AuthRequest(string path)
            : base(path)
        {
            headers["AuthKey"] = AuthKey;
        }
    }
}
