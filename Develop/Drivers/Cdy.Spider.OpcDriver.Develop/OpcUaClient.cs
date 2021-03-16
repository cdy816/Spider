using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OpcDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcUaClient
    {

        #region ... Variables  ...

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

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 默认的构造函数，实例化一个新的OPC UA类
        /// </summary>
        public OpcUaClient()
        {
            var certificateValidator = new CertificateValidator();
            certificateValidator.CertificateValidation += (sender, eventArgs) =>
            {
                if (ServiceResult.IsGood(eventArgs.Error))
                    eventArgs.Accept = true;
                else if (eventArgs.Error.StatusCode.Code == StatusCodes.BadCertificateUntrusted)
                    eventArgs.Accept = true;
                else
                    LoggerService.Service.Warn("OpcClient", string.Format("Failed to validate certificate with error code {0}: {1}", eventArgs.Error.Code, eventArgs.Error.AdditionalInfo));
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
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
            /// 
            /// </summary>
        public bool UseSecurity
        {
            get
            {
                return m_useSecurity;
            }
            set
            {
                if (m_useSecurity != value)
                {
                    m_useSecurity = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OpcUaName
        {
            get
            {
                return "SpiderOpcClient";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return m_IsConnected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ApplicationConfiguration Configuration
        {
            get
            {
                return m_configuration;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public IUserIdentity UserIdentity { get; set; }

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

        #endregion ...Properties...

        #region ... Methods    ...

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
                LoggerService.Service.Info("OpcClient", exception.Message);
            }
        }

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
        /// Raises the connect complete event on the main GUI thread.
        /// </summary>
        private void DoConnectComplete(object state)
        {
            m_ConnectComplete?.Invoke(this, null);
        }


        #region Browse

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="targetId"></param>
        /// <param name="m_referenceTypeIds"></param>
        /// <returns></returns>
        public ReferenceDescriptionCollection Browse(NodeId targetId, params NodeId[] m_referenceTypeIds)
        {
            BrowseDescriptionCollection nb = new BrowseDescriptionCollection();

            for (int ii = 0; ii < m_referenceTypeIds.Length; ii++)
            {
                BrowseDescription nodeToBrowse = new BrowseDescription();

                nodeToBrowse.NodeId = targetId;
                nodeToBrowse.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse.ReferenceTypeId = m_referenceTypeIds[ii];
                nodeToBrowse.IncludeSubtypes = true;
                nodeToBrowse.NodeClassMask = 0;
                nodeToBrowse.ResultMask = (uint)BrowseResultMask.All;

                nb.Add(nodeToBrowse);
            }
            return Browse(this.m_session, nb, false);
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
            return Browse(session, null, nodesToBrowse, throwOnError);
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        public static ReferenceDescriptionCollection Browse(Session session, ViewDescription view, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
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
                        view,
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
                            false,
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
            return Browse(session, null, nodeToBrowse, throwOnError);
        }

        /// <summary>
        /// Browses the address space and returns the references found.
        /// </summary>
        public static ReferenceDescriptionCollection Browse(Session session, ViewDescription view, BrowseDescription nodeToBrowse, bool throwOnError)
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
                    view,
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
        #endregion

        /// <summary>
        /// Reads the attributes for the node.
        /// </summary>
        public Dictionary<string,string> ReadAttributes(NodeId nodeId)
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            if (NodeId.IsNull(nodeId))
            {
                return re;
            }

            // build list of attributes to read.
            ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

            foreach (uint attributeId in new uint[Attributes.NodeId,Attributes.BrowseName,Attributes.DisplayName,Attributes.Description,Attributes.DataType,Attributes.AccessLevel])
            {
                ReadValueId nodeToRead = new ReadValueId();
                nodeToRead.NodeId = nodeId;
                nodeToRead.AttributeId = attributeId;
                nodesToRead.Add(nodeToRead);
            }

            // read the attributes.
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

            // add the results to the display.
            for (int ii = 0; ii < results.Count; ii++)
            {
                // check for error.
                if (StatusCode.IsBad(results[ii].StatusCode))
                {
                    if (results[ii].StatusCode == StatusCodes.BadAttributeIdInvalid)
                    {
                        continue;
                    }
                }

                // add the metadata for the attribute.
                uint attributeId = nodesToRead[ii].AttributeId;
                string skey = Attributes.GetBrowseName(attributeId);

                string sval = GetAttributeDisplayText(m_session, attributeId, results[ii].WrappedValue);

                re.Add(skey, sval);
            }

            return re;
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

                case Attributes.DataTypeDefinition:
                    {
                        ExtensionObject field = value.Value as ExtensionObject;
                        if (field != null)
                        {
                            return field.ToString();
                        }
                        break;
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
        /// Gets the display text for the access level attribute.
        /// </summary>
        /// <param name="accessLevel">The access level.</param>
        /// <returns>The access level formatted as a string.</returns>
        public static string GetAccessLevelDisplayText(byte accessLevel)
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
        public static string GetEventNotifierDisplayText(byte eventNotifier)
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
        public static string GetValueRankDisplayText(int valueRank)
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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public static class OpcUaClientExtends
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static List<string> DiscoverServers(this ApplicationConfiguration configuration,string hostname="localhost")
        {
            List<string> serverUrls = new List<string>();

            // set a short timeout because this is happening in the drop down event.
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(configuration);
            endpointConfiguration.OperationTimeout = 5000;

            // Connect to the local discovery server and find the available servers.
            using (DiscoveryClient client = DiscoveryClient.Create(new Uri("opc.tcp://"+hostname+":4840"), endpointConfiguration))
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
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="nodesToBrowse"></param>
        /// <returns></returns>
        public static ReferenceDescriptionCollection BrowsePart(this Session session, BrowseDescriptionCollection nodesToBrowse)
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
                    }
                    // check if unprocessed results exist.
                    nodesToBrowse = unprocessedOperations;
                }

                // return complete list.
                return references;
            }
            catch (Exception)
            {
                return null;
            }
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
        public static ReferenceDescriptionCollection Browse(this Session session, BrowseDescriptionCollection nodesToBrowse, bool throwOnError)
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
    }


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
}
