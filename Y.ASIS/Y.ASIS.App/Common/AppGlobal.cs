using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Y.ASIS.App.ViewModels;
using Y.ASIS.Common.Models.Enums;


public enum AppEnvironment
{
    Development,
    Production
}

public class AppGlobal
{
    private static AppGlobal instance;
    public static AppGlobal Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppGlobal();
            }
            return instance;
        }
    }

    public ProjectType Project { get; private set; }

    public static AppEnvironment Env { get; set; }

    public string ExecuteDirectory { get; }

    private readonly object locker = new object();

    public ConcurrentDictionary<string, object> Data { get; private set; }

    public MainViewModel MainVM { get; set; }

    private AppGlobal()
    {
        Data = new ConcurrentDictionary<string, object>();
        Project = (ProjectType)Enum.Parse(typeof(ProjectType), Y.ASIS.App.Properties.Settings.Default.Project);
        ExecuteDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }

    public object GetData(string key)
    {
        object obj = null;
        lock (locker)
        {
            Data.TryGetValue(key, out obj);
        }

        return obj;
    }

    public void SetData(string key, object obj)
    {
        lock (locker)
        {
            Data[key] = obj;
        }
    }

    public IEnumerable<string> Keys(string pattern)
    {
        foreach (string key in Data.Keys)
        {
            if (!string.IsNullOrEmpty(pattern))
            {
                if (key.StartsWith(pattern))
                    yield return key;
            }
            else
                yield return key;
        }
    }

    public bool HasLogin()
    {
        return MainVM?.CurrentUser != null;
    }


    public static string CreatePhotoPath(string subDir, string imageName)
    {
        string photoBaseDir = Path.Combine(Environment.CurrentDirectory, "Photos", subDir);

        if (!Directory.Exists(photoBaseDir))
            Directory.CreateDirectory(photoBaseDir);

        DirectoryInfo info = new DirectoryInfo(photoBaseDir);
        var now = DateTime.Now;
        var folders = info.GetDirectories().Where(d => now.Subtract(d.CreationTime).TotalDays > 7);
        foreach (var folder in folders)
        {
            folder.Delete(true);
        }

        string photoDir = Path.Combine(photoBaseDir, $"{now:yyyy-MM-dd}");
        if (!Directory.Exists(photoDir))
            Directory.CreateDirectory(photoDir);

        string photoPath = Path.Combine(photoDir, imageName);

        return photoPath;
    }
}
