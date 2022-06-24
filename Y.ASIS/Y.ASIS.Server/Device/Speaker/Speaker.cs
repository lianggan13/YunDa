using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Y.ASIS.Common.Models;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Services;
using Y.ASIS.Server.Models;

namespace Y.ASIS.Server.Device.Speaker
{

    class Speaker : DeviceBase
    {
        public int TerminalId { get; private set; }

        private DeviceState state;
        public DeviceState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;

                    DeviceStateMessage dmsg = new DeviceStateMessage()
                    {
                        ID = Info.ID,
                        Name = Info.Name,
                        State = state,
                        Type = Info.Type,
                        Extention = Info.Extension,
                    };

                    PushMessage message = new PushMessage(PushDataType.Device, dmsg);
                    PushTaskService.Instance.Push(message, 10);
                }
            }
        }

        public Speaker(DeviceInfo info)
        {
            Info = info;
            try
            {
                JObject jobj = JObject.Parse(info.Extension);
                TerminalId = jobj.Value<int>(nameof(TerminalId));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
    }

    class SpeakerInfoResponse
    {
        [JsonProperty("res")]
        public string Code { get; set; }

        [JsonProperty("rows")]
        public List<SpeakerInfo> Data { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        public bool Success
        {
            get { return Code == "1"; }
        }
    }

    class SpeakerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ip")]
        public string ServerIp { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("bcoutv")]
        public string BroadcastOutVolume { get; set; }

        [JsonProperty("bcinv")]
        public string BroadcastInVolume { get; set; }

        [JsonProperty("talkoutv")]
        public string TalkOutVolume { get; set; }

        [JsonProperty("talkinv")]
        public string TalkInVolume { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("localip")]
        public string Ip { get; set; }

        [JsonProperty("task")]
        public string Task { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("nexttask")]
        public string NextTask { get; set; }

        [JsonProperty("rtspurl")]
        public string RtspUrl { get; set; }

        [JsonProperty("panels")]
        public string Panels { get; set; }

        [JsonProperty("deviceinfo")]
        public string Info { get; set; }

        [JsonProperty("nvrurl")]
        public string NvrUrl { get; set; }
    }

    class SpeakerTaskResponse
    {
        [JsonProperty("res")]
        public string Code { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        public bool Success
        {
            get { return Code == "1"; }
        }
    }
}
