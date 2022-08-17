using System;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom.Da.Wrapper
{
	// Token: 0x02000022 RID: 34
	[CLSCompliant(false)]
	public class ConnectionPoint : IConnectionPoint
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00012E78 File Offset: 0x00011E78
		public ConnectionPoint(Guid iid, ConnectionPointContainer container)
		{
			this.m_interface = iid;
			this.m_container = container;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00012E99 File Offset: 0x00011E99
		public object Callback
		{
			get
			{
				return this.m_callback;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00012EA1 File Offset: 0x00011EA1
		public bool IsConnected
		{
			get
			{
				return this.m_callback != null;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00012EB0 File Offset: 0x00011EB0
		public void Advise(object pUnkSink, out int pdwCookie)
		{
			lock (this)
			{
				try
				{
					if (pUnkSink == null)
					{
						throw new ExternalException("E_POINTER", -2147467261);
					}
					pdwCookie = 0;
					if (this.m_callback != null)
					{
						throw new ExternalException("CONNECT_E_ADVISELIMIT", -2147220991);
					}
					this.m_callback = pUnkSink;
					pdwCookie = ++this.m_cookie;
					this.m_container.OnAdvise(this.m_interface);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00012F50 File Offset: 0x00011F50
		public void Unadvise(int dwCookie)
		{
			lock (this)
			{
				try
				{
					if (this.m_cookie != dwCookie || this.m_callback == null)
					{
						throw new ExternalException("CONNECT_E_NOCONNECTION", -2147220992);
					}
					this.m_callback = null;
					this.m_container.OnUnadvise(this.m_interface);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00012FD0 File Offset: 0x00011FD0
		public void GetConnectionInterface(out Guid pIID)
		{
			lock (this)
			{
				try
				{
					pIID = this.m_interface;
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00013020 File Offset: 0x00012020
		public void EnumConnections(out IEnumConnections ppenum)
		{
			throw new ExternalException("E_NOTIMPL", -2147467263);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00013034 File Offset: 0x00012034
		public void GetConnectionPointContainer(out IConnectionPointContainer ppCPC)
		{
			lock (this)
			{
				try
				{
					ppCPC = this.m_container;
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x040000A7 RID: 167
		private Guid m_interface = Guid.Empty;

		// Token: 0x040000A8 RID: 168
		private ConnectionPointContainer m_container;

		// Token: 0x040000A9 RID: 169
		private object m_callback;

		// Token: 0x040000AA RID: 170
		private int m_cookie;
	}
}
