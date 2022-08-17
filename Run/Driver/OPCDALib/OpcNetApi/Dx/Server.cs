using System;
using System.Runtime.Serialization;
using Opc.Da;

namespace Opc.Dx
{
    // Token: 0x0200000A RID: 10
    [Serializable]
    public class Server : Opc.Server, IServer, Opc.IServer, IDisposable, ISerializable
    {
        // Token: 0x0600005A RID: 90 RVA: 0x000036B8 File Offset: 0x000026B8
        public Server(Factory factory, URL url) : base(factory, url)
        {
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600005B RID: 91 RVA: 0x000036D8 File Offset: 0x000026D8
        public string Version
        {
            get
            {
                return this.m_version;
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600005C RID: 92 RVA: 0x000036E0 File Offset: 0x000026E0
        public SourceServerCollection SourceServers
        {
            get
            {
                return this.m_sourceServers;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600005D RID: 93 RVA: 0x000036E8 File Offset: 0x000026E8
        public DXConnectionQueryCollection Queries
        {
            get
            {
                return this.m_connectionQueries;
            }
        }

        // Token: 0x0600005E RID: 94 RVA: 0x000036F0 File Offset: 0x000026F0
        public SourceServer AddSourceServer(SourceServer server)
        {
            GeneralResponse generalResponse = this.AddSourceServers(new SourceServer[]
            {
                server
            });
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
            return new SourceServer(server)
            {
                ItemName = generalResponse[0].ItemName,
                ItemPath = generalResponse[0].ItemPath,
                Version = generalResponse[0].Version
            };
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00003788 File Offset: 0x00002788
        public SourceServer ModifySourceServer(SourceServer server)
        {
            GeneralResponse generalResponse = this.ModifySourceServers(new SourceServer[]
            {
                server
            });
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
            return new SourceServer(server)
            {
                ItemName = generalResponse[0].ItemName,
                ItemPath = generalResponse[0].ItemPath,
                Version = generalResponse[0].Version
            };
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00003820 File Offset: 0x00002820
        public void DeleteSourceServer(SourceServer server)
        {
            GeneralResponse generalResponse = this.DeleteSourceServers(new ItemIdentifier[]
            {
                server
            });
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
        }

        // Token: 0x06000061 RID: 97 RVA: 0x0000387C File Offset: 0x0000287C
        public DXConnection AddDXConnection(DXConnection connection)
        {
            GeneralResponse generalResponse = this.AddDXConnections(new DXConnection[]
            {
                connection
            });
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
            return new DXConnection(connection)
            {
                ItemName = generalResponse[0].ItemName,
                ItemPath = generalResponse[0].ItemPath,
                Version = generalResponse[0].Version
            };
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00003914 File Offset: 0x00002914
        public DXConnection ModifyDXConnection(DXConnection connection)
        {
            GeneralResponse generalResponse = this.ModifyDXConnections(new DXConnection[]
            {
                connection
            });
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
            return new DXConnection(connection)
            {
                ItemName = generalResponse[0].ItemName,
                ItemPath = generalResponse[0].ItemPath,
                Version = generalResponse[0].Version
            };
        }

        // Token: 0x06000063 RID: 99 RVA: 0x000039AC File Offset: 0x000029AC
        public void DeleteDXConnections(DXConnection connection)
        {
            ResultID[] array = null;
            GeneralResponse generalResponse = this.DeleteDXConnections(null, new DXConnection[]
            {
                connection
            }, true, out array);
            if (array != null && array.Length > 0 && array[0].Failed())
            {
                throw new ResultIDException(array[0]);
            }
            if (generalResponse == null || generalResponse.Count != 1)
            {
                throw new InvalidResponseException();
            }
            if (generalResponse[0].ResultID.Failed())
            {
                throw new ResultIDException(generalResponse[0].ResultID);
            }
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00003A38 File Offset: 0x00002A38
        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            DXConnectionQuery[] array = (DXConnectionQuery[])info.GetValue("Queries", typeof(DXConnectionQuery[]));
            if (array != null)
            {
                foreach (DXConnectionQuery value in array)
                {
                    this.m_connectionQueries.Add(value);
                }
            }
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00003AA4 File Offset: 0x00002AA4
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            DXConnectionQuery[] array = null;
            if (this.m_connectionQueries.Count > 0)
            {
                array = new DXConnectionQuery[this.m_connectionQueries.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = this.m_connectionQueries[i];
                }
            }
            info.AddValue("Queries", array);
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00003B04 File Offset: 0x00002B04
        public SourceServer[] GetSourceServers()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            SourceServer[] sourceServers = ((IServer)this.m_server).GetSourceServers();
            this.m_sourceServers.Initialize(sourceServers);
            return sourceServers;
        }

        // Token: 0x06000067 RID: 103 RVA: 0x00003B40 File Offset: 0x00002B40
        public GeneralResponse AddSourceServers(SourceServer[] servers)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).AddSourceServers(servers);
            if (generalResponse != null)
            {
                this.GetSourceServers();
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00003B84 File Offset: 0x00002B84
        public GeneralResponse ModifySourceServers(SourceServer[] servers)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).ModifySourceServers(servers);
            if (generalResponse != null)
            {
                this.GetSourceServers();
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x06000069 RID: 105 RVA: 0x00003BC8 File Offset: 0x00002BC8
        public GeneralResponse DeleteSourceServers(ItemIdentifier[] servers)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).DeleteSourceServers(servers);
            if (generalResponse != null)
            {
                this.GetSourceServers();
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00003C0C File Offset: 0x00002C0C
        public GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, ItemIdentifier[] servers)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).CopyDefaultSourceServerAttributes(configToStatus, servers);
            if (generalResponse != null)
            {
                if (!configToStatus)
                {
                    this.GetSourceServers();
                }
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00003C54 File Offset: 0x00002C54
        public DXConnection[] QueryDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).QueryDXConnections(browsePath, connectionMasks, recursive, out errors);
        }

        // Token: 0x0600006C RID: 108 RVA: 0x00003C7C File Offset: 0x00002C7C
        public GeneralResponse AddDXConnections(DXConnection[] connections)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).AddDXConnections(connections);
            if (generalResponse != null)
            {
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00003CBC File Offset: 0x00002CBC
        public GeneralResponse ModifyDXConnections(DXConnection[] connections)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).ModifyDXConnections(connections);
            if (generalResponse != null)
            {
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00003CFC File Offset: 0x00002CFC
        public GeneralResponse UpdateDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, DXConnection connectionDefinition, out ResultID[] errors)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).UpdateDXConnections(browsePath, connectionMasks, recursive, connectionDefinition, out errors);
            if (generalResponse != null)
            {
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00003D40 File Offset: 0x00002D40
        public GeneralResponse DeleteDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).DeleteDXConnections(browsePath, connectionMasks, recursive, out errors);
            if (generalResponse != null)
            {
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x06000070 RID: 112 RVA: 0x00003D84 File Offset: 0x00002D84
        public GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse generalResponse = ((IServer)this.m_server).CopyDXConnectionDefaultAttributes(configToStatus, browsePath, connectionMasks, recursive, out errors);
            if (generalResponse != null)
            {
                this.m_version = generalResponse.Version;
            }
            return generalResponse;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x00003DC7 File Offset: 0x00002DC7
        public string ResetConfiguration(string configurationVersion)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_version = ((IServer)this.m_server).ResetConfiguration((configurationVersion == null) ? this.m_version : configurationVersion);
            return this.m_version;
        }

        // Token: 0x04000011 RID: 17
        private string m_version;

        // Token: 0x04000012 RID: 18
        private SourceServerCollection m_sourceServers = new SourceServerCollection();

        // Token: 0x04000013 RID: 19
        private DXConnectionQueryCollection m_connectionQueries = new DXConnectionQueryCollection();

        // Token: 0x0200000B RID: 11
        private class Names
        {
            // Token: 0x04000014 RID: 20
            internal const string QUERIES = "Queries";
        }
    }
}
