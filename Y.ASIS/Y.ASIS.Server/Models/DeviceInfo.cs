using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.CompilerServices;
using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Server.Database;

namespace Y.ASIS.Server.Models
{
    [BsonIgnoreExtraElements]
    public class DeviceInfo : DynamicBson
    {
        public int ID { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DeviceType Type { get; set; }

        public string Ip { get; set; }

        public int? Port { get; set; }

        public string Extension { get; set; }

        //[JsonIgnore]
        //[BsonIgnore]
        //public override bool CanUpdate { get; set; }

        protected override void UpdateProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            throw new NotImplementedException();
        }
    }
}
