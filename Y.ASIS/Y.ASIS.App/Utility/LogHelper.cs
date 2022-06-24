using log4net;
using System;

public class LogHelper
{
    public static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");

    public static void Info(string text) => Log.Info(text);

    public static void Info(string text, Exception ex) => Log.Info(text, ex);

    public static void Warn(string text) => Log.Warn(text);

    public static void Warn(string text, Exception ex) => Log.Warn(text, ex);

    public static void Error(string text) => Log.Error(text);

    public static void Error(string text, Exception ex) => Log.Error(text, ex);

    public static void Fatal(string text) => Log.Fatal(text);

    public static void Fatal(string text, Exception ex) => Log.Fatal(text, ex);
}