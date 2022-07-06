using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using Y.ASIS.Common.Manager;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Server.Database;

namespace Y.ASIS.Server.Common
{
    public static class ServerGlobal
    {
        public static User CurrentUser { get; set; }

        public static ProjectType Project { get; set; } = ProjectType.Shenzhen12;
        public static string ExecuteDirectory { get; }

        public static string ServerHostUrl { get; }

        public static string PhotoDirectory { get; }

        public static string PhotoUrlPrefix { get; }

        static ServerGlobal()
        {
            ExecuteDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ServerHostUrl = LocalConfigManager.GetAppSettingValue("MainService.Address");

            PhotoDirectory = "./Photos";
            if (!Directory.Exists(PhotoDirectory))
                Directory.CreateDirectory(PhotoDirectory);

            PhotoUrlPrefix = LocalConfigManager.GetAppSettingValue("PhotoUrlPrefix");



        }

        private static IPAddress GetLocalIP()
        {
            IPAddress ipAddr = null;
            var nics = NetworkInterface.GetAllNetworkInterfaces().Where(adt =>
                              adt.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                              adt.OperationalStatus == OperationalStatus.Up);

            foreach (var nic in nics)
            {
                IPInterfaceProperties ipProp = nic.GetIPProperties();
                if (ipProp.GetIPv4Properties().IsDhcpEnabled)
                    continue;

                var ip = ipProp.UnicastAddresses.Where(i => i.Address.AddressFamily == AddressFamily.InterNetwork).LastOrDefault();
                if (ip != null)
                {
                    ipAddr = ip.Address;
                    break;
                }
            }

            return ipAddr;
        }
    }
}
