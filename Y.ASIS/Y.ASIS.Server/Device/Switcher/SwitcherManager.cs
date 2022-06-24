using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Y.ASIS.Common.Communication;

namespace Y.ASIS.Server.Device.Speaker
{
    class SwitcherManager
    {
        public static SwitcherManager Instance { get; } = new SwitcherManager();

        private readonly Dictionary<Switcher, ModbusTcpClient> clients;
        private readonly Timer timer;

        private SwitcherManager()
        {
            clients = new Dictionary<Switcher, ModbusTcpClient>();
            timer = new Timer(CheckState, null, 0, 1000);
        }

        private void CheckState(object _)
        {
            foreach (KeyValuePair<Switcher, ModbusTcpClient> pair in clients)
            {
                bool success = pair.Value.ReadHoldingRegister(5, out short value);
                if (success)
                {
                    LogHelper.Info(pair.Key.Ip + ": " + value);
                }
                else
                {
                    LogHelper.Warn(pair.Key.Ip + ": 获取继电器状态错误");
                }
            }
        }

        public void Register(Switcher switcher)
        {
            if (clients.ContainsKey(switcher))
            {
                return;
            }
            int port = switcher.Port == null ? 502 : (int)switcher.Port;
            ModbusTcpClient client = new ModbusTcpClient(switcher.Ip, port)
            {
                EnableNetTouch = true,
                ReadWriteTimeout = 1000
            };
            client.Connected += (s, e) =>
            {
                LogHelper.Info(">> " + e.Ip + " 建立连接");
            };

            client.Disconnected += (s, e) =>
            {
                LogHelper.Warn(">> " + e.Ip + " 断开连接");
            };
            client.Start();
            clients.Add(switcher, client);
        }

        public bool Switch(int index)
        {
            IEnumerable<KeyValuePair<Switcher, ModbusTcpClient>> switchers =
                clients.Where(i => i.Key.StartIndex <= index && i.Key.StartIndex + i.Key.Count > index);

            if (!switchers.Any())
            {
                return false;
            }

            bool flag = switchers.Any(i => !i.Value.IsConnected);
            if (flag)
            {
                return false;
            }

            foreach (KeyValuePair<Switcher, ModbusTcpClient> pair in switchers)
            {
                flag = pair.Value.ReadHoldingRegister(13, out short sn);
                if (!flag)
                {
                    return false;
                }

                flag = pair.Value.WriteSingleRegister(1, (short)index);
                if (!flag)
                {
                    return false;
                }
                short[] data = new short[] { 1, sn };
                flag = pair.Value.WriteMultipleRegisters(2, data);
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
