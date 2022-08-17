using System;
using System.Collections;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom.Da.Wrapper
{
	// Token: 0x0200001D RID: 29
	[CLSCompliant(false)]
	public class ConnectionPointContainer : IConnectionPointContainer
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000DE4A File Offset: 0x0000CE4A
		public virtual void OnAdvise(Guid riid)
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000DE4C File Offset: 0x0000CE4C
		public virtual void OnUnadvise(Guid riid)
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000DE4E File Offset: 0x0000CE4E
		protected ConnectionPointContainer()
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000DE61 File Offset: 0x0000CE61
		protected void RegisterInterface(Guid iid)
		{
			this.m_connectionPoints[iid] = new ConnectionPoint(iid, this);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000DE7B File Offset: 0x0000CE7B
		protected void UnregisterInterface(Guid iid)
		{
			this.m_connectionPoints.Remove(iid);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000DE90 File Offset: 0x0000CE90
		protected object GetCallback(Guid iid)
		{
			ConnectionPoint connectionPoint = (ConnectionPoint)this.m_connectionPoints[iid];
			if (connectionPoint != null)
			{
				return connectionPoint.Callback;
			}
			return null;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000DEC0 File Offset: 0x0000CEC0
		protected bool IsConnected(Guid iid)
		{
			ConnectionPoint connectionPoint = (ConnectionPoint)this.m_connectionPoints[iid];
			return connectionPoint != null && connectionPoint.IsConnected;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000DEF0 File Offset: 0x0000CEF0
		public void EnumConnectionPoints(out IEnumConnectionPoints ppenum)
		{
			lock (this)
			{
				try
				{
					ppenum = new EnumConnectionPoints(this.m_connectionPoints.Values);
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000DF48 File Offset: 0x0000CF48
		public void FindConnectionPoint(ref Guid riid, out IConnectionPoint ppCP)
		{
			lock (this)
			{
				try
				{
					ppCP = null;
					ConnectionPoint connectionPoint = (ConnectionPoint)this.m_connectionPoints[riid];
					if (connectionPoint == null)
					{
						throw new ExternalException("CONNECT_E_NOCONNECTION", -2147220992);
					}
					ppCP = connectionPoint;
				}
				catch (Exception e)
				{
					throw Server.CreateException(e);
				}
			}
		}

		// Token: 0x04000091 RID: 145
		private Hashtable m_connectionPoints = new Hashtable();
	}
}
