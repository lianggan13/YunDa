using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlgorithmServer.Model;
using Nancy;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace AlgorithmServer.Model
{
    public class BoxInfo
    {
        public BoxInfo(IEnumerable<Point> points)
        {
            Name = "";
            Num = points.Count();
            X = points.Select(i => i.X);
            Y = points.Select(i => i.Y);
        }

        public string Name { get; set; }

        public int Num { get; set; }

        public IEnumerable<int> X { get; private set; }

        public IEnumerable<int> Y { get; private set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
