using AlgorithmServer;
using AlgorithmServer.Algorithm.DetectAlgorithm;
using AlgorithmServer.Model;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using static AlgorithmServer.Algorithm.DetectAlgorithm.SafetyChainAlgorithm;

public class Tester
{
    static Dictionary<int, string> dict = new Dictionary<int, string>();
    static Tester()
    {
        dict.Add(0, "隔离开关-分闸");
        dict.Add(1, "隔离开关-合闸");
        dict.Add(2, "");
        dict.Add(3, "");
        dict.Add(4, "接地杆-已撤接");
        dict.Add(5, "接地杆-已挂接");
        dict.Add(10, "验电杆-已撤接");
        dict.Add(11, "验电杆-已挂接");
    }

    public static void Init()
    {
        new SafetyChainAlgorithm();
        TrainCheckAlgorithm.Init();
        PersonNumAlgorithm.Init();
    }

    public static void TestSdkTrain()
    {
        #region
        {
            TestSdkTrain("101776", $"二楼_车号.jpeg");
            TestSdkTrain("HXD1C0624", $"7道-车号(隔离开关侧).png");
            TestSdkTrain("XD1C0046", $"10道-车号(隔离开关侧).png");
        }
        #endregion
    }


    public static void TestSdkTrain(string target, string name)
    {
        string dir1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string dir2 = Path.Combine(dir1, "Test", "Images");
        //string file = Path.Combine(dir2, $"{track}道-{dict[k]}.png");
        string file = Path.Combine(dir2, $"{name}");
        //string file = Path.Combine(dir2, $"二楼_车号.jpeg"); 

        MethodParam param = new MethodParam();

        MemoryStream ms = new MemoryStream();
        Image image = Image.FromFile(file);
        image.Save(ms, ImageFormat.Jpeg);

        param.Mat = Mat.FromStream(ms, ImreadModes.AnyColor);

        IntPtr p = TrainCheckAlgorithm.Recognize(param.Mat.CvPtr);
        string result = Marshal.PtrToStringAnsi(p);

        Console.WriteLine($"---------------------------");
        Console.WriteLine($"测试内容:{Path.GetFileName(file)}");
        Console.WriteLine($"测试结果:{result}");
        Console.WriteLine($"期望结果:{target}");
        string pass = $"{result}" == target ? "通过√" : "有误×";
        Console.WriteLine($"测试{pass}");
        Console.WriteLine($"---------------------------");
    }


    public static void TestPersonnel()
    {
        {
            TestPersonnel(0, $"6道-通道南-人员.png");
        }

        {
            double w = 1000;
            double h = 560;
            OpenCvSharp.Point point1 = new OpenCvSharp.Point(0, 0);
            OpenCvSharp.Point point2 = new OpenCvSharp.Point(w, 0);
            OpenCvSharp.Point point3 = new OpenCvSharp.Point(w, h);
            OpenCvSharp.Point point4 = new OpenCvSharp.Point(0, h);
            OpenCvSharp.Point point5 = new OpenCvSharp.Point(0, 0);

            var points = new List<OpenCvSharp.Point>() { point1, point2, point3, point4, point5 };

            TestPersonnel(1, $"7道-通道北-人员.png", points);
        }

    }

    public static void TestPersonnel(int target, string name, IEnumerable<OpenCvSharp.Point> points = null)
    {
        if (points == null)
            points = CreateMaxPoints();
        string dir1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string dir2 = Path.Combine(dir1, "Test", "Images");
        //string file = Path.Combine(dir2, $"7道-{dict[k]}.png"); // 7道-通道北-人员.png
        //string file = Path.Combine(dir2, $"{track}道-{dict[k]}.png");
        string file = Path.Combine(dir2, $"{name}");

        MethodParam param = new MethodParam();

        MemoryStream ms = new MemoryStream();
        Image image = Image.FromFile(file);
        image.Save(ms, ImageFormat.Jpeg);

        param.Mat = Mat.FromStream(ms, ImreadModes.AnyColor);

        BoxInfo boxInfo = new BoxInfo(points);
        string ss = boxInfo.ToString();
        int result = PersonNumAlgorithm.Detect(param.Mat.CvPtr, ss);
        Console.WriteLine($"---------------------------");
        Console.WriteLine($"测试内容:{Path.GetFileName(file)}");
        string txt = result == 1 ? "有人" : "无人";
        Console.WriteLine($"测试结果:{result}-{txt}");
        string txt2 = target == 1 ? "有人" : "无人";
        Console.WriteLine($"期望结果:{target}-{txt2}");
        string pass = result == target ? "通过√" : "有误×";
        Console.WriteLine($"测试{pass}");
        Console.WriteLine($"---------------------------");
    }



    public static void TestSdkSafety()
    {
        #region 6-道
        {
            int track = 6;
            // 隔离开关
            //TestSdkSafety(0, CreateMaxPoints());
            //TestSdkSafety(1, CreateMaxPoints());
            // 接地杆
            TestSdkSafety(track, 4, CreateMaxPoints());
            TestSdkSafety(track, 5, CreateMaxPoints()); // -3 识别不到
                                                        // 验电杆
            TestSdkSafety(track, 10, CreateMaxPoints());
            TestSdkSafety(track, 11, CreateMaxPoints());
        }
        #endregion

        #region 7-道
        {
            int track = 7;
            // 隔离开关
            TestSdkSafety(track, 0, CreateMaxPoints());
            TestSdkSafety(track, 1, CreateMaxPoints());
            // 接地杆
            TestSdkSafety(track, 4, CreateRightPoints());
            TestSdkSafety(track, 5, CreateRightPoints());
            // 验电杆
            TestSdkSafety(track, 10, CreateMaxPoints());
            TestSdkSafety(track, 11, CreateMaxPoints());
        }
        #endregion
    }

    // 原始分辨率(像素): 1920x1088
    const double w = 1900;
    const double h = 1000;

    public static void TestSdkSafety(int track, int k, IEnumerable<OpenCvSharp.Point> points)
    {
        string dir1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string dir2 = Path.Combine(dir1, "Test", "Images");
        //string file = Path.Combine(dir2, $"7道-{dict[k]}.png");
        string file = Path.Combine(dir2, $"{track}道-{dict[k]}.png");

        MethodParam param = new MethodParam();

        MemoryStream ms = new MemoryStream();
        Image image = Image.FromFile(file);
        image.Save(ms, ImageFormat.Jpeg);

        param.Mat = Mat.FromStream(ms, ImreadModes.AnyColor);

        BoxInfo boxInfo = new BoxInfo(points);
        string ss = boxInfo.ToString();

        int result = Recognition(param.Mat.CvPtr, ss);
        Console.WriteLine($"---------------------------");
        Console.WriteLine($"测试内容:{Path.GetFileName(file)}");
        Console.WriteLine($"测试结果:{result}");
        Console.WriteLine($"期望结果:{k}");
        string pass = result == k ? "通过√" : "有误×";
        Console.WriteLine($"测试{pass}");
        Console.WriteLine($"---------------------------");
    }


    public static IEnumerable<OpenCvSharp.Point> CreateMaxPoints()
    {
        OpenCvSharp.Point point1 = new OpenCvSharp.Point(0, 0);
        OpenCvSharp.Point point2 = new OpenCvSharp.Point(w, 0);
        OpenCvSharp.Point point3 = new OpenCvSharp.Point(w, h);
        OpenCvSharp.Point point4 = new OpenCvSharp.Point(0, h);
        OpenCvSharp.Point point5 = new OpenCvSharp.Point(0, 0);

        var points = new List<OpenCvSharp.Point>() { point1, point2, point3, point4, point5 };

        return points;
    }

    public static IEnumerable<OpenCvSharp.Point> CreateRightPoints()
    {
        OpenCvSharp.Point point1 = new OpenCvSharp.Point(w / 2, 0);
        OpenCvSharp.Point point2 = new OpenCvSharp.Point(w, 0);
        OpenCvSharp.Point point3 = new OpenCvSharp.Point(w, h);
        OpenCvSharp.Point point4 = new OpenCvSharp.Point(w / 2, h);
        OpenCvSharp.Point point5 = new OpenCvSharp.Point(w / 2, 0);

        var points = new List<OpenCvSharp.Point>() { point1, point2, point3, point4, point5 };

        return points;
    }

    public static IEnumerable<OpenCvSharp.Point> CreateLeftPoints()
    {
        OpenCvSharp.Point point1 = new OpenCvSharp.Point(0, 0);
        OpenCvSharp.Point point2 = new OpenCvSharp.Point(w / 2, 0);
        OpenCvSharp.Point point3 = new OpenCvSharp.Point(w / 2, h);
        OpenCvSharp.Point point4 = new OpenCvSharp.Point(0, h);
        OpenCvSharp.Point point5 = new OpenCvSharp.Point(0, 0);

        var points = new List<OpenCvSharp.Point>() { point1, point2, point3, point4, point5 };

        return points;
    }

    public static void TestCapture()
    {
        while (true)
        {
            Console.Write("Input NVR Channel: ");
            string chan = Console.ReadLine();
            byte[] data = HIKNVRClient.CaptureCache(int.Parse(chan));

            string path = ImageHelper.SaveImage(data, $"Channel_{chan}_{DateTime.Now:yyyy-MM-dd}.png");

            Console.WriteLine(path);

            MethodParam param = new MethodParam();
            using (MemoryStream stream = new MemoryStream(data))
            {
                param.Mat = Mat.FromStream(stream, ImreadModes.AnyColor);
                param.Image = Image.FromStream(stream);
            }

        }


    }
}
