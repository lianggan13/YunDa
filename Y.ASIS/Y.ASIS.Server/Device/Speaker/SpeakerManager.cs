using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.Communication;
using Y.ASIS.Common.Manager;
using Y.ASIS.Models.Enums;
using Y.ASIS.Server.Database;

namespace Y.ASIS.Server.Device.Speaker
{
    class SpeakerManager
    {
        private readonly static string SwitchOffText;
        private readonly static string SwitchOnText;
        private readonly static string EvacuateText;
        private readonly static string[] PositionName = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        static SpeakerManager()
        {
            SwitchOffText = LocalConfigManager.GetAppSettingValue("Speaker.SwitchOff");
            SwitchOnText = LocalConfigManager.GetAppSettingValue("Speaker.SwitchOn");
            EvacuateText = LocalConfigManager.GetAppSettingValue("Speaker.Evacuate");
        }

        private static SpeakerManager instance;

        public static SpeakerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpeakerManager();
                }
                return instance;
            }
        }

        private readonly string baseUrl;
        public readonly Dictionary<int, Speaker> Devices;
        private SpeakerManager()
        {
            baseUrl = LocalConfigManager.GetAppSettingValue("Speaker.Server");
            Devices = new Dictionary<int, Speaker>();
            CheckSpeakerState();
        }

        public void Register(Speaker speaker)
        {
            int id = speaker.Info.ID;
            if (!Devices.ContainsKey(id))
            {
                Devices[id] = speaker;
            }
        }

        private void CheckSpeakerState()
        {
            TimerManager.Instance.AddSchedule(() =>
            {
                List<SpeakerInfo> spks = GetSpeakerInfo();
                spks.ForEach(s =>
                {
                    int tid = int.Parse(s.Id);
                    var device = Devices.Values.FirstOrDefault(d => d.TerminalId == tid);

                    if (device != null && Devices.ContainsKey(device.Info.ID))
                    {
                        Devices[device.Info.ID].State = s.State == "1" ? DeviceState.Online : DeviceState.Offline;
                    }
                });
            }, TimeSpan.FromSeconds(3d));
        }

        private List<SpeakerInfo> GetSpeakerInfo()
        {
            int pageCount = 10;
            string url = baseUrl + "/php/getsingleterminaldata.php";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "jsondata[pageCount]", pageCount.ToString() },
                { "jsondata[simple]", "1" }
            };
            //List<int> keys = Devices.Keys.ToList();
            List<int> keys = Devices.Values.Select(d => d.TerminalId).ToList();
            List<List<int>> blocks = GetBlockList(keys, pageCount);
            List<SpeakerInfo> result = new List<SpeakerInfo>();
            foreach (List<int> ids in blocks)
            {
                parameters["jsondata[searchTxt]"] = string.Join(",", ids) + ",";
                SpeakerRequest request = new SpeakerRequest(url, parameters, 1000);
                SpeakerInfoResponse resp = request.Request<SpeakerInfoResponse>();
                if (resp != null && resp.Success)
                {
                    result.AddRange(resp.Data);
                }
            }
            return result;
        }

        private List<List<T>> GetBlockList<T>(List<T> list, int blockSize)
        {
            List<List<T>> result = new List<List<T>>();
            List<T> temp = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                temp.Add(list[i]);
                if ((i + 1) % blockSize == 0 && i > 0)
                {
                    result.Add(temp);
                    temp = new List<T>();
                }
                if (i == list.Count - 1)
                {
                    result.Add(temp);
                }
            }
            return result;
        }


        public bool SwitchOff(List<int> ids, string positionId)
        {
            // modify temporarily
            int.TryParse(positionId, out int posId);
            var tarck = DataProvider.Instance.GetTrackByPosId(posId);
            var tarckNo = string.Empty;//股道号
            var positionName = string.Empty;//列位名（A段、B段）
            if (tarck != null)
            {
                tarckNo = tarck.No + "";
                if (tarck.Positions.Any() && tarck.Positions.Count > 1)
                {
                    tarck.Positions = tarck.Positions.OrderBy(x => x.ID).ToList();
                    var index = tarck.Positions.IndexOf(new Models.Position() { ID = posId });
                    if (index >= 0) positionName = PositionName[index] + "段";
                }
            }
            // ，各单位请注意，{0}股道{1}断电警示
            // ，各单位请注意，19 股道 B 段断电警示
            string text = string.Format(SwitchOffText, tarckNo, positionName);
            return Start(ids, text);
        }

        public bool SwitchOn(List<int> ids, string positionId)
        {
            // modify temporarily
            int.TryParse(positionId, out int posId);
            var tarck = DataProvider.Instance.GetTrackByPosId(posId);
            var tarckNo = string.Empty;//股道号
            var positionName = string.Empty;//列位名（A段、B段）
            if (tarck != null)
            {
                tarckNo = tarck.No + "";
                if (tarck.Positions.Any() && tarck.Positions.Count > 1)
                {
                    tarck.Positions = tarck.Positions.OrderBy(x => x.ID).ToList();
                    var index = tarck.Positions.IndexOf(new Models.Position() { ID = posId });
                    if (index >= 0) positionName = PositionName[index] + "段";
                }
            }

            string text = string.Format(SwitchOnText, tarckNo, positionName);
            return Start(ids, text);
        }

        public bool Evacuate(List<int> ids, string positionId)
        {
            // modify temporarily
            int.TryParse(positionId, out int posId);
            var tarck = DataProvider.Instance.GetTrackByPosId(posId);
            var tarckNo = string.Empty;//股道号
            var positionName = string.Empty;//列位名（A段、B段）
            if (tarck != null)
            {
                tarckNo = tarck.No + "";
                if (tarck.Positions.Any() && tarck.Positions.Count > 1)
                {
                    tarck.Positions = tarck.Positions.OrderBy(x => x.ID).ToList();
                    var index = tarck.Positions.IndexOf(new Models.Position() { ID = posId });
                    if (index >= 0) positionName = PositionName[index] + "段";
                }
            }

            string text = string.Format(EvacuateText, tarckNo, positionName);
            return Start(ids, text);
        }


        public bool Start(List<int> ids, string text)
        {
            IEnumerable<KeyValuePair<string, string>> parameters = new Dictionary<string, string>()
            {
                // startbct: 开始广播
                // stopbct:  停止广播
                { "jsondata[rtype]", "startbct" },
                // 接收广播终端列表
                { "jsondata[param1]", string.Join("<", ids) },
                // 音源类型（0：文件；1：终端；2：声卡；3：文本）
                { "jsondata[param2]", "3" },
                { "jsondata[param4]", text },
                // 广播次数
                { "jsondata[param7]", "3" },
                // 广播文本输速
                { "jsondata[param8]", "0" }
            };
            string url = baseUrl + "/php/exeRealPlayFile.php";
            SpeakerRequest request = new SpeakerRequest(url, parameters);
            SpeakerTaskResponse resp = request.Request<SpeakerTaskResponse>();
            return resp != null && resp.Success && !resp.Guid.IsNullOrEmptyOrWhiteSpace();
        }

        public bool RealPlay(IEnumerable<int> terminalIds, string text, int times = 3)
        {
            IEnumerable<KeyValuePair<string, string>> parameters = new Dictionary<string, string>()
            {
                // startbct: 开始广播
                // stopbct:  停止广播
                { "jsondata[rtype]", "startbct" },
                // 接收广播终端列表
                { "jsondata[param1]", string.Join("<", terminalIds) },
                // 音源类型（0：文件；1：终端；2：声卡；3：文本）
                { "jsondata[param2]", "3" },
                { "jsondata[param4]", text },
                // 广播次数
                { "jsondata[param7]",$"{times}" },
                // 广播文本输速
                { "jsondata[param8]", "0" }
            };
            string url = baseUrl + "/php/exeRealPlayFile.php";
            SpeakerRequest request = new SpeakerRequest(url, parameters);
            SpeakerTaskResponse resp = request.Request<SpeakerTaskResponse>();
            return resp != null && resp.Success && !resp.Guid.IsNullOrEmptyOrWhiteSpace();
        }


    }
}

