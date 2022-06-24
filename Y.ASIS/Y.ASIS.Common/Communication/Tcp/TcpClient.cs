using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Communication
{
    public class TcpClient : TcpBase
    {
        private readonly string remoteIp;
        private readonly int remotePort;
        private readonly EndPoint remote;

        public bool IsConnected { get; set; }

        public TcpClient(string remoteIp, int remotePort)
            : base()
        {
            this.remoteIp = remoteIp;
            this.remotePort = remotePort;
            this.remote = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
        }

        public override void Start()
        {
            running = true;
            DoBeginConnect();
        }

        private async void DoBeginConnect()
        {
            while (running)
            {
                if (IsConnected && socket.Connected)
                {
                    if (EnableNetTouch && !remoteIp.Ping())
                    {
                        IsConnected = false;
                        OnDisconnected(remoteIp, remotePort);
                    }
                    else
                    {
                        await Task.Delay(ConnectCheckInterval);
                        continue;
                    }
                }
                if (socket != null)
                {
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(remote, ConnectAsyncCallback, remote);
                break;
            }
        }

        private void ConnectAsyncCallback(IAsyncResult result)
        {
            try
            {
                socket.EndConnect(result);
                if (socket.Connected)
                {
                    IsConnected = true;
                    OnConnected(remoteIp, remotePort);
                    Task.Factory.StartNew(DoBeginReceive);
                }
            }
            catch (Exception e)
            {
                IsConnected = false;
                OnException(e, remoteIp, remotePort);
            }
            finally
            {
                DoBeginConnect();
            }
        }

        private void DoBeginReceive()
        {
            while (running && IsConnected)
            {
                try
                {
                    if (socket.Poll(ReceiveTimeoutMilliseconds * 1000, SelectMode.SelectRead))
                    {
                        byte[] buffer = new byte[BufferSize];
                        socket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveAsyncCallback, buffer);
                        break;
                    }
                    else
                    {
                        OnReceiveTimeout(remoteIp, remotePort);
                    }
                }
                catch (Exception e)
                {
                    IsConnected = false;
                    OnDisconnected(remoteIp, remotePort);
                    OnException(e, remoteIp, remotePort);
                }
            }
        }

        private void ReceiveAsyncCallback(IAsyncResult result)
        {
            try
            {
                int bytes = socket.EndReceive(result);
                if (bytes > 0)
                {
                    byte[] data = result.AsyncState as byte[];
                    if (bytes == 0)
                    {
                        IsConnected = false;
                    }
                    else if (bytes > 0)
                    {
                        OnReceivedData(remoteIp, remotePort, data.Take(bytes).ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                IsConnected = false;
                OnException(e, remoteIp, remotePort);
            }
            finally
            {
                DoBeginReceive();
            }
        }

        public virtual bool Send(byte[] data)
        {
            if (socket != null && IsConnected)
            {
                socket.Send(data);
                return true;
            }
            return false;
        }

        public virtual Task<bool> SendAsync(byte[] data)
        {
            return Task.Run(() =>
            {
                return Send(data);
            });
        }
    }
}
