using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Y.ASIS.Server.Install;
using Y.ASIS.Server.Services.Main;

namespace Y.ASIS.Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Tester.TestIPVoice();
            bool runservice = !(Environment.UserInteractive || Debugger.IsAttached);
            if (runservice)
            {
                RunAsAService();
            }
            else
            {
                if (args != null && args.Length > 0)
                {
                    if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                    {
                        // Admin cmd: Y.ASIS.Server.exe -i
                        ASISServiceInstaller.InstallMe();
                    }
                    else
                    {
                        if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                        {
                            // Admin cmd: Y.ASIS.Server.exe -u (or sc delete Y.ASIS.Server)
                            ASISServiceInstaller.UninstallMe();
                        }
                        else
                        {
                            LogHelper.Fatal("Invalid argument!");
                        }
                    }
                }
                else
                {
                    RunAsAConsole();
                }
            }
        }

        static void RunAsAConsole()
        {
            //Console.Clear();
            ManualResetEvent loop = new ManualResetEvent(false);

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                LogHelper.Info($"Application exited at {DateTime.Now}.");
            };

            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                loop.Set();
            };

            try
            {
                new MainService().Start();
                LogHelper.Info($"Application started. Press Ctrl+C to shut down.");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"Application failed to start", ex);
            }

            loop.WaitOne();

            Environment.Exit(0);
        }

        static void RunAsAService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new MainService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
