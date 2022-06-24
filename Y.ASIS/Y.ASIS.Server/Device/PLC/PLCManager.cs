using System.Collections.Generic;

namespace Y.ASIS.Server.Device
{
    /// <summary>
    /// 主要用于管理PLC集群之间的通信
    /// </summary>
    class PLCManager
    {
        private static PLCManager instance;
        public static PLCManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PLCManager();
                }
                return instance;
            }
        }

        public Dictionary<int, PLC> PLCDictionary { get; }

        private PLCManager()
        {
            PLCDictionary = new Dictionary<int, PLC>();
        }

        public void Register(PLC plc)
        {
            PLCDictionary[plc.Position.ID] = plc;
        }

        public bool GetPlcByPositionId(int positionId, out PLC plc)
        {
            return PLCDictionary.TryGetValue(positionId, out plc);
        }
    }
}
