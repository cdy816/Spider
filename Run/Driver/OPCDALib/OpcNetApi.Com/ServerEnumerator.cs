using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
using Opc;
using Opc.Ae;
using Opc.Da;
using Opc.Dx;
using Opc.Hda;
using OpcRcw.Comn;

namespace OpcCom
{
	// Token: 0x02000002 RID: 2
	public class ServerEnumerator : IDiscovery, IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		public void Dispose()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D2 File Offset: 0x000010D2
		public string[] EnumerateHosts()
		{
			return Interop.EnumComputers();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D9 File Offset: 0x000010D9
		public Opc.Server[] GetAvailableServers(Specification specification)
		{
			return this.GetAvailableServers(specification, null, null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E4 File Offset: 0x000010E4
		public Opc.Server[] GetAvailableServers(Specification specification, string host, ConnectData connectData)
		{
			Opc.Server[] result;
			lock (this)
			{
				NetworkCredential credential = (connectData != null) ? connectData.GetCredential(null, null) : null;

                //           System.Type typeOfList = System.Type.GetTypeFromCLSID(ServerEnumerator.CLSID);
                //           var obj = Activator.CreateInstance(typeOfList, host);
                //this.m_server = obj as IOPCServerList2;
                this.m_server = (IOPCServerList2)Interop.CreateInstance(ServerEnumerator.CLSID, host, credential);
                this.m_host = host;
				try
				{
					ArrayList arrayList = new ArrayList();
					Guid guid = new Guid(specification.ID);
					IOPCEnumGUID iopcenumGUID = null;
					this.m_server.EnumClassesOfCategories(1, new Guid[]
					{
						guid
					}, 0, null, out iopcenumGUID);
					Guid[] array = this.ReadClasses(iopcenumGUID);
					Interop.ReleaseServer(iopcenumGUID);
					iopcenumGUID = null;
					foreach (Guid clsid in array)
					{
						Factory factory = new Factory();
						try
						{
							URL url = this.CreateUrl(specification, clsid);
							Opc.Server value = null;
							if (specification == Specification.COM_DA_30)
							{
								value = new Opc.Da.Server(factory, url);
							}
							else if (specification == Specification.COM_DA_20)
							{
								value = new Opc.Da.Server(factory, url);
							}
							else if (specification == Specification.COM_AE_10)
							{
								value = new Opc.Ae.Server(factory, url);
							}
							else if (specification == Specification.COM_HDA_10)
							{
								value = new Opc.Hda.Server(factory, url);
							}
							else if (specification == Specification.COM_DX_10)
							{
								value = new Opc.Dx.Server(factory, url);
							}
							arrayList.Add(value);
						}
						catch (Exception)
						{
						}
					}
					result = (Opc.Server[])arrayList.ToArray(typeof(Opc.Server));
				}
				finally
				{
					Interop.ReleaseServer(this.m_server);
					this.m_server = null;
				}
			}
			return result;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022C8 File Offset: 0x000012C8
		public Guid CLSIDFromProgID(string progID, string host, ConnectData connectData)
		{
			Guid result;
			lock (this)
			{
				NetworkCredential credential = (connectData != null) ? connectData.GetCredential(null, null) : null;
				this.m_server = (IOPCServerList2)Interop.CreateInstance(ServerEnumerator.CLSID, host, credential);
				this.m_host = host;
				Guid empty;
				try
				{
					this.m_server.CLSIDFromProgID(progID, out empty);
				}
				catch
				{
					empty = Guid.Empty;
				}
				finally
				{
					Interop.ReleaseServer(this.m_server);
					this.m_server = null;
				}
				result = empty;
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000236C File Offset: 0x0000136C
		private Guid[] ReadClasses(IOPCEnumGUID enumerator)
		{
			ArrayList arrayList = new ArrayList();
			int num = 0;
			int num2 = 10;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Guid)) * num2);
			Guid[] result;
			try
			{
				do
				{
					try
					{
						enumerator.Next(num2, intPtr, out num);
						IntPtr ptr = intPtr;
						for (int i = 0; i < num; i++)
						{
							Guid guid = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
							arrayList.Add(guid);
							ptr = (IntPtr)(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
						}
					}
					catch
					{
						break;
					}
				}
				while (num > 0);
				result = (Guid[])arrayList.ToArray(typeof(Guid));
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002444 File Offset: 0x00001444
		private URL CreateUrl(Specification specification, Guid clsid)
		{
			URL url = new URL();
			url.HostName = this.m_host;
			url.Port = 0;
			url.Path = null;
			if (specification == Specification.COM_DA_30)
			{
				url.Scheme = "opcda";
			}
			else if (specification == Specification.COM_DA_20)
			{
				url.Scheme = "opcda";
			}
			else if (specification == Specification.COM_DA_10)
			{
				url.Scheme = "opcda";
			}
			else if (specification == Specification.COM_DX_10)
			{
				url.Scheme = "opcdx";
			}
			else if (specification == Specification.COM_AE_10)
			{
				url.Scheme = "opcae";
			}
			else if (specification == Specification.COM_HDA_10)
			{
				url.Scheme = "opchda";
			}
			else if (specification == Specification.COM_BATCH_10)
			{
				url.Scheme = "opcbatch";
			}
			else if (specification == Specification.COM_BATCH_20)
			{
				url.Scheme = "opcbatch";
			}
			try
			{
				string text = null;
				string text2 = null;
				string text3 = null;
				this.m_server.GetClassDetails(ref clsid, out text, out text2, out text3);
				if (text3 != null)
				{
					url.Path = string.Format("{0}/{1}", text3, "{" + clsid.ToString() + "}");
				}
				else if (text != null)
				{
					url.Path = string.Format("{0}/{1}", text, "{" + clsid.ToString() + "}");
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (url.Path == null)
				{
					url.Path = string.Format("{0}", "{" + clsid.ToString() + "}");
				}
			}
			return url;
		}

		// Token: 0x04000001 RID: 1
		private const string ProgID = "OPC.ServerList.1";

		// Token: 0x04000002 RID: 2
		private IOPCServerList2 m_server;

		// Token: 0x04000003 RID: 3
		private string m_host;

		// Token: 0x04000004 RID: 4
		private static readonly Guid CLSID = new Guid("13486D51-4821-11D2-A494-3CB306C10000");
	}
}
