using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Opc;
using OpcCom.Ae;
using OpcCom.Da;
using OpcCom.Da20;
using OpcCom.Dx;
using OpcCom.Hda;
using OpcRcw.Ae;
using OpcRcw.Da;
using OpcRcw.Dx;
using OpcRcw.Hda;

namespace OpcCom
{
    [Serializable]
    public class Factory : Opc.Factory
    {
        public Factory()
            : base(null, useRemoting: false)
        {
        }

        public Factory(bool useRemoting)
            : base(null, useRemoting)
        {
        }

        protected Factory(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override IServer CreateInstance(URL url, ConnectData connectData)
        {
            object obj = Connect(url, connectData);
            if (obj == null)
            {
                return null;
            }

            Server server = null;
            System.Type type = null;
            try
            {
                if (url.Scheme == "opcda")
                {
                    if (!typeof(IOPCServer).IsInstanceOfType(obj))
                    {
                        type = typeof(IOPCServer);
                        throw new NotSupportedException();
                    }

                    if (typeof(IOPCBrowse).IsInstanceOfType(obj) && typeof(IOPCItemIO).IsInstanceOfType(obj))
                    {
                        server = new OpcCom.Da.Server(url, obj);
                    }
                    else
                    {
                        if (!typeof(IOPCItemProperties).IsInstanceOfType(obj))
                        {
                            type = typeof(IOPCItemProperties);
                            throw new NotSupportedException();
                        }

                        server = new OpcCom.Da20.Server(url, obj);
                    }
                }
                else if (url.Scheme == "opcae")
                {
                    if (!typeof(IOPCEventServer).IsInstanceOfType(obj))
                    {
                        type = typeof(IOPCEventServer);
                        throw new NotSupportedException();
                    }

                    server = new OpcCom.Ae.Server(url, obj);
                }
                else if (url.Scheme == "opchda")
                {
                    if (!typeof(IOPCHDA_Server).IsInstanceOfType(obj))
                    {
                        type = typeof(IOPCHDA_Server);
                        throw new NotSupportedException();
                    }

                    server = new OpcCom.Hda.Server(url, obj);
                }
                else
                {
                    if (!(url.Scheme == "opcdx"))
                    {
                        throw new NotSupportedException($"The URL scheme '{url.Scheme}' is not supported.");
                    }

                    if (!typeof(IOPCConfiguration).IsInstanceOfType(obj))
                    {
                        type = typeof(IOPCConfiguration);
                        throw new NotSupportedException();
                    }

                    server = new OpcCom.Dx.Server(url, obj);
                }
            }
            catch (NotSupportedException ex)
            {
                Interop.ReleaseServer(server);
                server = null;
                if ((object)type != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("The COM server does not support the interface ");
                    stringBuilder.AppendFormat("'{0}'.", type.FullName);
                    stringBuilder.Append("\r\n\r\nThis problem could be caused by:\r\n");
                    stringBuilder.Append("- incorrectly installed proxy/stubs.\r\n");
                    stringBuilder.Append("- problems with the DCOM security settings.\r\n");
                    stringBuilder.Append("- a personal firewall (sometimes activated by default).\r\n");
                    throw new NotSupportedException(stringBuilder.ToString());
                }

                throw ex;
            }
            catch (Exception ex2)
            {
                Interop.ReleaseServer(server);
                server = null;
                throw ex2;
            }

            server?.Initialize(url, connectData);
            return server;
        }

        public static object Connect(URL url, ConnectData connectData)
        {
            string text = url.Path;
            string text2 = null;
            int num = url.Path.LastIndexOf('/');
            if (num >= 0)
            {
                text = url.Path.Substring(0, num);
                text2 = url.Path.Substring(num + 1);
            }

            Guid guid;
            if (text2 == null)
            {
                guid = new ServerEnumerator().CLSIDFromProgID(text, url.HostName, connectData);
                if (guid == Guid.Empty)
                {
                    try
                    {
                        guid = new Guid(text);
                    }
                    catch
                    {
                        throw new ConnectFailedException(text);
                    }
                }
            }
            else
            {
                try
                {
                    guid = new Guid(text2);
                }
                catch
                {
                    throw new ConnectFailedException(text2);
                }
            }

            NetworkCredential credential = connectData?.GetCredential(null, null);
            if (connectData == null || connectData.LicenseKey == null)
            {
                try
                {
                    return Interop.CreateInstance(guid, url.HostName, credential);
                }
                catch (Exception e)
                {
                    throw new ConnectFailedException(e);
                }
            }

            try
            {
                return Interop.CreateInstanceWithLicenseKey(guid, url.HostName, credential, connectData.LicenseKey);
            }
            catch (Exception e2)
            {
                throw new ConnectFailedException(e2);
            }
        }
    }
}
