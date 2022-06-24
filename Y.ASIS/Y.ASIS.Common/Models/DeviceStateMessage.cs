using Y.ASIS.Common.Models.Enums;
using Y.ASIS.Models.Enums;

namespace Y.ASIS.Common.Models
{
    public class DeviceStateMessage
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DeviceType Type { get; set; }

        public DeviceState State { get; set; }

        public string Extention { get; set; }

    }
}
