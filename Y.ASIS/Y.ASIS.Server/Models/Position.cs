using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using Y.ASIS.Server.Database;

namespace Y.ASIS.Server.Models
{
    /// <summary>
    /// 描述一个列位
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Position
    {
        [JsonProperty("Id")]
        public int ID { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public OpcConfig OpcConfig { get; set; }

        public List<VideoStream> Videos { get; set; }

        public List<VideoStream> ExtraVideos { get; set; }

        [BsonIgnore]
        public List<int> SpeakerIds { get; set; }

        public List<int> DeviceIds { get; set; }

        private PositionState state;
        [JsonIgnore]
        public PositionState State
        {
            get
            {
                if (state == null)
                {
                    state = new PositionState(ID);
                }
                return state;
            }
        }
    }

}
