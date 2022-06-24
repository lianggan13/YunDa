using Newtonsoft.Json.Linq;
using System;
using Y.ASIS.Server.Models;

namespace Y.ASIS.Server.Device.Speaker
{
    /// <summary>
    /// 股道切换板卡
    /// </summary>
    public class Switcher : DeviceBase
    {
        public int ID { get; set; }

        public string Ip { get; set; }

        public int? Port { get; set; }

        /// <summary>
        /// 起始道号
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 可控道数量
        /// </summary>
        public int Count { get; set; }

        public Switcher(DeviceInfo info)
        {
            Info = info;
            try
            {
                JObject jobj = JObject.Parse(info.Extension);
                StartIndex = jobj.Value<int>(nameof(StartIndex));
                Count = jobj.Value<int>(nameof(Count));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Switcher switcher))
            {
                return false;
            }
            return Ip == switcher.Ip;
        }

        public override int GetHashCode()
        {
            return Ip.GetHashCode();
        }
    }
}
