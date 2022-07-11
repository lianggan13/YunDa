using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmServer;
using AlgorithmServer.Algorithm.DetectAlgorithm;
using AlgorithmServer.Model;
using AlgorithmServer.Test;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;


namespace AlgorithmServer.Test
{
    public static class TestHelper
    {
        public static List<Target> Targets { get; }

        static TestHelper()
        {
            Target target1 = new Target()
            {
                HasElsc = false,
                VideoType = VideoType.Isolation,
                Value = "0",
            };
            Target target2 = new Target()
            {
                HasElsc = true,
                VideoType = VideoType.Isolation,
                Value = "1",
            };
            Target target3 = new Target()
            {
                HasElsc = false,
                VideoType = VideoType.Pantograph,
                Value = "2",
            };
            Target target4 = new Target()
            {
                HasElsc = true,
                VideoType = VideoType.Pantograph,
                Value = "3",
            };
            Target target5 = new Target()
            {
                HasElsc = true,
                VideoType = VideoType.Grounding,
                Value = "4",
            };
            Target target6 = new Target()
            {
                HasElsc = false,
                VideoType = VideoType.Grounding,
                Value = "5",
            };
            Target target7 = new Target()
            {
                HasElsc = true,
                VideoType = VideoType.Elec,
                Value = "10",
            };
            Target target8 = new Target()
            {
                HasElsc = false,
                VideoType = VideoType.Elec,
                Value = "11",
            };
            Targets = new List<Target>()
            {
                 target1,target2,target3,target4,
                 target5,target6,target7,target8,
            };
        }

        public static MethodParam Capture(int chan)
        {
            byte[] data = HIKNVRClient.CaptureCache(chan);

            string path = ImageHelper.SaveImage(data, $"Channel_{chan}_{DateTime.Now:yyyy-MM-dd}.png");

            LogHelper.Info(path);

            MethodParam param = new MethodParam();
            using (MemoryStream stream = new MemoryStream(data))
            {
                param.Mat = Mat.FromStream(stream, ImreadModes.AnyColor);
                param.Image = Image.FromStream(stream);
            }

            return param;
        }

        public static void SetTarget(Track t,Video v)
        {
            if (string.IsNullOrEmpty(v.Target))
            {
                var target = Targets.FirstOrDefault(o => o.HasElsc == t.HasElsc
                                       && o.VideoType == v.VideoType);
                v.Target = target.Value;
            }

        }

        public static void SetCoordinate(Video v)
        {
            if (string.IsNullOrEmpty(v.Coordinate))
            {
                v.Points = CreateMaxPoints();
            
                if(v.Channel == 82)
                {
                    v.Points = CreatePoints(1, 1, 3,colspan:2);
                }
            }
            else
            {
                JObject jobj = JObject.Parse(v.Coordinate);
                var xx = jobj["X"].ToObject<List<int>>();
                var yy = jobj["Y"].ToObject<List<int>>();
                var points3 = xx.Zip(yy, (x, y) => new { x, y });
                var pointsStr = Newtonsoft.Json.JsonConvert.SerializeObject(points3);
                v.Points = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<OpenCvSharp.Point>>(pointsStr);
            }
        }

        // 原始分辨率(像素): 1920x1088
        const double w = 1900;
        const double h = 1000;

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

        public static IEnumerable<OpenCvSharp.Point> CreatePoints(int index, int rows, int cols,int rowspan=1,int colspan=1)
        {
            double wi = w / cols;
            double hi = h / rows;
            int rowi = index / cols;
            int coli = index % cols;
            double xi = coli * wi;
            double yi = rowi * hi;

            OpenCvSharp.Point point1 = new OpenCvSharp.Point(xi, yi);
            OpenCvSharp.Point point2 = new OpenCvSharp.Point(xi+wi*colspan, yi);
            OpenCvSharp.Point point3 = new OpenCvSharp.Point(xi + wi*colspan, yi+hi*rowspan);
            OpenCvSharp.Point point4 = new OpenCvSharp.Point(xi, yi+hi*rowspan);
            OpenCvSharp.Point point5 = new OpenCvSharp.Point(xi, yi);

            var points = new List<OpenCvSharp.Point>() { point1, point2, point3, point4, point5 };

            return points;
        }

        public static void ShowResult(Video v, string result)
        {
            LogHelper.Info($"---------------------------");
            LogHelper.Info($"测试内容:{v.Name}");
            LogHelper.Info($"测试结果:{result}");
            LogHelper.Info($"期望结果:{v.Target}");
            if ($"{result}" == v.Target)
            {
                LogHelper.Info($"测试通过√");
            }
            else
            {
                LogHelper.Warn($"测试有误×");
            }
            LogHelper.Info($"---------------------------");
        }

    }
}
