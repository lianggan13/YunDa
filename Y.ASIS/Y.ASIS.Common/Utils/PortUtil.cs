using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;


namespace Y.ASIS.Common.Utils
{
    public class PortUtil
    {
        private const string PortReleaseGuid = "8875BD8E-4D5B-11DE-B2F4-691756D89593";

        /// <summary>
        /// 查询给定端口是否为可用的TCP端口
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static bool IsAvailableTcpPort(int port)
        {
            Mutex mutex = new Mutex(false, "Global" + PortReleaseGuid);
            mutex.WaitOne();
            try
            {
                IPGlobalProperties ipGlobalProperties =
                    IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints =
                    ipGlobalProperties.GetActiveTcpListeners();
                return endPoints.Any(i => i.Port == port);
            }
            catch
            {
                return false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 查询给定端口是否为可用的UASIS端口
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static bool IsAvailableUdpPort(int port)
        {
            Mutex mutex = new Mutex(false, "Global" + PortReleaseGuid);
            mutex.WaitOne();
            try
            {
                IPGlobalProperties ipGlobalProperties =
                    IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints =
                    ipGlobalProperties.GetActiveUdpListeners();
                return endPoints.Any(i => i.Port == port);
            }
            catch
            {
                return false;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public static int GetAvailableTcpUdpPort(int startPort = 10000)
        {
            Mutex mutex = new Mutex(false, "Global" + PortReleaseGuid);
            mutex.WaitOne();
            try
            {
                IPGlobalProperties ipGlobalProperties =
                    IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] tcpEndPoints =
                    ipGlobalProperties.GetActiveTcpListeners();
                IPEndPoint[] udpEndPoints =
                    ipGlobalProperties.GetActiveUdpListeners();
                for (int i = startPort; i < 65536; i++)
                {
                    if (!tcpEndPoints.Any(j => j.Port == i) && !udpEndPoints.Any(j => j.Port == i))
                    {
                        return i;
                    }
                }
                return -1;
            }
            catch
            {
                return -1;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 获取所有进程占用的端口
        /// </summary>
        /// <returns></returns>
        public static List<Port> GetProcessPorts()
        {
            var ports = new List<Port>();
            var process = Process.GetProcesses();
            try
            {
                using (Process p = new Process())
                {
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.Arguments = "-a -n -o";
                    ps.FileName = "netstat.exe";
                    ps.UseShellExecute = false;
                    ps.WindowStyle = ProcessWindowStyle.Hidden;
                    ps.RedirectStandardInput = true;
                    ps.RedirectStandardOutput = true;
                    ps.RedirectStandardError = true;

                    p.StartInfo = ps;
                    p.Start();

                    StreamReader stdOutput = p.StandardOutput;
                    StreamReader stdError = p.StandardError;

                    string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    string exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0")
                    {
                        // Command Errored. Handle Here If Need Be
                    }

                    //Get The Rows
                    string[] rows = Regex.Split(content, "\r\n");
                    foreach (string row in rows)
                    {
                        //Split it baby
                        string[] tokens = Regex.Split(row, "\\s+");
                        if (tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                        {
                            string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                            Port port = new Port
                            {
                                Protocol = localAddress.Contains("1.1.1.1") ? String.Format("{0}v6", tokens[1]) : String.Format("{0}v4", tokens[1]),
                                PortNumber = localAddress.Split(':')[1],
                                ProcessId = tokens[1] == "UDP" ? Convert.ToInt32(tokens[4]) : Convert.ToInt32(tokens[5]),
                            };
                            port.Process = process.FirstOrDefault(pp => pp.Id == port.ProcessId);
                            ports.Add(port);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            return ports;
        }
    }

    public class Port
    {
        public string Protocol { get; set; }
        public string PortNumber { get; set; }
        public int ProcessId { get; set; }
        public Process Process { get; set; }
    }
}
