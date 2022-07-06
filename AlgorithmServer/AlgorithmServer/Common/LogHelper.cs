using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Text;

namespace AlgorithmServer.Common
{
    public class LogHelper
    {
        private static log4net.ILog sysLogger = null;         //写日志实体
        private static log4net.ILog uiLogger = null;          //ui日志实体
        private static log4net.ILog testLogger = null;        //测试日志实体

        /// <summary>
        /// 构造函数
        /// </summary>
        static LogHelper()
        {
            // Appender2 系统调试日志
            log4net.Filter.LevelRangeFilter levfilter2 = new log4net.Filter.LevelRangeFilter();
            levfilter2.LevelMax = log4net.Core.Level.Debug;
            levfilter2.LevelMin = log4net.Core.Level.Debug;
            levfilter2.ActivateOptions();
            log4net.Appender.RollingFileAppender infoAppender = new log4net.Appender.RollingFileAppender();
            infoAppender.File = Path.Combine(System.Environment.CurrentDirectory, "Log\\");
            infoAppender.AppendToFile = true;
            infoAppender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Composite;
            infoAppender.DatePattern = "'DEBUG'-yyyyMMdd.LOG";
            infoAppender.StaticLogFileName = false;
            infoAppender.MaxFileSize = 1024 * 1024 * 1024;
            infoAppender.MaxSizeRollBackups = 10;
            infoAppender.Name = "InfoAppender";
            infoAppender.AddFilter(levfilter2);
            // UI
            UiLogAppender uiAppender = new UiLogAppender();
            uiAppender.AddFilter(levfilter2);

            //layout  
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
            layout.Header = "------ Application Started ------" + Environment.NewLine;
            layout.Footer = "------ Application Exited ------" + Environment.NewLine + Environment.NewLine;

            //  
            infoAppender.Layout = layout;
            uiAppender.Layout = layout;

            infoAppender.ActivateOptions();
            uiAppender.ActivateOptions();

            // 
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            log4net.Repository.ILoggerRepository uiRepository = hierarchy.Root.Repository;
            log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository("SysLog");

            log4net.Config.BasicConfigurator.Configure(repository, infoAppender);
            log4net.Config.BasicConfigurator.Configure(uiRepository, uiAppender);

            sysLogger = log4net.LogManager.GetLogger(repository.Name, "sys");
            uiLogger = log4net.LogManager.GetLogger(uiRepository.Name, "ui");
        }

        public static void Init()
        {
            // 会自动调用 LogHelper() 构造方法
        }

        public static void CreateTestLog(string path)
        {
            if (File.Exists(path + "\\Test.LOG")) return;//tis&trp 会进入两次，导致当tis和trp同时测时候日志文件无日志
            testLogger = null;
            log4net.Filter.LevelRangeFilter levfilter = new log4net.Filter.LevelRangeFilter();
            levfilter.LevelMax = log4net.Core.Level.Debug;
            levfilter.LevelMin = log4net.Core.Level.Debug;
            levfilter.ActivateOptions();
            // Appender1  
            log4net.Appender.FileAppender appender1 = new log4net.Appender.RollingFileAppender();
            appender1.File = Path.Combine(path, "Test.LOG");
            appender1.ImmediateFlush = true;
            appender1.Encoding = Encoding.UTF8;
            appender1.AppendToFile = true;
            appender1.Name = "TestAppender";
            appender1.AddFilter(levfilter);
            //layout  
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
            //  
            appender1.Layout = layout;
            appender1.ActivateOptions();
            //  
            log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            log4net.Config.BasicConfigurator.Configure(repository, appender1);
            testLogger = log4net.LogManager.GetLogger(repository.Name, "testing");
        }

        public static void ShutDownTestLog()
        {
            if (testLogger != null)
            {
                testLogger.Logger.Repository.Shutdown();
                testLogger = null;
            }
        }

        /// <summary>
        /// 系统调试日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void DebugSys(string message, LogDisplay display = LogDisplay.OnlyWrite)
        {
            DateTime now = DateTime.Now;
            if ((display == LogDisplay.Both || display == LogDisplay.OnlyDisplay) &&
                uiLogger.IsInfoEnabled)
            {
                UiLogEventArgs arg = new UiLogEventArgs(LogLevel.Debug, message, now);
                uiLogger.Debug(arg);
            }
            if ((display == LogDisplay.Both || display == LogDisplay.OnlyWrite) &&
                sysLogger.IsInfoEnabled)
            {
                message = string.Format("[{0} {1}] {2}", now.ToString("yyyy-MM-dd HH:mm:ss.fff"), LogLevel.Debug.ToString(), message);
                sysLogger.Debug(message);
            }
        }

        /// <summary>
        /// 添加了 ChengDu 前缀的系统日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void DebugUpper(string message, LogDisplay display = LogDisplay.OnlyWrite)
        {
            DebugSys("Upper: " + message, display);
        }

        /// <summary>
        /// 添加了 ChengDu 前缀的系统日志
        /// </summary>
        /// <param name="ex">异常</param>
        public static void DebugUpper(System.Exception ex)
        {
            string message = ex.Message.ToString() + "/r/n" + ex.Source.ToString() + "/r/n" + ex.TargetSite.ToString() + "/r/n" + ex.StackTrace.ToString();
            DebugUpper(message);
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Test(LogLevel logLevel, string message, LogDisplay display)
        {
            DateTime now = DateTime.Now;
            if ((display == LogDisplay.Both || display == LogDisplay.OnlyDisplay) &&
                uiLogger.IsInfoEnabled)
            {
                UiLogEventArgs arg = new UiLogEventArgs(logLevel, message, now);
                uiLogger.Debug(arg);
            }
            if ((display == LogDisplay.Both || display == LogDisplay.OnlyWrite) &&
                testLogger != null &&
                testLogger.IsDebugEnabled)
            {
                message = string.Format("[{0} {1}] {2}", now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logLevel.ToString(), message);
                testLogger.Debug(message);
            }
        }

        /// <summary>
        /// 警告日志 暂时不用
        /// </summary>
        /// <param name="message">日志内容</param>
        private static void Warn(string message)
        {
            if (sysLogger.IsWarnEnabled)
            {
                sysLogger.Warn(message);
            }
        }

        /// <summary>
        /// 致命错误日志 暂时不用
        /// </summary>
        /// <param name="message">日志内容</param>
        private static void Fatal(string message)
        {
            if (sysLogger.IsFatalEnabled)
            {
                sysLogger.Fatal(message);
            }
        }

        public class UiLogEventArgs : EventArgs
        {
            public DateTime Time { get; private set; }

            public string Message { get; private set; }

            public LogLevel Level { get; private set; }

            public UiLogEventArgs(LogLevel Level, string message, DateTime time)
            {
                this.Level = Level;
                this.Message = message;
                this.Time = time;
            }
        }

        public class UiLogAppender : AppenderSkeleton
        {
            public event EventHandler<UiLogEventArgs> UiLogReceived;

            protected override void Append(LoggingEvent loggingEvent)
            {
                UiLogEventArgs arg = (UiLogEventArgs)loggingEvent.MessageObject;
                OnUiLogReceived(arg);
            }
            protected virtual void OnUiLogReceived(UiLogEventArgs e)
            {
                if (UiLogReceived != null)
                    UiLogReceived(this, e);
            }
        }

    }

    public enum LogDisplay
    {
        OnlyDisplay = 0,
        OnlyWrite,
        Both
    }

    public enum LogLevel
    {
        Info = 0,
        Debug,
        Warn,
        Error
    }
}
