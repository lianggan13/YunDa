using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Y.ASIS.Common.Utils;
using Y.ASIS.Server.Device.ToolBox;

namespace Y.ASIS.Server.MainThread.Components
{
    public class ToolBoxService
    {
        public static ToolBoxService Instance { get; } = new ToolBoxService();
        public ToolBoxService()
        {

        }

        private Process cmdExe = null;
        public void Start()
        {
            var java = GetJavaToolBox();
            if (java != null)
            {
                return;
            }

            // 启动工具柜子 java 程序
            string serverPath = Path.Combine(Environment.CurrentDirectory, "ToolBox", "YunDaKeyServer_20211013.jar");
            string cmd = $"java -jar {serverPath}";
            //cmdExe = Process.Start(CmdPath, "/K " + cmd);
            cmdExe = new Process();
            cmdExe.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmdExe.StartInfo.Arguments = "/k " + cmd;
            cmdExe.Start();

            // 启动 工具柜 监听程序
            ToolBoxManager.Instance.Running = true;
        }

        public void Stop()
        {
            var java = GetJavaToolBox();
            if (java != null)
            {
                java.Kill();
            }

            if (cmdExe != null && !cmdExe.HasExited)
            {
                cmdExe.Kill();
            }

            // 停止 工具柜 监听程序
            ToolBoxManager.Instance.Running = false;
        }


        private Process GetJavaToolBox()
        {
            var ports = PortUtil.GetProcessPorts();
            var port = ports.FirstOrDefault(p => p.PortNumber == "8082" && p?.Process.ProcessName == "java");
            return port?.Process;
        }
    }
}
