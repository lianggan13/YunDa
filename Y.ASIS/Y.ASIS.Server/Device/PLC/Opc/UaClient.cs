using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Y.ASIS.Server.Device;

namespace Y.ASIS.Server.Communication
{
    class UaClient
    {
        private readonly ApplicationConfiguration config;
        private readonly ConfiguredEndpoint configuredEndpoint;
        private readonly UserIdentity userIdentity;
        private readonly IEnumerable<MonitorNode> monitorNodes;

        private Session session;

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    if (isConnected)
                    {
                        Connected?.Invoke(this, null);
                    }
                    else
                    {
                        Disconnected?.Invoke(this, null);
                    }
                }
            }
        }

        public int ReConnectionInterval { get; set; } = 1000;

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public UaClient(string address, string username, string password)
        {
            // NuGet 程序包: OPCFoundation.NetStandard.Opc.Ua
            userIdentity = new UserIdentity(username, password);
            configuredEndpoint = new ConfiguredEndpoint(null, new EndpointDescription(address));
            config = new ApplicationConfiguration()
            {
                ApplicationName = "OPC UA Client.",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreType = @"Windows",
                        StorePath = @"CurrentUser\YunDa",
                        SubjectName = @"CN="
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = @"Windows",
                        StorePath = @"CurrentUser\TrustedUser",
                    },
                    NonceLength = 32,
                    AutoAcceptUntrustedCertificates = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 600000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
            };
            config.Validate(ApplicationType.Client);
            config.CertificateValidator.CertificateValidation += (s, e) =>
            {
                e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted);
            };
        }

        public UaClient(string address, string username, string password, IEnumerable<MonitorNode> monitorNodes)
            : this(address, username, password)
        {
            this.monitorNodes = monitorNodes;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                if (session != null)
                {
                    session.KeepAlive -= OnKeepAlive;
                    session.Close();
                    session.Dispose();
                    session = null;
                }
                try
                {
                    session = Session.Create(config, configuredEndpoint, true, "Default.", 60000, userIdentity, null);
                    IsConnected = true;
                    session.KeepAlive += OnKeepAlive;
                    if (monitorNodes != null && monitorNodes.Any())
                    {
                        Subscription subscription = new Subscription(session.DefaultSubscription)
                        {
                            PublishingInterval = 500,
                            LifetimeCount = 2400,
                            KeepAliveCount = 10,
                            MaxNotificationsPerPublish = 65536,
                            PublishingEnabled = true,
                            Priority = 0
                        };
                        foreach (MonitorNode node in monitorNodes)
                        {
                            NodeId nodeId = new NodeId(node.Identifier, node.NamespaceIndex);
                            MonitoredItem item = new MonitoredItem()
                            {
                                NodeClass = NodeClass.Variable,
                                StartNodeId = nodeId,
                                MonitoringMode = MonitoringMode.Reporting,
                                SamplingInterval = 250,
                                QueueSize = 1,
                                DiscardOldest = true
                            };
                            item.Notification += (s, e) =>
                            {
                                MonitoredItemNotification notify = (MonitoredItemNotification)e.NotificationValue;
                                node.Callback?.Invoke(notify.Value.WrappedValue.Value); // PLC 节点回调
                            };
                            subscription.AddItem(item);
                        }
                        session.AddSubscription(subscription);
                        subscription.Create();
                    }

                }
                catch (Exception ex)
                {
                    IsConnected = false;
                    Thread.Sleep(ReConnectionInterval);
                    Start();
                    //throw ex;
                }
            });
        }

        public bool TryGetValue<T>(object identifier, ushort namespaceIndex, out T value)
        {
            value = default;
            if (session == null || !IsConnected)
            {
                return false;
            }
            try
            {
                NodeId nodeId = new NodeId(identifier, namespaceIndex);
                DataValue dataValue = session.ReadValue(nodeId);
                value = (T)dataValue.Value;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool TryWriteValue<T>(object identifier, ushort namespaceIndex, T value)
        {
            if (session == null || !IsConnected)
            {
                return false;
            }
            try
            {
                NodeId nodeId = new NodeId(identifier, namespaceIndex);
                WriteValueCollection writeValues = new WriteValueCollection();
                WriteValue writeValue = new WriteValue()
                {
                    NodeId = nodeId,
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(value))
                };

                writeValues.Add(writeValue);
                ResponseHeader resp = session.Write(null, writeValues, out StatusCodeCollection status, out DiagnosticInfoCollection info);
                bool success = status != null & status.Count > 0 && !status.Any(s => StatusCode.IsBad(s));
                return success;
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                return false;
            }
        }

        public DataValueCollection GetAllValues()
        {
            var ids = monitorNodes.Select(m =>
            {
                NodeId nid = new NodeId(m.Identifier, m.NamespaceIndex);
                return new ReadValueId() { NodeId = nid, AttributeId = Attributes.Value };
            });

            ReadValueIdCollection readValueIds = new ReadValueIdCollection(ids);
            session.Read(null, 0, TimestampsToReturn.Server, readValueIds, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);

            return results;
        }

        private void OnKeepAlive(Session session, KeepAliveEventArgs e)
        {
            IsConnected = ServiceResult.IsGood(e.Status);
            if (!IsConnected)
            {
                Start();
            }
        }
    }

    public class MonitorNode
    {
        public MonitorNode(PLCNodeType nodeType, object identifier, ushort namespaceIndex, Action<dynamic> callback)
        {
            NodeType = nodeType;
            Identifier = identifier;
            NamespaceIndex = namespaceIndex;
            Callback = callback;
        }

        public PLCNodeType NodeType { get; private set; }

        public object Identifier { get; private set; }

        public ushort NamespaceIndex { get; private set; }

        public Action<dynamic> Callback { get; private set; }
    }
}
