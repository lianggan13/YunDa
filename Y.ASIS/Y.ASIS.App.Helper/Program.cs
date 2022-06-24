using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Y.ASIS.Common.Communication;

namespace Y.ASIS.App.Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            KillProcessByPort(5565);
            if (args.Length == 0)
            {
                Console.WriteLine("Y.ASIS.App.Helper Startup!");
                Console.WriteLine("> param 1: port to receive heartbeat of app.");
                Console.WriteLine("> param 2: need to be closed port when app die.");
            }
            if (args.Length == 2)
            {
                try
                {
                    int udpPort = Convert.ToInt32(args[0]);
                    int actionPort = Convert.ToInt32(args[1]);
                    UdpServer udpServer = new UdpServer(udpPort);
                    DateTime last = DateTime.Now;
                    udpServer.ReceivedData += (s, e) =>
                    {
                        Console.WriteLine("Heartbeat ~ ~");
                        last = DateTime.Now;
                    };
                    udpServer.Start();
                    while ((DateTime.Now - last).Seconds < 5d)
                    {
                        Thread.Sleep(1000);
                    }
                    udpServer.Dispose();
                    KillProcessByPort(actionPort);
                }
                catch
                {
                    return;
                }
            }
        }

        static void KillProcessByPort(int port)
        {
            Process pro = new Process();

            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            
            pro.StandardInput.WriteLine("netstat -ano | findstr " + port);
            pro.StandardInput.WriteLine("exit");

            Regex reg = new Regex(@"\s ", RegexOptions.Compiled);
            string line = null;
            while ((line = pro.StandardOutput.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("TCP", StringComparison.OrdinalIgnoreCase))
                {
                    line = reg.Replace(line, ",");
                    string[] arr = line.Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
                    if (arr[1].EndsWith(":" + port))
                    {
                        KillProcess(Convert.ToInt32(arr[4]));
                    }
                    continue;
                }
                else if(line.StartsWith("UDP", StringComparison.OrdinalIgnoreCase))
                {
                    line = reg.Replace(line, ",");
                    string[] arr = line.Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
                    if (arr[1].EndsWith(":" + port))
                    {
                        KillProcess(Convert.ToInt32(arr[3]));
                    }
                    continue;
                }
            }
        }

        static void KillProcess(int pid)
        {
            string processName = "Unknown";
            try
            {
                Process process = Process.GetProcessById(pid);
                processName = process.ProcessName;
                if (!process.CloseMainWindow())
                {
                    process.Kill();
                }
                Console.WriteLine("close process {0} success.", processName);
            }
            catch
            {
                Console.WriteLine("close process {0} fail.", processName);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
