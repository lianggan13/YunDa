using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Y.ASIS.Server.Install
{
    [RunInstaller(true)]
    public partial class ASISServiceInstaller : Installer
    {
        private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;
        public static bool InstallMe()
        {
            bool result;
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] {
                    _exePath
                });
            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.Fatal(ex.Message, ex);
                return result;
            }
            result = true;
            return result;
        }

        public static bool UninstallMe()
        {
            bool result;
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] {
                    "/u", _exePath
                });
            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.Fatal(ex.Message, ex);
                return result;
            }
            result = true;
            return result;
        }

        public ASISServiceInstaller()
        {
            InitializeComponent();

            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            // Setup the Service Account type per your requirement
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            serviceInstaller.ServiceName = "Y.ASIS.Server";
            serviceInstaller.DisplayName = "安全连锁服务";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.Description = $"{serviceInstaller.ServiceName} installed at {DateTime.Now}.";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }


}
