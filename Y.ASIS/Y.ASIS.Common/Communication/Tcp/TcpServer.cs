using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Y.ASIS.Common.Communication
{
    public class TcpServer : TcpBase
    {
        private readonly int localPort;
        protected readonly ConcurrentDictionary<string, Socket> clients;

        public int MaxConnectionSize { get; set; } = 100;

        public TcpServer(int localPort)
            : base()
        {
            this.localPort = localPort;
            clients = new ConcurrentDictionary<string, Socket>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            running = true;
            EndPoint local = new IPEndPoint(IPAddress.Any, localPort);
            socket.Bind(local);
            socket.Listen(MaxConnectionSize);
            DoBeginAccept();
            CheckClients();
        }

        private void DoBeginAccept()
        {
            socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket remote = socket.EndAccept(result);
            if (running)
            {
                DoBeginAccept();
            }
            (remote.RemoteEndPoint as IPEndPoint).GetIpAndPort(out string remoteIp, out int remotePort);
            string clientKey = $"{remoteIp}:{remotePort}";
            clients[clientKey] = remote;

            OnConnected(remoteIp, remotePort);
            DoBeginReceive(remote, remoteIp, remotePort);
        }

        private void DoBeginReceive(Socket remote, string remoteIp, int remotePort)
        {
            if (remote == null)
            {
                return;
            }
            string clientKey = $"{remoteIp}:{remotePort}";
            while (running && clients.ContainsKey(clientKey))
            {
                try
                {
                    if (remote.Poll(ReceiveTimeoutMilliseconds * 1000, SelectMode.SelectRead))
                    {
                        byte[] buffer = new byte[BufferSize];
                        TcpClientState state = new TcpClientState()
                        {
                            Remote = remote,
                            Buffer = buffer
                        };
                        remote.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveAsyncCallback, state);
                        //break;
                    }
                    else
                    {
                        OnReceiveTimeout(remoteIp, remotePort);
                    }
                }
                catch (Exception ex)
                {
                    RemoveClient(remoteIp, remotePort);
                    OnException(ex, remoteIp, remotePort);
                }
            }
        }

        private void ReceiveAsyncCallback(IAsyncResult result)
        {
            Socket remote = null;
            string remoteIp = "Unknown";
            int remotePort = -1;
            try
            {
                TcpClientState state = result.AsyncState as TcpClientState;
                remote = state.Remote;
                if (remote == null)
                    return;

                (remote.RemoteEndPoint as IPEndPoint).GetIpAndPort(out remoteIp, out remotePort);

                if (remote?.Connected == false)
                {
                    RemoveClient(remoteIp, remotePort);
                }

                int bytes = remote.EndReceive(result);

                if (bytes == 0)
                {
                    RemoveClient(remoteIp, remotePort);
                }
                else if (bytes > 0)
                {
                    OnReceivedData(remoteIp, remotePort, state.Buffer.Take(bytes).ToArray());
                }
            }
            catch (Exception ex)
            {
                OnException(ex, remoteIp, remotePort);
            }
            finally
            {
                //DoBeginReceive(remote, remoteIp, remotePort);
            }
        }

        public virtual bool Send(string ip, int port, byte[] data)
        {
            string key = ip + ":" + port;
            if (running && clients.ContainsKey(key))
            {
                try
                {
                    clients[key].Send(data);
                    return true;
                }
                catch (Exception e)
                {
                    OnException(e, ip, port);
                }
            }
            return false;
        }

        public virtual Task<bool> SendAsync(string ip, int port, byte[] data)
        {
            return Task.Run(() =>
            {
                return Send(ip, port, data);
            });
        }

        private void CheckClients()
        {
            Task.Factory.StartNew(() =>
            {
                while (running && EnableNetTouch)
                {
                    string[] keys = clients.Keys.ToArray();
                    foreach (string key in keys)
                    {
                        if (key != null && clients.ContainsKey(key))
                        {
                            string[] arr = key.Split(':');
                            if (!arr[0].Ping())
                            {
                                RemoveClient(arr[0], Convert.ToInt32(arr[1]));
                            }
                        }
                    }
                    Thread.Sleep(ConnectCheckInterval);
                }
            });
        }

        public virtual void RemoveClient(string remoteIp, int remotePort)
        {
            string clientKey = $"{remoteIp}:{remotePort}";
            if (clients.TryRemove(clientKey, out Socket client))
            {
                client.Close();
                client.Dispose();
                OnDisconnected(remoteIp, remotePort);
            }
        }
    }

    internal class TcpClientState
    {
        public Socket Remote { get; set; }

        public byte[] Buffer { get; set; }
    }
}
