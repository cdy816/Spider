using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpcRcw.Comn;

namespace OpcRcw
{
	// Token: 0x020000AE RID: 174
	public class ServerEnumerator : IDisposable
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00002DF5 File Offset: 0x00000FF5
		public ServerEnumerator()
		{
			this.Initialize();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00002E03 File Offset: 0x00001003
		private void Initialize()
		{
			this.m_server = null;
			this.m_host = null;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00002E14 File Offset: 0x00001014
		~ServerEnumerator()
		{
			this.Dispose(false);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00002E44 File Offset: 0x00001044
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00002E53 File Offset: 0x00001053
		protected virtual void Dispose(bool disposing)
		{
			if (this.m_server != null)
			{
				Utils.ReleaseServer(this.m_server);
				this.m_server = null;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00002E6F File Offset: 0x0000106F
		public void Connect()
		{
			this.Connect(null, null, null, null);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00002E7C File Offset: 0x0000107C
		public void Connect(string host, string username, string password, string domain)
		{
			this.Disconnect();
			object obj = null;
			try
			{
				obj = Utils.CreateInstance(ServerEnumerator.OPCEnumCLSID, host, username, password, domain);
			}
			catch (Exception e)
			{
				throw Utils.CreateComException(e);
			}
			this.m_server = (obj as IOPCServerList2);
			if (this.m_server == null)
			{
				Utils.ReleaseServer(obj);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Server does not support IOPCServerList2. ");
				stringBuilder.Append("The OPC proxy/stubs may not be installed properly or the client or server machine. ");
				stringBuilder.Append("The also could be a problem with DCOM security configuration.");
				throw Utils.CreateComException(-2147467262, stringBuilder.ToString(), new object[0]);
			}
			this.m_host = host;
			if (string.IsNullOrEmpty(this.m_host))
			{
				this.m_host = "localhost";
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00002F38 File Offset: 0x00001138
		public void Disconnect()
		{
			try
			{
				if (this.m_server != null)
				{
					Utils.ReleaseServer(this.m_server);
					this.m_server = null;
				}
			}
			catch (Exception e)
			{
				throw Utils.CreateComException(e, -2147467259, "Could not release OPCEnum server.", new object[0]);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00002F8C File Offset: 0x0000118C
		public ServerDescription[] GetAvailableServers(params Guid[] catids)
		{
			ServerDescription[] result;
			try
			{
				IOPCEnumGUID iopcenumGUID = null;
				this.m_server.EnumClassesOfCategories(catids.Length, catids, 0, null, out iopcenumGUID);
				List<Guid> list = this.ReadClasses(iopcenumGUID);
				Utils.ReleaseServer(iopcenumGUID);
				iopcenumGUID = null;
				ServerDescription[] array = new ServerDescription[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.ReadServerDetails(list[i]);
				}
				result = array;
			}
			catch (Exception e)
			{
				throw Utils.CreateComException(e, -2147467259, "Could not enumerate COM servers.", new object[0]);
			}
			return result;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000301C File Offset: 0x0000121C
		public Guid CLSIDFromProgID(string progID)
		{
			Guid empty;
			try
			{
				this.m_server.CLSIDFromProgID(progID, out empty);
			}
			catch
			{
				empty = Guid.Empty;
			}
			return empty;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003054 File Offset: 0x00001254
		private List<Guid> ReadClasses(IOPCEnumGUID enumerator)
		{
			List<Guid> list = new List<Guid>();
			int num = 0;
			Guid[] array = new Guid[10];
			do
			{
				try
				{
					IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Guid)) * array.Length);
					try
					{
						enumerator.Next(array.Length, intPtr, out num);
						if (num > 0)
						{
							IntPtr ptr = intPtr;
							for (int i = 0; i < num; i++)
							{
								array[i] = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
								ptr = (IntPtr)(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
								list.Add(array[i]);
							}
						}
					}
					finally
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
				catch
				{
					break;
				}
			}
			while (num > 0);
			return list;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003134 File Offset: 0x00001334
		private ServerDescription ReadServerDetails(Guid clsid)
		{
			ServerDescription serverDescription = new ServerDescription();
			serverDescription.HostName = this.m_host;
			serverDescription.Clsid = clsid;
			string progId = null;
			try
			{
				string description = null;
				string text = null;
				this.m_server.GetClassDetails(ref clsid, out progId, out description, out text);
				if (!string.IsNullOrEmpty(text))
				{
					progId = text;
				}
				serverDescription.Description = description;
				serverDescription.VersionIndependentProgId = text;
			}
			catch
			{
				progId = null;
			}
			serverDescription.ProgId = progId;
			return serverDescription;
		}

		// Token: 0x0400048D RID: 1165
		private IOPCServerList2 m_server;

		// Token: 0x0400048E RID: 1166
		private string m_host;

		// Token: 0x0400048F RID: 1167
		private static readonly Guid OPCEnumCLSID = new Guid("13486D51-4821-11D2-A494-3CB306C10000");
	}
}
