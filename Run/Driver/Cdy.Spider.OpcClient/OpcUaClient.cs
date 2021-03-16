using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OpcClient
{
    /// <summary>
    /// 一个二次封装了的OPC UA库，支持从opc ua服务器读写节点数据，批量读写，订阅，批量订阅，历史数据读取，方法调用操作。
    /// </summary>
    public class OpcUaClient
    {
        #region Constructors

        /// <summary>
        /// 默认的构造函数，实例化一个新的OPC UA类
        /// </summary>
        public OpcUaClient()
        {
            dic_subscriptions = new Dictionary<string, Subscription>();

            var certificateValidator = new CertificateValidator();
            certificateValidator.CertificateValidation += (sender, eventArgs) =>
            {
                if (ServiceResult.IsGood(eventArgs.Error))
                    eventArgs.Accept = true;
                else if (eventArgs.Error.StatusCode.Code == StatusCodes.BadCertificateUntrusted)
                    eventArgs.Accept = true;
                else
                    LoggerService.Service.Warn("OpcClient",string.Format("Failed to validate certificate with error code {0}: {1}", eventArgs.Error.Code, eventArgs.Error.AdditionalInfo));
            };

            SecurityConfiguration securityConfigurationcv = new SecurityConfiguration
            {
                AutoAcceptUntrustedCertificates = true,
                RejectSHA1SignedCertificates = false,
                MinimumCertificateKeySize = 1024,
            };
            certificateValidator.Update(securityConfigurationcv);

            // Build the application configuration
            var configuration = new ApplicationConfiguration
            {
                ApplicationName = OpcUaName,
                ApplicationType = ApplicationType.Client,
                CertificateValidator = certificateValidator,
                ApplicationUri = "urn:SpiderOpcClient", //Kepp this syntax
                ProductUri = "SpiderOpcClient",

                ServerConfiguration = new ServerConfiguration
                {
                    MaxSubscriptionCount = 100000,
                    MaxMessageQueueSize = 1000000,
                    MaxNotificationQueueSize = 1000000,
                    MaxPublishRequestCount = 10000000,
                },

                SecurityConfiguration = new SecurityConfiguration
                {
                    AutoAcceptUntrustedCertificates = true,
                    RejectSHA1SignedCertificates = false,
                    MinimumCertificateKeySize = 1024,
                    SuppressNonceValidationErrors = true,

                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\My",
                        SubjectName = OpcUaName,
                    },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\Root",
                    },
                    TrustedPeerCertificates = new CertificateTrustList
                    {
                        StoreType = CertificateStoreType.X509Store,
                        StorePath = "CurrentUser\\Root",
                    }
                },

                TransportQuotas = new TransportQuotas
                {
                    OperationTimeout = 6000000,
                    MaxStringLength = int.MaxValue,
                    MaxByteStringLength = int.MaxValue,
                    MaxArrayLength = 65535,
                    MaxMessageSize = 419430400,
                    MaxBufferSize = 65535,
                    ChannelLifetime = -1,
                    SecurityTokenLifetime = -1
                },
                ClientConfiguration = new ClientConfiguration
                {
                    DefaultSessionTimeout = -1,
                    MinSubscriptionLifetime = -1,
                },
                DisableHiResClock = true
            };

            configuration.Validate(ApplicationType.Client);
            m_configuration = configuration;
        }

        #endregion Constructors

        #region Connect And Disconnect

        /// <summary>
        /// connect to server
        /// </summary>
        /// <param name="serverUrl">remote url</param>
        public async Task ConnectServer(string serverUrl)
        {
            m_session = await Connect(serverUrl);
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <returns>The new session object.</returns>
        private async Task<Session> Connect(string serverUrl)
        {
            // disconnect from existing session.
            Disconnect();

            if (m_configuration == null)
            {
                throw new ArgumentNullException("_configuration");
            }

            // select the best endpoint.
            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(serverUrl, UseSecurity);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(m_configuration);

            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

            m_session = await Session.Create(
                m_configuration,
                endpoint,
                false,
                false,
                (string.IsNullOrEmpty(OpcUaName)) ? m_configuration.ApplicationName : OpcUaName,
                60000,
                UserIdentity,
                new string[] { });

            // set up keep alive callback.
            m_session.KeepAlive += new KeepAliveEventHandler(Session_KeepAlive);

            // update the client status
            m_IsConnected = true;

            // raise an event.
            DoConnectComplete(null);

            // return the new session.
            return m_session;
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public void Disconnect()
        {
            UpdateStatus(false, DateTime.UtcNow, "Disconnected");

            // stop any reconnect operation.
            if (m_reConnectHandler != null)
            {
                m_reConnectHandler.Dispose();
                m_reConnectHandler = null;
            }

            // disconnect any existing session.
            if (m_session != null)
            {
                m_session.Close(10000);
                m_session = null;
            }

            // update the client status
            m_IsConnected = false;

            // raise an event.
            DoConnectComplete(null);
        }

        #endregion Connect And Disconnect

        #region Event Handlers

        /// <summary>
        /// Report the client status
        /// </summary>
        /// <param name="error">Whether the status represents an error.</param>
        /// <param name="time">The time associated with the status.</param>
        /// <param name="status">The status message.</param>
        /// <param name="args">Arguments used to format the status message.</param>
        private void UpdateStatus(bool error, DateTime time, string status, params object[] args)
        {
            m_OpcStatusChange?.Invoke(this, new OpcUaStatusEventArgs()
            {
                Error = error,
                Time = time.ToLocalTime(),
                Text = String.Format(status, args),
            });
        }

        /// <summary>
        /// Handles a keep alive event from a session.
        /// </summary>
        private void Session_KeepAlive(Session session, KeepAliveEventArgs e)
        {
            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(session, m_session))
                {
                    return;
                }

                // start reconnect sequence on communication error.
                if (ServiceResult.IsBad(e.Status))
                {
                    if (m_reconnectPeriod <= 0)
                    {
                        UpdateStatus(true, e.CurrentTime, "Communication Error ({0})", e.Status);
                        return;
                    }

                    UpdateStatus(true, e.CurrentTime, "Reconnecting in {0}s", m_reconnectPeriod);

                    if (m_reConnectHandler == null)
                    {
                        m_ReconnectStarting?.Invoke(this, e);

                        m_reConnectHandler = new SessionReconnectHandler();
                        m_reConnectHandler.BeginReconnect(m_session, m_reconnectPeriod * 1000, Server_ReconnectComplete);
                    }

                    return;
                }

                // update status.
                UpdateStatus(false, e.CurrentTime, "Connected [{0}]", session.Endpoint.EndpointUrl);

                // raise any additional notifications.
                m_KeepAliveComplete?.Invoke(this, e);
            }
            catch (Exception exception)
            {
                LoggerService.Service.Info("OpcClient", exception.Message);
            }
        }

        /// <summary>
        /// Handles a reconnect event complete from the reconnect handler.
        /// </summary>
        private void Server_ReconnectComplete(object sender, EventArgs e)
        {
            try
            {
                // ignore callbacks from discarded objects.
                if (!Object.ReferenceEquals(sender, m_reConnectHandler))
                {
                    return;
                }

                m_session = m_reConnectHandler.Session;
                m_reConnectHandler.Dispose();
                m_reConnectHandler = null;

                // raise any additional notifications.
                m_ReconnectComplete?.Invoke(this, e);
            }
            catch (Exception exception)
            {
                LoggerService.Service.Info("OpcClient",exception.Message);
            }
        }

        #endregion Event Handlers

        #region LogOut Setting

        /// <summary>
        /// 设置OPC客户端的日志输出
        /// </summary>
        /// <param name="filePath">完整的文件路径</param>
        /// <param name="deleteExisting">是否删除原文件</param>
        public void SetLogPathName(string filePath, bool deleteExisting)
        {
            Utils.SetTraceLog(filePath, deleteExisting);
            Utils.SetTraceMask(515);
        }

        #endregion LogOut Setting

        #region Public Members

        /// <summary>
        /// a name of application name show on server
        /// </summary>
        public string OpcUaName { get; set; } = "SpiderOpcUaClient";

        /// <summary>
        /// Whether to use security when connecting.
        /// </summary>
        public bool UseSecurity
        {
            get { return m_useSecurity; }
            set { m_useSecurity = value; }
        }

        /// <summary>
        /// The user identity to use when creating the session.
        /// </summary>
        public IUserIdentity UserIdentity { get; set; }

        /// <summary>
        /// The currently active session.
        /// </summary>
        public Session Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// Indicate the connect status
        /// </summary>
        public bool Connected
        {
            get { return m_IsConnected; }
        }

        /// <summary>
        /// The number of seconds between reconnect attempts (0 means reconnect is disabled).
        /// </summary>
        public int ReconnectPeriod
        {
            get { return m_reconnectPeriod; }
            set { m_reconnectPeriod = value; }
        }

        /// <summary>
        /// Raised when a good keep alive from the server arrives.
        /// </summary>
        public event EventHandler KeepAliveComplete
        {
            add { m_KeepAliveComplete += value; }
            remove { m_KeepAliveComplete -= value; }
        }

        /// <summary>
        /// Raised when a reconnect operation starts.
        /// </summary>
        public event EventHandler ReconnectStarting
        {
            add { m_ReconnectStarting += value; }
            remove { m_ReconnectStarting -= value; }
        }

        /// <summary>
        /// Raised when a reconnect operation completes.
        /// </summary>
        public event EventHandler ReconnectComplete
        {
            add { m_ReconnectComplete += value; }
            remove { m_ReconnectComplete -= value; }
        }

        /// <summary>
        /// Raised after successfully connecting to or disconnecing from a server.
        /// </summary>
        public event EventHandler ConnectComplete
        {
            add { m_ConnectComplete += value; }
            remove { m_ConnectComplete -= value; }
        }

        /// <summary>
        /// Raised after the client status change
        /// </summary>
        public event EventHandler<OpcUaStatusEventArgs> OpcStatusChange
        {
            add { m_OpcStatusChange += value; }
            remove { m_OpcStatusChange -= value; }
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public ApplicationConfiguration AppConfig => m_configuration;

        #endregion Public Members

        #region Node Write/Read Support

        /// <summary>
        /// Read a value node from server
        /// </summary>
        /// <param name="nodeId">node id</param>
        /// <returns>DataValue</returns>
        public DataValue ReadNode(NodeId nodeId)
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection
            {
                new ReadValueId( )
                {
                    NodeId = nodeId,
                    AttributeId = Attributes.Value
                }
            };

            // read the current value
            m_session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out DataValueCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results[0];
        }

        /// <summary>
        /// Read a value node from server
        /// </summary>
        /// <typeparam name="T">type of value</typeparam>
        /// <param name="tag">node id</param>
        /// <returns>实际值</returns>
        public T ReadNode<T>(string tag)
        {
            DataValue dataValue = ReadNode(new NodeId(tag));
            return (T)dataValue.Value;
        }

        /// <summary>
        /// Read a tag asynchronously
        /// </summary>
        /// <typeparam name="T">The type of tag to read</typeparam>
        /// <param name="tag">tag值</param>
        /// <returns>The value retrieved from the OPC</returns>
        public Task<T> ReadNodeAsync<T>(string tag)
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection
            {
                new ReadValueId()
                {
                    NodeId = new NodeId(tag),
                    AttributeId = Attributes.Value
                }
            };

            // Wrap the ReadAsync logic in a TaskCompletionSource, so we can use C# async/await syntax to call it:
            var taskCompletionSource = new TaskCompletionSource<T>();
            m_session.BeginRead(
                requestHeader: null,
                maxAge: 0,
                timestampsToReturn: TimestampsToReturn.Neither,
                nodesToRead: nodesToRead,
                callback: ar =>
                {
                    DataValueCollection results;
                    DiagnosticInfoCollection diag;
                    var response = m_session.EndRead(
                      result: ar,
                      results: out results,
                      diagnosticInfos: out diag);

                    try
                    {
                        CheckReturnValue(response.ServiceResult);
                        CheckReturnValue(results[0].StatusCode);
                        var val = results[0];
                        taskCompletionSource.TrySetResult((T)val.Value);
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                },
                asyncState: null);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// read several value nodes from server
        /// </summary>
        /// <param name="nodeIds">all NodeIds</param>
        /// <returns>all values</returns>
        public List<DataValue> ReadNodes(NodeId[] nodeIds)
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            for (int i = 0; i < nodeIds.Length; i++)
            {
                nodesToRead.Add(new ReadValueId()
                {
                    NodeId = nodeIds[i],
                    AttributeId = Attributes.Value
                });
            }

            // 读取当前的值
            m_session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out DataValueCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results.ToList();
        }

        /// <summary>
        /// read several value nodes from server
        /// </summary>
        /// <param name="nodeIds">all NodeIds</param>
        /// <returns>all values</returns>
        public Task<List<DataValue>> ReadNodesAsync(NodeId[] nodeIds)
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            for (int i = 0; i < nodeIds.Length; i++)
            {
                nodesToRead.Add(new ReadValueId()
                {
                    NodeId = nodeIds[i],
                    AttributeId = Attributes.Value
                });
            }

            var taskCompletionSource = new TaskCompletionSource<List<DataValue>>();
            // 读取当前的值
            m_session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                callback: ar =>
                {
                    DataValueCollection results;
                    DiagnosticInfoCollection diag;
                    var response = m_session.EndRead(
                      result: ar,
                      results: out results,
                      diagnosticInfos: out diag);

                    try
                    {
                        CheckReturnValue(response.ServiceResult);
                        taskCompletionSource.TrySetResult(results.ToList());
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                },
                asyncState: null);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// read several value nodes from server
        /// </summary>
        /// <param name="tags">所以的节点数组信息</param>
        /// <returns>all values</returns>
        public List<T> ReadNodes<T>(string[] tags)
        {
            List<T> result = new List<T>();
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            for (int i = 0; i < tags.Length; i++)
            {
                nodesToRead.Add(new ReadValueId()
                {
                    NodeId = new NodeId(tags[i]),
                    AttributeId = Attributes.Value
                });
            }

            // 读取当前的值
            m_session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out DataValueCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            foreach (var item in results)
            {
                result.Add((T)item.Value);
            }
            return result;
        }

        /// <summary>
        /// read several value nodes from server
        /// </summary>
        /// <param name="tags">all NodeIds</param>
        /// <returns>all values</returns>
        public Task<List<T>> ReadNodesAsync<T>(string[] tags)
        {
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
            for (int i = 0; i < tags.Length; i++)
            {
                nodesToRead.Add(new ReadValueId()
                {
                    NodeId = new NodeId(tags[i]),
                    AttributeId = Attributes.Value
                });
            }

            var taskCompletionSource = new TaskCompletionSource<List<T>>();
            // 读取当前的值
            m_session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                callback: ar =>
                {
                    DataValueCollection results;
                    DiagnosticInfoCollection diag;
                    var response = m_session.EndRead(
                      result: ar,
                      results: out results,
                      diagnosticInfos: out diag);

                    try
                    {
                        CheckReturnValue(response.ServiceResult);
                        List<T> result = new List<T>();
                        foreach (var item in results)
                        {
                            result.Add((T)item.Value);
                        }
                        taskCompletionSource.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                },
                asyncState: null);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// write a note to server(you should use try catch)
        /// </summary>
        /// <typeparam name="T">The type of tag to write on</typeparam>
        /// <param name="tag">节点名称</param>
        /// <param name="value">值</param>
        /// <returns>if success True,otherwise False</returns>
        public bool WriteNode<T>(string tag, T value)
        {
            WriteValue valueToWrite = new WriteValue()
            {
                NodeId = new NodeId(tag),
                AttributeId = Attributes.Value
            };
            valueToWrite.Value.Value = value;
            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;

            WriteValueCollection valuesToWrite = new WriteValueCollection
            {
                valueToWrite
            };

            // 写入当前的值

            m_session.Write(
                null,
                valuesToWrite,
                out StatusCodeCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, valuesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);

            if (StatusCode.IsBad(results[0]))
            {
                throw new ServiceResultException(results[0]);
            }

            return !StatusCode.IsBad(results[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteNode(string tag, object value)
        {
            WriteValue valueToWrite = new WriteValue()
            {
                NodeId = new NodeId(tag),
                AttributeId = Attributes.Value
            };
            valueToWrite.Value.Value = value;
            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;

            WriteValueCollection valuesToWrite = new WriteValueCollection
            {
                valueToWrite
            };

            // 写入当前的值

            m_session.Write(
                null,
                valuesToWrite,
                out StatusCodeCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, valuesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);

            if (StatusCode.IsBad(results[0]))
            {
                throw new ServiceResultException(results[0]);
            }

            return !StatusCode.IsBad(results[0]);
        }

        /// <summary>
        /// Write a value on the specified opc tag asynchronously
        /// </summary>
        /// <typeparam name="T">The type of tag to write on</typeparam>
        /// <param name="tag">The fully-qualified identifier of the tag. You can specify a subfolder by using a comma delimited name. E.g: the tag `foo.bar` writes on the tag `bar` on the folder `foo`</param>
        /// <param name="value">The value for the item to write</param>
        public Task<bool> WriteNodeAsync<T>(string tag, T value)
        {
            WriteValue valueToWrite = new WriteValue()
            {
                NodeId = new NodeId(tag),
                AttributeId = Attributes.Value,
            };
            valueToWrite.Value.Value = value;
            valueToWrite.Value.StatusCode = StatusCodes.Good;
            valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
            valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
            WriteValueCollection valuesToWrite = new WriteValueCollection
            {
                valueToWrite
            };

            // Wrap the WriteAsync logic in a TaskCompletionSource, so we can use C# async/await syntax to call it:
            var taskCompletionSource = new TaskCompletionSource<bool>();
            m_session.BeginWrite(
                requestHeader: null,
                nodesToWrite: valuesToWrite,
                callback: ar =>
                {
                    var response = m_session.EndWrite(
                      result: ar,
                      results: out StatusCodeCollection results,
                      diagnosticInfos: out DiagnosticInfoCollection diag);

                    try
                    {
                        ClientBase.ValidateResponse(results, valuesToWrite);
                        ClientBase.ValidateDiagnosticInfos(diag, valuesToWrite);
                        taskCompletionSource.SetResult(StatusCode.IsGood(results[0]));
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                },
                asyncState: null);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 所有的节点都写入成功，返回<c>True</c>，否则返回<c>False</c>
        /// </summary>
        /// <param name="tags">节点名称数组</param>
        /// <param name="values">节点的值数据</param>
        /// <returns>所有的是否都写入成功</returns>
        public bool WriteNodes(string[] tags, object[] values)
        {
            WriteValueCollection valuesToWrite = new WriteValueCollection();

            for (int i = 0; i < tags.Length; i++)
            {
                if (i < values.Length)
                {
                    WriteValue valueToWrite = new WriteValue()
                    {
                        NodeId = new NodeId(tags[i]),
                        AttributeId = Attributes.Value
                    };
                    valueToWrite.Value.Value = values[i];
                    valueToWrite.Value.StatusCode = StatusCodes.Good;
                    valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
                    valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
                    valuesToWrite.Add(valueToWrite);
                }
            }

            // 写入当前的值

            m_session.Write(
                null,
                valuesToWrite,
                out StatusCodeCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, valuesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);

            bool result = true;
            foreach (var r in results)
            {
                if (StatusCode.IsBad(r))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        #endregion Node Write/Read Support

        #region DeleteNode Support

        /// <summary>
        /// 删除一个节点的操作，除非服务器配置允许，否则引发异常，成功返回<c>True</c>，否则返回<c>False</c>
        /// </summary>
        /// <param name="tag">节点文本描述</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteExsistNode(string tag)
        {
            DeleteNodesItemCollection waitDelete = new DeleteNodesItemCollection();

            DeleteNodesItem nodesItem = new DeleteNodesItem()
            {
                NodeId = new NodeId(tag),
            };

            m_session.DeleteNodes(
                null,
                waitDelete,
                out StatusCodeCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, waitDelete);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, waitDelete);

            return !StatusCode.IsBad(results[0]);
        }

        #endregion DeleteNode Support

        #region Test Function

        /// <summary>
        /// 新增一个节点数据
        /// </summary>
        /// <param name="parent">父节点tag名称</param>
        [Obsolete("还未经过测试，无法使用")]
        public void AddNewNode(NodeId parent)
        {
            // Create a Variable node.
            AddNodesItem node2 = new AddNodesItem();
            node2.ParentNodeId = new NodeId(parent);
            node2.ReferenceTypeId = ReferenceTypes.HasComponent;
            node2.RequestedNewNodeId = null;
            node2.BrowseName = new QualifiedName("DataVariable1");
            node2.NodeClass = NodeClass.Variable;
            node2.NodeAttributes = null;
            node2.TypeDefinition = VariableTypeIds.BaseDataVariableType;

            //specify node attributes.
            VariableAttributes node2Attribtues = new VariableAttributes();
            node2Attribtues.DisplayName = "DataVariable1";
            node2Attribtues.Description = "DataVariable1 Description";
            node2Attribtues.Value = new Variant(123);
            node2Attribtues.DataType = (uint)BuiltInType.Int32;
            node2Attribtues.ValueRank = ValueRanks.Scalar;
            node2Attribtues.ArrayDimensions = new UInt32Collection();
            node2Attribtues.AccessLevel = AccessLevels.CurrentReadOrWrite;
            node2Attribtues.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            node2Attribtues.MinimumSamplingInterval = 0;
            node2Attribtues.Historizing = false;
            node2Attribtues.WriteMask = (uint)AttributeWriteMask.None;
            node2Attribtues.UserWriteMask = (uint)AttributeWriteMask.None;
            node2Attribtues.SpecifiedAttributes = (uint)NodeAttributesMask.All;

            node2.NodeAttributes = new ExtensionObject(node2Attribtues);

            AddNodesItemCollection nodesToAdd = new AddNodesItemCollection { node2 };

            m_session.AddNodes(
                null,
                nodesToAdd,
                out AddNodesResultCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToAdd);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToAdd);
        }

        #endregion Test Function

        #region Monitor Support

        /// <summary>
        /// 新增一个订阅，需要指定订阅的关键字，订阅的tag名，以及回调方法
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="tag">tag</param>
        /// <param name="callback">回调方法</param>
        public void AddSubscription(string key, string tag, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
        {
            AddSubscription(key, new string[] { tag }, callback);
        }

        /// <summary>
        /// 新增一批订阅，需要指定订阅的关键字，订阅的tag名数组，以及回调方法
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="tags">节点名称数组</param>
        /// <param name="callback">回调方法</param>
        public void AddSubscription(string key, string[] tags, Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> callback)
        {
            Subscription m_subscription = new Subscription(m_session.DefaultSubscription);

            m_subscription.PublishingEnabled = true;
            m_subscription.PublishingInterval = 0;
            m_subscription.KeepAliveCount = uint.MaxValue;
            m_subscription.LifetimeCount = uint.MaxValue;
            m_subscription.MaxNotificationsPerPublish = uint.MaxValue;
            m_subscription.Priority = 100;
            m_subscription.DisplayName = key;

            for (int i = 0; i < tags.Length; i++)
            {
                var item = new MonitoredItem
                {
                    StartNodeId = new NodeId(tags[i]),
                    AttributeId = Attributes.Value,
                    DisplayName = tags[i],
                    SamplingInterval = 100,
                };
                item.Notification += (MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs args) =>
                {
                    callback?.Invoke(key, monitoredItem, args);
                };
                m_subscription.AddItem(item);
            }

            m_session.AddSubscription(m_subscription);
            m_subscription.Create();

            lock (dic_subscriptions)
            {
                if (dic_subscriptions.ContainsKey(key))
                {
                    // remove
                    dic_subscriptions[key].Delete(true);
                    m_session.RemoveSubscription(dic_subscriptions[key]);
                    dic_subscriptions[key].Dispose();
                    dic_subscriptions[key] = m_subscription;
                }
                else
                {
                    dic_subscriptions.Add(key, m_subscription);
                }
            }
        }

        /// <summary>
        /// 移除订阅消息，如果该订阅消息是批量的，也直接移除
        /// </summary>
        /// <param name="key">订阅关键值</param>
        public void RemoveSubscription(string key)
        {
            lock (dic_subscriptions)
            {
                if (dic_subscriptions.ContainsKey(key))
                {
                    // remove
                    dic_subscriptions[key].Delete(true);
                    m_session.RemoveSubscription(dic_subscriptions[key]);
                    dic_subscriptions[key].Dispose();
                    dic_subscriptions.Remove(key);
                }
            }
        }

        /// <summary>
        /// 移除所有的订阅消息
        /// </summary>
        public void RemoveAllSubscription()
        {
            lock (dic_subscriptions)
            {
                foreach (var item in dic_subscriptions)
                {
                    item.Value.Delete(true);
                    m_session.RemoveSubscription(item.Value);
                    item.Value.Dispose();
                }
                dic_subscriptions.Clear();
            }
        }

        #endregion Monitor Support

        #region ReadHistory Support

        /// <summary>
        /// read History data
        /// </summary>
        /// <param name="tag">节点的索引</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="count">读取的个数</param>
        /// <param name="containBound">是否包含边界</param>
        /// <returns>读取的数据列表</returns>
        public IEnumerable<DataValue> ReadHistoryRawDataValues(string tag, DateTime start, DateTime end, uint count = 1, bool containBound = false)
        {
            HistoryReadValueId m_nodeToContinue = new HistoryReadValueId()
            {
                NodeId = new NodeId(tag),
            };

            ReadRawModifiedDetails m_details = new ReadRawModifiedDetails
            {
                StartTime = start,
                EndTime = end,
                NumValuesPerNode = count,
                IsReadModified = false,
                ReturnBounds = containBound
            };

            HistoryReadValueIdCollection nodesToRead = new HistoryReadValueIdCollection();
            nodesToRead.Add(m_nodeToContinue);

            m_session.HistoryRead(
                null,
                new ExtensionObject(m_details),
                TimestampsToReturn.Both,
                false,
                nodesToRead,
                out HistoryReadResultCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            if (StatusCode.IsBad(results[0].StatusCode))
            {
                throw new ServiceResultException(results[0].StatusCode);
            }

            HistoryData values = ExtensionObject.ToEncodeable(results[0].HistoryData) as HistoryData;
            foreach (var value in values.DataValues)
            {
                yield return value;
            }
        }

        /// <summary>
        /// 读取一连串的历史数据，并将其转化成指定的类型
        /// </summary>
        /// <param name="tag">节点的索引</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="count">读取的个数</param>
        /// <param name="containBound">是否包含边界</param>
        /// <returns>读取的数据列表</returns>
        public IEnumerable<T> ReadHistoryRawDataValues<T>(string tag, DateTime start, DateTime end, uint count = 1, bool containBound = false)
        {
            HistoryReadValueId m_nodeToContinue = new HistoryReadValueId()
            {
                NodeId = new NodeId(tag),
            };

            ReadRawModifiedDetails m_details = new ReadRawModifiedDetails
            {
                StartTime = start.ToUniversalTime(),
                EndTime = end.ToUniversalTime(),
                NumValuesPerNode = count,
                IsReadModified = false,
                ReturnBounds = containBound
            };

            HistoryReadValueIdCollection nodesToRead = new HistoryReadValueIdCollection();
            nodesToRead.Add(m_nodeToContinue);

            m_session.HistoryRead(
                null,
                new ExtensionObject(m_details),
                TimestampsToReturn.Both,
                false,
                nodesToRead,
                out HistoryReadResultCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            if (StatusCode.IsBad(results[0].StatusCode))
            {
                throw new ServiceResultException(results[0].StatusCode);
            }

            HistoryData values = ExtensionObject.ToEncodeable(results[0].HistoryData) as HistoryData;
            foreach (var value in values.DataValues)
            {
                yield return (T)value.Value;
            }
        }

        #endregion ReadHistory Support

        #region BrowseNode Support

        /// <summary>
        /// 浏览一个节点的引用
        /// </summary>
        /// <param name="tag">节点值</param>
        /// <returns>引用节点描述</returns>
        public ReferenceDescription[] BrowseNodeReference(string tag)
        {
            NodeId sourceId = new NodeId(tag);

            // 该节点可以读取到方法
            BrowseDescription nodeToBrowse1 = new BrowseDescription();

            nodeToBrowse1.NodeId = sourceId;
            nodeToBrowse1.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.Aggregates;
            nodeToBrowse1.IncludeSubtypes = true;
            nodeToBrowse1.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.Method);
            nodeToBrowse1.ResultMask = (uint)BrowseResultMask.All;

            // 该节点无论怎么样都读取不到方法
            // find all nodes organized by the node.
            BrowseDescription nodeToBrowse2 = new BrowseDescription();

            nodeToBrowse2.NodeId = sourceId;
            nodeToBrowse2.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse2.ReferenceTypeId = ReferenceTypeIds.Organizes;
            nodeToBrowse2.IncludeSubtypes = true;
            nodeToBrowse2.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable);
            nodeToBrowse2.ResultMask = (uint)BrowseResultMask.All;

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
            nodesToBrowse.Add(nodeToBrowse1);
            nodesToBrowse.Add(nodeToBrowse2);

            // fetch references from the server.
            ReferenceDescriptionCollection references = FormUtils.Browse(m_session, nodesToBrowse, false);

            return references.ToArray();
        }

        #endregion BrowseNode Support

        #region Read Attributes Support

        /// <summary>
        /// 读取一个节点的所有属性
        /// </summary>
        /// <param name="tag">节点信息</param>
        /// <returns>节点的特性值</returns>
        public OpcNodeAttribute[] ReadNoteAttributes(string tag)
        {
            NodeId sourceId = new NodeId(tag);
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

            // attempt to read all possible attributes.
            // 尝试着去读取所有可能的特性
            for (uint ii = Attributes.NodeClass; ii <= Attributes.UserExecutable; ii++)
            {
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = sourceId;
                nodeToRead.AttributeId = ii;
                nodesToRead.Add(nodeToRead);
            }

            int startOfProperties = nodesToRead.Count;

            // find all of the pror of the node.
            BrowseDescription nodeToBrowse1 = new BrowseDescription();

            nodeToBrowse1.NodeId = sourceId;
            nodeToBrowse1.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.HasProperty;
            nodeToBrowse1.IncludeSubtypes = true;
            nodeToBrowse1.NodeClassMask = 0;
            nodeToBrowse1.ResultMask = (uint)BrowseResultMask.All;

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
            nodesToBrowse.Add(nodeToBrowse1);

            // fetch property references from the server.
            ReferenceDescriptionCollection references = FormUtils.Browse(m_session, nodesToBrowse, false);

            if (references == null)
            {
                return new OpcNodeAttribute[0];
            }

            for (int ii = 0; ii < references.Count; ii++)
            {
                // ignore external references.
                if (references[ii].NodeId.IsAbsolute)
                {
                    continue;
                }

                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = (NodeId)references[ii].NodeId;
                nodeToRead.AttributeId = Attributes.Value;
                nodesToRead.Add(nodeToRead);
            }

            // read all values.
            DataValueCollection results = null;
            DiagnosticInfoCollection diagnosticInfos = null;

            m_session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out results,
                out diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            // process results.

            List<OpcNodeAttribute> nodeAttribute = new List<OpcNodeAttribute>();
            for (int ii = 0; ii < results.Count; ii++)
            {
                OpcNodeAttribute item = new OpcNodeAttribute();

                // process attribute value.
                if (ii < startOfProperties)
                {
                    // ignore attributes which are invalid for the node.
                    if (results[ii].StatusCode == StatusCodes.BadAttributeIdInvalid)
                    {
                        continue;
                    }

                    // get the name of the attribute.
                    item.Name = Attributes.GetBrowseName(nodesToRead[ii].AttributeId);

                    // display any unexpected error.
                    if (StatusCode.IsBad(results[ii].StatusCode))
                    {
                        item.Type = Utils.Format("{0}", Attributes.GetDataTypeId(nodesToRead[ii].AttributeId));
                        item.Value = Utils.Format("{0}", results[ii].StatusCode);
                    }

                    // display the value.
                    else
                    {
                        TypeInfo typeInfo = TypeInfo.Construct(results[ii].Value);

                        item.Type = typeInfo.BuiltInType.ToString();

                        if (typeInfo.ValueRank >= ValueRanks.OneOrMoreDimensions)
                        {
                            item.Type += "[]";
                        }

                        item.Value = results[ii].Value;//Utils.Format("{0}", results[ii].Value);
                    }
                }

                // process property value.
                else
                {
                    // ignore properties which are invalid for the node.
                    if (results[ii].StatusCode == StatusCodes.BadNodeIdUnknown)
                    {
                        continue;
                    }

                    // get the name of the property.
                    item.Name = Utils.Format("{0}", references[ii - startOfProperties]);

                    // display any unexpected error.
                    if (StatusCode.IsBad(results[ii].StatusCode))
                    {
                        item.Type = String.Empty;
                        item.Value = Utils.Format("{0}", results[ii].StatusCode);
                    }

                    // display the value.
                    else
                    {
                        TypeInfo typeInfo = TypeInfo.Construct(results[ii].Value);

                        item.Type = typeInfo.BuiltInType.ToString();

                        if (typeInfo.ValueRank >= ValueRanks.OneOrMoreDimensions)
                        {
                            item.Type += "[]";
                        }

                        item.Value = results[ii].Value; //Utils.Format("{0}", results[ii].Value);
                    }
                }

                nodeAttribute.Add(item);
            }

            return nodeAttribute.ToArray();
        }

        /// <summary>
        /// 读取一个节点的所有属性
        /// </summary>
        /// <param name="tag">节点值</param>
        /// <returns>所有的数据</returns>
        public DataValue[] ReadNoteDataValueAttributes(string tag)
        {
            NodeId sourceId = new NodeId(tag);
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

            // attempt to read all possible attributes.
            // 尝试着去读取所有可能的特性
            for (uint ii = Attributes.NodeId; ii <= Attributes.UserExecutable; ii++)
            {
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = sourceId;
                nodeToRead.AttributeId = ii;
                nodesToRead.Add(nodeToRead);
            }

            int startOfProperties = nodesToRead.Count;

            // find all of the pror of the node.
            BrowseDescription nodeToBrowse1 = new BrowseDescription();

            nodeToBrowse1.NodeId = sourceId;
            nodeToBrowse1.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.HasProperty;
            nodeToBrowse1.IncludeSubtypes = true;
            nodeToBrowse1.NodeClassMask = 0;
            nodeToBrowse1.ResultMask = (uint)BrowseResultMask.All;

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
            nodesToBrowse.Add(nodeToBrowse1);

            // fetch property references from the server.
            ReferenceDescriptionCollection references = FormUtils.Browse(m_session, nodesToBrowse, false);

            if (references == null)
            {
                return new DataValue[0];
            }

            for (int ii = 0; ii < references.Count; ii++)
            {
                // ignore external references.
                if (references[ii].NodeId.IsAbsolute)
                {
                    continue;
                }

                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = (NodeId)references[ii].NodeId;
                nodeToRead.AttributeId = Attributes.Value;
                nodesToRead.Add(nodeToRead);
            }

            // read all values.
            DataValueCollection results = null;
            DiagnosticInfoCollection diagnosticInfos = null;

            m_session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out results,
                out diagnosticInfos);

            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results.ToArray();
        }

        #endregion Read Attributes Support

        #region Method Call Support

        /// <summary>
        /// call a server method
        /// </summary>
        /// <param name="tagParent">方法的父节点tag</param>
        /// <param name="tag">方法的节点tag</param>
        /// <param name="args">传递的参数</param>
        /// <returns>输出的结果值</returns>
        public object[] CallMethodByNodeId(string tagParent, string tag, params object[] args)
        {
            if (m_session == null)
            {
                return null;
            }

            IList<object> outputArguments = m_session.Call(
                new NodeId(tagParent),
                new NodeId(tag),
                args);

            return outputArguments.ToArray();
        }

        #endregion Method Call Support

        #region Private Methods

        /// <summary>
        /// Raises the connect complete event on the main GUI thread.
        /// </summary>
        private void DoConnectComplete(object state)
        {
            m_ConnectComplete?.Invoke(this, null);
        }

        private void CheckReturnValue(StatusCode status)
        {
            if (!StatusCode.IsGood(status))
                throw new Exception(string.Format("Invalid response from the server. (Response Status: {0})", status));
        }

        #endregion Private Methods

        #region Private Fields

        private ApplicationConfiguration m_configuration;
        private Session m_session;
        private bool m_IsConnected;                       //是否已经连接过
        private int m_reconnectPeriod = 10;               // 重连状态
        private bool m_useSecurity;

        private SessionReconnectHandler m_reConnectHandler;
        private EventHandler m_ReconnectComplete;
        private EventHandler m_ReconnectStarting;
        private EventHandler m_KeepAliveComplete;
        private EventHandler m_ConnectComplete;
        private EventHandler<OpcUaStatusEventArgs> m_OpcStatusChange;

        private Dictionary<string, Subscription> dic_subscriptions;        // 系统所有的节点信息

        #endregion Private Fields
    }


    /// <summary>
    /// OPC UA的状态更新消息
    /// </summary>
    public class OpcUaStatusEventArgs
    {


        /// <summary>
        /// 是否异常
        /// </summary>
        public bool Error { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Error ? "[异常]" : "[正常]" + Time.ToString("  yyyy-MM-dd HH:mm:ss  ") + Text;
        }


    }

    /// <summary>
    /// 读取属性过程中用于描述的
    /// </summary>
    public class OpcNodeAttribute
    {
        /// <summary>
        /// 属性的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性的类型描述
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 操作结果状态描述
        /// </summary>
        public StatusCode StatusCode { get; set; }
        /// <summary>
        /// 属性的值，如果读取错误，返回文本描述
        /// </summary>
        public object Value { get; set; }

    }


    /// <summary>
    /// 辅助类
    /// </summary>
    public class FormUtils
    {
        /// <summary>
        /// Gets the display text for the access level attribute.
        /// </summary>
        /// <param name="accessLevel">The access level.</param>
        /// <returns>The access level formatted as a string.</returns>
        private static string GetAccessLevelDisplayText(byte accessLevel)
        {
            StringBuilder buffer = new StringBuilder();

            if (accessLevel == AccessLevels.None)
            {
                buffer.Append("None");
            }

            if ((accessLevel & AccessLevels.CurrentRead) == AccessLevels.CurrentRead)
            {
                buffer.Append("Read");
            }

            if ((accessLevel & AccessLevels.CurrentWrite) == AccessLevels.CurrentWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("Write");
            }

            if ((accessLevel & AccessLevels.HistoryRead) == AccessLevels.HistoryRead)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryRead");
            }

            if ((accessLevel & AccessLevels.HistoryWrite) == AccessLevels.HistoryWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryWrite");
            }

            if ((accessLevel & AccessLevels.SemanticChange) == AccessLevels.SemanticChange)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("SemanticChange");
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Gets the display text for the event notifier attribute.
        /// </summary>
        /// <param name="eventNotifier">The event notifier.</param>
        /// <returns>The event notifier formatted as a string.</returns>
        private static string GetEventNotifierDisplayText(byte eventNotifier)
        {
            StringBuilder buffer = new StringBuilder();

            if (eventNotifier == EventNotifiers.None)
            {
                buffer.Append("None");
            }

            if ((eventNotifier & EventNotifiers.SubscribeToEvents) == EventNotifiers.SubscribeToEvents)
            {
                buffer.Append("Subscribe");
            }

            if ((eventNotifier & EventNotifiers.HistoryRead) == EventNotifiers.HistoryRead)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryRead");
            }

            if ((eventNotifier & EventNotifiers.HistoryWrite) == EventNotifiers.HistoryWrite)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append(" | ");
                }

                buffer.Append("HistoryWrite");
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Gets the display text for the value rank attribute.
        /// </summary>
        /// <param name="valueRank">The value rank.</param>
        /// <returns>The value rank formatted as a string.</returns>
        private static string GetValueRankDisplayText(int valueRank)
        {
            switch (valueRank)
            {
                case ValueRanks.Any: return "Any";
                case ValueRanks.Scalar: return "Scalar";
                case ValueRanks.ScalarOrOneDimension: return "ScalarOrOneDimension";
                case ValueRanks.OneOrMoreDimensions: return "OneOrMoreDimensions";
                case ValueRanks.OneDimension: return "OneDimension";
                case ValueRanks.TwoDimensions: return "TwoDimensions";
            }

            return valueRank.ToString();
        }

        /// <summary>
        /// Gets the display text for the specified attribute.
        /// </summary>
        /// <param name="session">The currently active session.</param>
        /// <param name="attributeId">The id of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>The attribute formatted as a string.</returns>
        public static string GetAttributeDisplayText(Session session, uint attributeId, Variant value)
        {
            if (value == Variant.Null)
            {
                return String.Empty;
            }

            switch (attributeId)
            {
                case Attributes.AccessLevel:
                case Attributes.UserAccessLevel:
                    {
                        byte? field = value.Value as byte?;

                        if (field != null)
                        {
                            return GetAccessLevelDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.EventNotifier:
                    {
                        byte? field = value.Value as byte?;

                        if (field != null)
                        {
                            return GetEventNotifierDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.DataType:
                    {
                        return session.NodeCache.GetDisplayText(value.Value as NodeId);
                    }

                case Attributes.ValueRank:
                    {
                        int? field = value.Value as int?;

                        if (field != null)
                        {
                            return GetValueRankDisplayText(field.Value);
                        }

                        break;
                    }

                case Attributes.NodeClass:
                    {
                        int? field = value.Value as int?;

                        if (field != null)
                        {
                            return ((NodeClass)field.Value).ToString();
                        }

                        break;
                    }

                case Attributes.NodeId:
                    {
                        NodeId field = value.Value as NodeId;

                        if (!NodeId.IsNull(field))
                        {
                            return field.ToString();
                        }

                        return "Null";
                    }
            }

            // check for byte strings.
            if (value.Value is byte[])
            {
                return Utils.ToHexString(value.Value as byte[]);
            }

            // use default format.
            return value.ToString();
        }

        /// <summary>
        /// Discovers the servers on the local machine.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of server urls.</returns>
        public static IList<string> DiscoverServers(ApplicationConfiguration configuration)
        {
            List<string> serverUrls = new List<string>();

            // set a short timeout because this is happening in the drop down event.
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(configuration);
            endpointConfiguration.OperationTimeout = 5000;

            // Connect to the local discovery server and find the available servers.
            using (DiscoveryClient client = DiscoveryClient.Create(new Uri("opc.tcp://localhost:4840"), endpointConfiguration))
            {
                ApplicationDescriptionCollection servers = client.FindServers(null);

                // populate the drop down list with the discovery URLs for the available servers.
                for (int ii = 0; ii < servers.Count; ii++)
                {
                    if (servers[ii].ApplicationType == ApplicationType.DiscoveryServer)
                    {
                        continue;
                    }

                    for (int jj = 0; jj < servers[ii].DiscoveryUrls.Count; jj++)
                    {
                        string discoveryUrl = servers[ii].DiscoveryUrls[jj];

                        // Many servers will use the '/discovery' suffix for the discovery endpoint.
                        // The URL without this prefix should be the base URL for the server. 
                        if (discoveryUrl.EndsWith("/discovery"))
                        {
                            discoveryUrl = discoveryUrl.Substring(0, discoveryUrl.Length - "/discovery".Length);
                        }

                        // ensure duplicates do not get added.
                        if (!serverUrls.Contains(discoveryUrl))
                        {
                            serverUrls.Add(discoveryUrl);
                        }
                    }
                }
            }

            return serverUrls;
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        /// <param name="discoveryUrl">The discovery URL.</param>
        /// <param name="useSecurity">if set to <c>true</c> select an endpoint that uses security.</param>
        /// <returns>The best available endpoint.</returns>
        public static EndpointDescription SelectEndpoint(string discoveryUrl, bool useSecurity)
        {
            // needs to add the '/discovery' back onto non-UA TCP URLs.
            if (!discoveryUrl.StartsWith(Utils.UriSchemeOpcTcp))
            {
                if (!discoveryUrl.EndsWith("/discovery"))
                {
                    discoveryUrl += "/discovery";
                }
            }

            // parse the selected URL.
            Uri uri = new Uri(discoveryUrl);

            // set a short timeout because this is happening in the drop down event.
            EndpointConfiguration configuration = EndpointConfiguration.Create();
            configuration.OperationTimeout = 5000;

            EndpointDescription selectedEndpoint = null;

            // Connect to the server's discovery endpoint and find the available configuration.
            using (DiscoveryClient client = DiscoveryClient.Create(uri, configuration))
            {
                EndpointDescriptionCollection endpoints = client.GetEndpoints(null);

                // select the best endpoint to use based on the selected URL and the UseSecurity checkbox. 
                for (int ii = 0; ii < endpoints.Count; ii++)
                {
                    EndpointDescription endpoint = endpoints[ii];

                    // check for a match on the URL scheme.
                    if (endpoint.EndpointUrl.StartsWith(uri.Scheme))
                    {
                        // check if security was requested.
                        if (useSecurity)
                        {
                            if (endpoint.SecurityMode == MessageSecurityMode.None)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (endpoint.SecurityMode != MessageSecurityMode.None)
                            {
                                continue;
                            }
                        }

                        // pick the first available endpoint by default.
                        if (selectedEndpoint == null)
                        {
                            selectedEndpoint = endpoint;
                        }

                        // The security level is a relative measure assigned by the server to the 
                        // endpoints that it returns. Clients should always pick the highest level
                        // unless they have a reason not too.
                        if (endpoint.SecurityLevel > selectedEndpoint.SecurityLevel)
                        {
                            selectedEndpoint = endpoint;
                        }
                    }
                }

                // pick the first available endpoint by default.
                if (selectedEndpoint == null && endpoints.Count > 0)
                {
                    selectedEndpoint = endpoints[0];
                }
            }

            // if a server is behind a firewall it may return URLs that are not accessible to the client.
            // This problem can be avoided by assuming that the domain in the URL used to call 
            // GetEndpoints can be used to access any of the endpoints. This code makes that conversion.
            // Note that the conversion only makes sense if discovery uses the same protocol as the endpoint.

            Uri endpointUrl = Utils.ParseUri(selectedEndpoint.EndpointUrl);

            if (endpointUrl != null && endpointUrl.Scheme == uri.Scheme)
            {
                UriBuilder builder = new UriBuilder(endpointUrl);
                builder.Host = uri.DnsSafeHost;
                builder.Port = uri.Port;
                selectedEndpoint.EndpointUrl = builder.ToString();
            }

            // return the selected endpoint.
            return selectedEndpoint;
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="nodesToBrowse">The set of browse operations to perform.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection Browse(Session session, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();
                BrowseDescriptionCollection unprocessedOperations = new BrowseDescriptionCollection();

                while (nodesToBrowse.Count > 0)
                {
                    // start the browse operation.
                    BrowseResultCollection results = null;
                    DiagnosticInfoCollection diagnosticInfos = null;

                    session.Browse(
                        null,
                        null,
                        0,
                        nodesToBrowse,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, nodesToBrowse);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                    ByteStringCollection continuationPoints = new ByteStringCollection();

                    for (int ii = 0; ii < nodesToBrowse.Count; ii++)
                    {
                        // check for error.
                        if (StatusCode.IsBad(results[ii].StatusCode))
                        {
                            // this error indicates that the server does not have enough simultaneously active 
                            // continuation points. This request will need to be resent after the other operations
                            // have been completed and their continuation points released.
                            if (results[ii].StatusCode == StatusCodes.BadNoContinuationPoints)
                            {
                                unprocessedOperations.Add(nodesToBrowse[ii]);
                            }

                            continue;
                        }

                        // check if all references have been fetched.
                        if (results[ii].References.Count == 0)
                        {
                            continue;
                        }

                        // save results.
                        references.AddRange(results[ii].References);

                        // check for continuation point.
                        if (results[ii].ContinuationPoint != null)
                        {
                            continuationPoints.Add(results[ii].ContinuationPoint);
                        }
                    }

                    // process continuation points.
                    ByteStringCollection revisedContiuationPoints = new ByteStringCollection();

                    while (continuationPoints.Count > 0)
                    {
                        // continue browse operation.
                        session.BrowseNext(
                            null,
                            true,
                            continuationPoints,
                            out results,
                            out diagnosticInfos);

                        ClientBase.ValidateResponse(results, continuationPoints);
                        ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);

                        for (int ii = 0; ii < continuationPoints.Count; ii++)
                        {
                            // check for error.
                            if (StatusCode.IsBad(results[ii].StatusCode))
                            {
                                continue;
                            }

                            // check if all references have been fetched.
                            if (results[ii].References.Count == 0)
                            {
                                continue;
                            }

                            // save results.
                            references.AddRange(results[ii].References);

                            // check for continuation point.
                            if (results[ii].ContinuationPoint != null)
                            {
                                revisedContiuationPoints.Add(results[ii].ContinuationPoint);
                            }
                        }

                        // check if browsing must continue;
                        revisedContiuationPoints = continuationPoints;
                    }

                    // check if unprocessed results exist.
                    nodesToBrowse = unprocessedOperations;
                }

                // return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Finds the type of the event for the notification.
        /// </summary>
        /// <param name="monitoredItem">The monitored item.</param>
        /// <param name="notification">The notification.</param>
        /// <returns>The NodeId of the EventType.</returns>
        public static NodeId FindEventType(MonitoredItem monitoredItem, EventFieldList notification)
        {
            EventFilter filter = monitoredItem.Status.Filter as EventFilter;

            if (filter != null)
            {
                for (int ii = 0; ii < filter.SelectClauses.Count; ii++)
                {
                    SimpleAttributeOperand clause = filter.SelectClauses[ii];

                    if (clause.BrowsePath.Count == 1 && clause.BrowsePath[0] == BrowseNames.EventType)
                    {
                        return notification.EventFields[ii].Value as NodeId;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="nodeToBrowse">The NodeId for the starting node.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection Browse(Session session, BrowseDescription nodeToBrowse, bool throwOnError)
        {
            try
            {
                ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

                // construct browse request.
                BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection();
                nodesToBrowse.Add(nodeToBrowse);

                // start the browse operation.
                BrowseResultCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                session.Browse(
                    null,
                    null,
                    0,
                    nodesToBrowse,
                    out results,
                    out diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToBrowse);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                do
                {
                    // check for error.
                    if (StatusCode.IsBad(results[0].StatusCode))
                    {
                        throw new ServiceResultException(results[0].StatusCode);
                    }

                    // process results.
                    for (int ii = 0; ii < results[0].References.Count; ii++)
                    {
                        references.Add(results[0].References[ii]);
                    }

                    // check if all references have been fetched.
                    if (results[0].References.Count == 0 || results[0].ContinuationPoint == null)
                    {
                        break;
                    }

                    // continue browse operation.
                    ByteStringCollection continuationPoints = new ByteStringCollection();
                    continuationPoints.Add(results[0].ContinuationPoint);

                    session.BrowseNext(
                        null,
                        false,
                        continuationPoints,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, continuationPoints);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);
                }
                while (true);

                //return complete list.
                return references;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Browses the address space and returns all of the supertypes of the specified type node.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="typeId">The NodeId for a type node in the address space.</param>
        /// <param name="throwOnError">if set to <c>true</c> a exception will be thrown on an error.</param>
        /// <returns>
        /// The references found. Null if an error occurred.
        /// </returns>
        public static ReferenceDescriptionCollection BrowseSuperTypes(Session session, NodeId typeId, bool throwOnError)
        {
            ReferenceDescriptionCollection supertypes = new ReferenceDescriptionCollection();

            try
            {
                // find all of the children of the field.
                BrowseDescription nodeToBrowse = new BrowseDescription();

                nodeToBrowse.NodeId = typeId;
                nodeToBrowse.BrowseDirection = BrowseDirection.Inverse;
                nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.HasSubtype;
                nodeToBrowse.IncludeSubtypes = false; // more efficient to use IncludeSubtypes=False when possible.
                nodeToBrowse.NodeClassMask = 0; // the HasSubtype reference already restricts the targets to Types. 
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

                ReferenceDescriptionCollection references = Browse(session, nodeToBrowse, throwOnError);

                while (references != null && references.Count > 0)
                {
                    // should never be more than one supertype.
                    supertypes.Add(references[0]);

                    // only follow references within this server.
                    if (references[0].NodeId.IsAbsolute)
                    {
                        break;
                    }

                    // get the references for the next level up.
                    nodeToBrowse.NodeId = (NodeId)references[0].NodeId;
                    references = Browse(session, nodeToBrowse, throwOnError);
                }

                // return complete list.
                return supertypes;
            }
            catch (Exception exception)
            {
                if (throwOnError)
                {
                    throw new ServiceResultException(exception, StatusCodes.BadUnexpectedError);
                }

                return null;
            }
        }

        /// <summary>
        /// Constructs an event object from a notification.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="monitoredItem">The monitored item that produced the notification.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="knownEventTypes">The known event types.</param>
        /// <param name="eventTypeMappings">Mapping between event types and known event types.</param>
        /// <returns>
        /// The event object. Null if the notification is not a valid event type.
        /// </returns>
        public static BaseEventState ConstructEvent(
            Session session,
            MonitoredItem monitoredItem,
            EventFieldList notification,
            Dictionary<NodeId, Type> knownEventTypes,
            Dictionary<NodeId, NodeId> eventTypeMappings)
        {
            // find the event type.
            NodeId eventTypeId = FindEventType(monitoredItem, notification);

            if (eventTypeId == null)
            {
                return null;
            }

            // look up the known event type.
            Type knownType = null;
            NodeId knownTypeId = null;

            if (eventTypeMappings.TryGetValue(eventTypeId, out knownTypeId))
            {
                knownType = knownEventTypes[knownTypeId];
            }

            // try again.
            if (knownType == null)
            {
                if (knownEventTypes.TryGetValue(eventTypeId, out knownType))
                {
                    knownTypeId = eventTypeId;
                    eventTypeMappings.Add(eventTypeId, eventTypeId);
                }
            }

            // try mapping it to a known type.
            if (knownType == null)
            {
                // browse for the supertypes of the event type.
                ReferenceDescriptionCollection supertypes = FormUtils.BrowseSuperTypes(session, eventTypeId, false);

                // can't do anything with unknown types.
                if (supertypes == null)
                {
                    return null;
                }

                // find the first supertype that matches a known event type.
                for (int ii = 0; ii < supertypes.Count; ii++)
                {
                    NodeId superTypeId = (NodeId)supertypes[ii].NodeId;

                    if (knownEventTypes.TryGetValue(superTypeId, out knownType))
                    {
                        knownTypeId = superTypeId;
                        eventTypeMappings.Add(eventTypeId, superTypeId);
                    }

                    if (knownTypeId != null)
                    {
                        break;
                    }
                }

                // can't do anything with unknown types.
                if (knownTypeId == null)
                {
                    return null;
                }
            }

            // construct the event based on the known event type.
            BaseEventState e = (BaseEventState)Activator.CreateInstance(knownType, new object[] { (NodeState)null });

            // get the filter which defines the contents of the notification.
            EventFilter filter = monitoredItem.Status.Filter as EventFilter;

            // initialize the event with the values in the notification.
            e.Update(session.SystemContext, filter.SelectClauses, notification);

            // save the orginal notification.
            e.Handle = notification;

            return e;
        }

        /// <summary>
        /// Returns the node ids for a set of relative paths.
        /// </summary>
        /// <param name="session">An open session with the server to use.</param>
        /// <param name="startNodeId">The starting node for the relative paths.</param>
        /// <param name="namespacesUris">The namespace URIs referenced by the relative paths.</param>
        /// <param name="relativePaths">The relative paths.</param>
        /// <returns>A collection of local nodes.</returns>
        public static List<NodeId> TranslateBrowsePaths(
            Session session,
            NodeId startNodeId,
            NamespaceTable namespacesUris,
            params string[] relativePaths)
        {
            // build the list of browse paths to follow by parsing the relative paths.
            BrowsePathCollection browsePaths = new BrowsePathCollection();

            if (relativePaths != null)
            {
                for (int ii = 0; ii < relativePaths.Length; ii++)
                {
                    BrowsePath browsePath = new BrowsePath();

                    // The relative paths used indexes in the namespacesUris table. These must be 
                    // converted to indexes used by the server. An error occurs if the relative path
                    // refers to a namespaceUri that the server does not recognize.

                    // The relative paths may refer to ReferenceType by their BrowseName. The TypeTree object
                    // allows the parser to look up the server's NodeId for the ReferenceType.

                    browsePath.RelativePath = RelativePath.Parse(
                        relativePaths[ii],
                        session.TypeTree,
                        namespacesUris,
                        session.NamespaceUris);

                    browsePath.StartingNode = startNodeId;

                    browsePaths.Add(browsePath);
                }
            }

            // make the call to the server.
            BrowsePathResultCollection results;
            DiagnosticInfoCollection diagnosticInfos;

            ResponseHeader responseHeader = session.TranslateBrowsePathsToNodeIds(
                null,
                browsePaths,
                out results,
                out diagnosticInfos);

            // ensure that the server returned valid results.
            Session.ValidateResponse(results, browsePaths);
            Session.ValidateDiagnosticInfos(diagnosticInfos, browsePaths);

            // collect the list of node ids found.
            List<NodeId> nodes = new List<NodeId>();

            for (int ii = 0; ii < results.Count; ii++)
            {
                // check if the start node actually exists.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    nodes.Add(null);
                    continue;
                }

                // an empty list is returned if no node was found.
                if (results[ii].Targets.Count == 0)
                {
                    nodes.Add(null);
                    continue;
                }

                // Multiple matches are possible, however, the node that matches the type model is the
                // one we are interested in here. The rest can be ignored.
                BrowsePathTarget target = results[ii].Targets[0];

                if (target.RemainingPathIndex != UInt32.MaxValue)
                {
                    nodes.Add(null);
                    continue;
                }

                // The targetId is an ExpandedNodeId because it could be node in another server. 
                // The ToNodeId function is used to convert a local NodeId stored in a ExpandedNodeId to a NodeId.
                nodes.Add(ExpandedNodeId.ToNodeId(target.TargetId, session.NamespaceUris));
            }

            // return whatever was found.
            return nodes;
        }

        /// <summary>
        /// Collects the fields for the type.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="fieldNodeIds">The node id for the declaration of the field.</param>
        public static void CollectFieldsForType(Session session, NodeId typeId, SimpleAttributeOperandCollection fields, List<NodeId> fieldNodeIds)
        {
            // get the supertypes.
            ReferenceDescriptionCollection supertypes = FormUtils.BrowseSuperTypes(session, typeId, false);

            if (supertypes == null)
            {
                return;
            }

            // process the types starting from the top of the tree.
            Dictionary<NodeId, QualifiedNameCollection> foundNodes = new Dictionary<NodeId, QualifiedNameCollection>();
            QualifiedNameCollection parentPath = new QualifiedNameCollection();

            for (int ii = supertypes.Count - 1; ii >= 0; ii--)
            {
                CollectFields(session, (NodeId)supertypes[ii].NodeId, parentPath, fields, fieldNodeIds, foundNodes);
            }

            // collect the fields for the selected type.
            CollectFields(session, typeId, parentPath, fields, fieldNodeIds, foundNodes);
        }

        /// <summary>
        /// Collects the fields for the instance.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="instanceId">The node id for the declaration of the field.</param>
        public static void CollectFieldsForInstance(Session session, NodeId instanceId, SimpleAttributeOperandCollection fields, List<NodeId> fieldNodeIds)
        {
            Dictionary<NodeId, QualifiedNameCollection> foundNodes = new Dictionary<NodeId, QualifiedNameCollection>();
            QualifiedNameCollection parentPath = new QualifiedNameCollection();
            CollectFields(session, instanceId, parentPath, fields, fieldNodeIds, foundNodes);
        }

        /// <summary>
        /// Collects the fields for the instance node.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="parentPath">The parent path.</param>
        /// <param name="fields">The event fields.</param>
        /// <param name="fieldNodeIds">The node id for the declaration of the field.</param>
        /// <param name="foundNodes">The table of found nodes.</param>
        private static void CollectFields(
            Session session,
            NodeId nodeId,
            QualifiedNameCollection parentPath,
            SimpleAttributeOperandCollection fields,
            List<NodeId> fieldNodeIds,
            Dictionary<NodeId, QualifiedNameCollection> foundNodes)
        {
            // find all of the children of the field.
            BrowseDescription nodeToBrowse = new BrowseDescription();

            nodeToBrowse.NodeId = nodeId;
            nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
            nodeToBrowse.ReferenceTypeId = ReferenceTypeIds.Aggregates;
            nodeToBrowse.IncludeSubtypes = true;
            nodeToBrowse.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable);
            nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

            ReferenceDescriptionCollection children = FormUtils.Browse(session, nodeToBrowse, false);

            if (children == null)
            {
                return;
            }

            // process the children.
            for (int ii = 0; ii < children.Count; ii++)
            {
                ReferenceDescription child = children[ii];

                if (child.NodeId.IsAbsolute)
                {
                    continue;
                }

                // construct browse path.
                QualifiedNameCollection browsePath = new QualifiedNameCollection(parentPath);
                browsePath.Add(child.BrowseName);

                // check if the browse path is already in the list.
                int index = ContainsPath(fields, browsePath);

                if (index < 0)
                {
                    SimpleAttributeOperand field = new SimpleAttributeOperand();

                    field.TypeDefinitionId = ObjectTypeIds.BaseEventType;
                    field.BrowsePath = browsePath;
                    field.AttributeId = (child.NodeClass == NodeClass.Variable) ? Attributes.Value : Attributes.NodeId;

                    fields.Add(field);
                    fieldNodeIds.Add((NodeId)child.NodeId);
                }

                // recusively find all of the children.
                NodeId targetId = (NodeId)child.NodeId;

                // need to guard against loops.
                if (!foundNodes.ContainsKey(targetId))
                {
                    foundNodes.Add(targetId, browsePath);
                    CollectFields(session, (NodeId)child.NodeId, browsePath, fields, fieldNodeIds, foundNodes);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified select clause contains the browse path.
        /// </summary>
        /// <param name="selectClause">The select clause.</param>
        /// <param name="browsePath">The browse path.</param>
        /// <returns>
        /// 	<c>true</c> if the specified select clause contains path; otherwise, <c>false</c>.
        /// </returns>
        private static int ContainsPath(SimpleAttributeOperandCollection selectClause, QualifiedNameCollection browsePath)
        {
            for (int ii = 0; ii < selectClause.Count; ii++)
            {
                SimpleAttributeOperand field = selectClause[ii];

                if (field.BrowsePath.Count != browsePath.Count)
                {
                    continue;
                }

                bool match = true;

                for (int jj = 0; jj < field.BrowsePath.Count; jj++)
                {
                    if (field.BrowsePath[jj] != browsePath[jj])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return ii;
                }
            }

            return -1;
        }
    }
}
