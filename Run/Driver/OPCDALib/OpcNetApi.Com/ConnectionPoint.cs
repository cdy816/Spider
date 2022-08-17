using System;
using OpcRcw.Comn;

namespace OpcCom
{
	// Token: 0x02000033 RID: 51
	public class ConnectionPoint : IDisposable
	{
		// Token: 0x06000200 RID: 512 RVA: 0x00017AC1 File Offset: 0x00016AC1
		public ConnectionPoint(object server, Guid iid)
		{
			((IConnectionPointContainer)server).FindConnectionPoint(ref iid, out this.m_server);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00017ADC File Offset: 0x00016ADC
		public void Dispose()
		{
			if (this.m_server != null)
			{
				while (this.Unadvise() > 0)
				{
				}
				Interop.ReleaseServer(this.m_server);
				this.m_server = null;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00017B01 File Offset: 0x00016B01
		public int Cookie
		{
			get
			{
				return this.m_cookie;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00017B0C File Offset: 0x00016B0C
		public int Advise(object callback)
		{
			if (this.m_refs++ == 0)
			{
				this.m_server.Advise(callback, out this.m_cookie);
			}
			return this.m_refs;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00017B44 File Offset: 0x00016B44
		public int Unadvise()
		{
			if (--this.m_refs == 0)
			{
				this.m_server.Unadvise(this.m_cookie);
			}
			return this.m_refs;
		}

		// Token: 0x04000135 RID: 309
		private IConnectionPoint m_server;

		// Token: 0x04000136 RID: 310
		private int m_cookie;

		// Token: 0x04000137 RID: 311
		private int m_refs;
	}
}
