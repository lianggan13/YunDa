using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Y.ASIS.Common.Models;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.Server.Models
{
    /// <summary>
    /// 摄像头视频
    /// </summary>
    [BsonIgnoreExtraElements]
    public class VideoStream : IVideoStream
    {
        //[JsonProperty("Id")]
        public int ID { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public VideoType Type { get; set; }

        public string Url { get; set; }

        public int Channel { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Model { get; set; }

        public string Extension { get; set; }
    }
}
