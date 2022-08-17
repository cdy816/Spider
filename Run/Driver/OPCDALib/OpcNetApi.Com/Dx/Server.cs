using System;
using Opc;
using Opc.Da;
using Opc.Dx;
using OpcCom.Da;
using OpcRcw.Dx;

namespace OpcCom.Dx
{
    [Serializable]
    public class Server : OpcCom.Da.Server, Opc.Dx.IServer, Opc.Da.IServer, Opc.IServer, IDisposable
    {
        public Server(URL url, object server)
            : base(url, server)
        {
        }

        public Opc.Dx.SourceServer[] GetSourceServers()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    int pdwCount = 0;
                    IntPtr ppServers = IntPtr.Zero;
                    ((IOPCConfiguration)m_server).GetServers(out pdwCount, out ppServers);
                    return Interop.GetSourceServers(ref ppServers, pdwCount, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.GetServers", e);
                }
            }
        }

        public GeneralResponse AddSourceServers(Opc.Dx.SourceServer[] servers)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.SourceServer[] sourceServers = Interop.GetSourceServers(servers);
                    ((IOPCConfiguration)m_server).AddServers(sourceServers.Length, sourceServers, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.AddServers", e);
                }
            }
        }

        public GeneralResponse ModifySourceServers(Opc.Dx.SourceServer[] servers)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.SourceServer[] sourceServers = Interop.GetSourceServers(servers);
                    ((IOPCConfiguration)m_server).ModifyServers(sourceServers.Length, sourceServers, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ModifyServers", e);
                }
            }
        }

        public GeneralResponse DeleteSourceServers(Opc.Dx.ItemIdentifier[] servers)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.ItemIdentifier[] itemIdentifiers = Interop.GetItemIdentifiers(servers);
                    ((IOPCConfiguration)m_server).DeleteServers(itemIdentifiers.Length, itemIdentifiers, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.DeleteServers", e);
                }
            }
        }

        public GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, Opc.Dx.ItemIdentifier[] servers)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.ItemIdentifier[] itemIdentifiers = Interop.GetItemIdentifiers(servers);
                    ((IOPCConfiguration)m_server).CopyDefaultServerAttributes(configToStatus ? 1 : 0, itemIdentifiers.Length, itemIdentifiers, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.CopyDefaultServerAttributes", e);
                }
            }
        }

        public Opc.Dx.DXConnection[] QueryDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connectionMasks);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    int pdwCount = 0;
                    IntPtr ppErrors = IntPtr.Zero;
                    IntPtr ppConnections = IntPtr.Zero;
                    ((IOPCConfiguration)m_server).QueryDXConnections((browsePath != null) ? browsePath : "", array.Length, array, recursive ? 1 : 0, out ppErrors, out pdwCount, out ppConnections);
                    errors = Interop.GetResultIDs(ref ppErrors, array.Length, deallocate: true);
                    return Interop.GetDXConnections(ref ppConnections, pdwCount, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.QueryDXConnections", e);
                }
            }
        }

        public GeneralResponse AddDXConnections(Opc.Dx.DXConnection[] connections)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connections);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    ((IOPCConfiguration)m_server).AddDXConnections(array.Length, array, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.AddDXConnections", e);
                }
            }
        }

        public GeneralResponse ModifyDXConnections(Opc.Dx.DXConnection[] connections)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connections);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    ((IOPCConfiguration)m_server).ModifyDXConnections(array.Length, array, out DXGeneralResponse pResponse);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ModifyDXConnections", e);
                }
            }
        }

        public GeneralResponse UpdateDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, Opc.Dx.DXConnection connectionDefinition, out ResultID[] errors)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connectionMasks);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    OpcRcw.Dx.DXConnection pDXConnectionDefinition = Interop.GetDXConnection(connectionDefinition);
                    IntPtr ppErrors = IntPtr.Zero;
                    ((IOPCConfiguration)m_server).UpdateDXConnections((browsePath != null) ? browsePath : "", array.Length, array, recursive ? 1 : 0, ref pDXConnectionDefinition, out ppErrors, out DXGeneralResponse pResponse);
                    errors = Interop.GetResultIDs(ref ppErrors, array.Length, deallocate: true);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.UpdateDXConnections", e);
                }
            }
        }

        public GeneralResponse DeleteDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connectionMasks);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    IntPtr ppErrors = IntPtr.Zero;
                    ((IOPCConfiguration)m_server).DeleteDXConnections((browsePath != null) ? browsePath : "", array.Length, array, recursive ? 1 : 0, out ppErrors, out DXGeneralResponse pResponse);
                    errors = Interop.GetResultIDs(ref ppErrors, array.Length, deallocate: true);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.DeleteDXConnections", e);
                }
            }
        }

        public GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    OpcRcw.Dx.DXConnection[] array = Interop.GetDXConnections(connectionMasks);
                    if (array == null)
                    {
                        array = new OpcRcw.Dx.DXConnection[0];
                    }

                    IntPtr ppErrors = IntPtr.Zero;
                    ((IOPCConfiguration)m_server).CopyDXConnectionDefaultAttributes(configToStatus ? 1 : 0, (browsePath != null) ? browsePath : "", array.Length, array, recursive ? 1 : 0, out ppErrors, out DXGeneralResponse pResponse);
                    errors = Interop.GetResultIDs(ref ppErrors, array.Length, deallocate: true);
                    return Interop.GetGeneralResponse(pResponse, deallocate: true);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.CopyDXConnectionDefaultAttributes", e);
                }
            }
        }

        public string ResetConfiguration(string configurationVersion)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    string pszConfigurationVersion = null;
                    ((IOPCConfiguration)m_server).ResetConfiguration(configurationVersion, out pszConfigurationVersion);
                    return pszConfigurationVersion;
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ResetConfiguration", e);
                }
            }
        }
    }
}
