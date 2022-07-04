using AlgorithmServer.Algorithm.DetectAlgorithm;
using AlgorithmServer.Common;
using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace AlgorithmServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += Domain_UnhandledException;
            try
            {
                HIKNVRClient.Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #region Test
            try
            {

                //Tester.TestCapture();

                Tester.Init();
                Tester.TestSdkTrain();
                Tester.TestSdkSafety();
                Tester.TestPersonnel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
            return;
            #endregion

            ClothCheckAlgorithm.Init();
            PersonNumAlgorithm.Init();
            new SafetyChainAlgorithm();  // SafetyChainAlgorithm.Init();
            TrainCheckAlgorithm.Init();

            LogHelper.DebugUpper($"Algoritm interfaces init success.");

            string address = LocalConfigManager.GetAppSettingValue("MainService.Address");
            Url url = new Url(address);
            HostConfiguration hostConfiguration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            try
            {
                NancyHost host = new NancyHost(hostConfiguration, url);
                host.Start();
                Console.WriteLine($"{nameof(AlgorithmServer)} is running on " + url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(AlgorithmServer)} startup fail. error: " + ex.Message);
                //Environment.Exit(0);
            }

            Console.ReadKey();
        }

        private static void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Task.Delay(100);
            if (e.ExceptionObject is Exception ex)
            {
                LogHelper.DebugSys($"{ex}", LogDisplay.Both);
            }
        }

        private static void FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
