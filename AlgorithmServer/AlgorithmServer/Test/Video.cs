using AlgorithmServer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmServer.Test
{

    public class Track
    {
        public string Name { get; set; }
        public bool HasElsc { get; set; }
        public List<Video> Videos { get; set; }
    }


    public class Video
    {
        public string Name { get; set; }
        public int Channel { get; set; }
        public Algorithms DetectType { get; set; }
        public VideoType VideoType { get; set; }
        public string Target { get; set; }
        public string Coordinate { get; set; }
        [JsonIgnore]
       public  IEnumerable<OpenCvSharp.Point> Points { get; set; }

    }

    public enum VideoType
    {
        /// <summary>
        /// 受电弓
        /// </summary>
        Pantograph,

        /// <summary>
        /// 车号
        /// </summary>
        TrainNo,

        /// <summary>
        /// 隔离开关
        /// </summary>
        Isolation,

        /// <summary>
        /// 接地装置
        /// </summary>
        Grounding,

        /// <summary>
        /// 验电装置
        /// </summary>
        Elec,

        /// <summary>
        /// 平台
        /// </summary>
        Platform,

        /// <summary>
        /// 渡板
        /// </summary>
        Gangway,

        /// <summary>
        /// 门禁
        /// </summary>
        Door,

        /// <summary>
        /// 未知
        /// </summary>
        UnKnow,
    }

    public class Target
    {
        public bool HasElsc { get; set; }
        public VideoType VideoType { get; set; }
        public string Value { get; set; }

      
    }
}
