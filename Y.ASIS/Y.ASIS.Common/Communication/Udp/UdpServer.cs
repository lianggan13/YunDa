using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Communication
{
    public class UdpServer : SocketBase
    {
        private readonly int localPort;

        private readonly List<IPEndPoint> listeners;
        public UdpServer(int port)
        {
            localPort = port;
            listeners = new List<IPEndPoint>();
        }

        public override void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint local = new IPEndPoint(IPAddress.Any, localPort);
            socket.Bind(local);
            running = true;
            socket.EnableBroadcast = true;
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UASIS_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            socket.IOControl((int)SIO_UASIS_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            DoBeginReceive();
        }

        private void DoBeginReceive()
        {
            if (running)
            {
                byte[] buffer = new byte[BufferSize];
                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult result = socket.BeginReceiveFrom(buffer, 0, BufferSize, SocketFlags.None, ref remote, ReceiveAsyncCallback, buffer);
            }
        }

        private void ReceiveAsyncCallback(IAsyncResult result)
        {
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            string ip = string.Empty;
            int port = 0;
            try
            {
                int length = socket.EndReceiveFrom(result, ref remote);
                (remote as IPEndPoint).GetIpAndPort(out ip, out port);
                if (length > 0)
                {
                    byte[] data = result.AsyncState as byte[];
                    OnReceivedData(ip, port, data.Take(length).ToArray());
                }
            }
            catch (Exception e)
            {
                OnException(e, ip, port);
            }
            finally
            {
                DoBeginReceive();
            }
        }

        public void AddListener(string ip, int port)
        {
            int index = listeners.FindIndex(i => i.Address.MapToIPv4().ToString() == ip && i.Port == port);
            if (index == -1)
            {
                listeners.Add(new IPEndPoint(IPAddress.Parse(ip), port));
            }
        }

        public void RemoveListener(string ip, int port)
        {
            int index = listeners.FindIndex(i => i.Address.MapToIPv4().ToString() == ip && i.Port == port);
            if (index != -1)
            {
                listeners.RemoveAt(index);
            }
        }

        public void Broadcast(byte[] data)
        {
            if (!running)
            {
                return;
            }
            foreach (IPEndPoint listener in listeners)
            {
                socket.SendTo(data, listener);
            }
        }

        public Task BroadcastAsync(byte[] data)
        {
            return Task.Run(() =>
            {
                Broadcast(data);
            });
        }

        public bool Send(string ip, int port, byte[] data)
        {
            if (running)
            {
                try
                {
                    socket.SendTo(data, 0, data.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse(ip), port));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public Task<bool> SendAsync(string ip, int port, byte[] data)
        {
            return Task.Run(() =>
            {
                return Send(ip, port, data);
            });
        }
    }
}