using System;

namespace Opc.Da
{
	// Token: 0x020000E3 RID: 227
	[Serializable]
	public class Request : IRequest
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000129D5 File Offset: 0x000119D5
		public ISubscription Subscription
		{
			get
			{
				return this.m_subscription;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000129DD File Offset: 0x000119DD
		public object Handle
		{
			get
			{
				return this.m_handle;
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000129E5 File Offset: 0x000119E5
		public void Cancel(CancelCompleteEventHandler callback)
		{
			this.m_subscription.Cancel(this, callback);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000129F4 File Offset: 0x000119F4
		public Request(ISubscription subscription, object handle)
		{
			this.m_subscription = subscription;
			this.m_handle = handle;
		}

		// Token: 0x04000369 RID: 873
		private ISubscription m_subscription;

		// Token: 0x0400036A RID: 874
		private object m_handle;
	}
}
