using System;

namespace Opc.Ae
{
	// Token: 0x020000C5 RID: 197
	[Serializable]
	public class SubscriptionState : ICloneable
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00011025 File Offset: 0x00010025
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001102D File Offset: 0x0001002D
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00011036 File Offset: 0x00010036
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x0001103E File Offset: 0x0001003E
		public object ClientHandle
		{
			get
			{
				return this.m_clientHandle;
			}
			set
			{
				this.m_clientHandle = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00011047 File Offset: 0x00010047
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x0001104F File Offset: 0x0001004F
		public bool Active
		{
			get
			{
				return this.m_active;
			}
			set
			{
				this.m_active = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00011058 File Offset: 0x00010058
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00011060 File Offset: 0x00010060
		public int BufferTime
		{
			get
			{
				return this.m_bufferTime;
			}
			set
			{
				this.m_bufferTime = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00011069 File Offset: 0x00010069
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00011071 File Offset: 0x00010071
		public int MaxSize
		{
			get
			{
				return this.m_maxSize;
			}
			set
			{
				this.m_maxSize = value;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001107A File Offset: 0x0001007A
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00011082 File Offset: 0x00010082
		public int KeepAlive
		{
			get
			{
				return this.m_keepAlive;
			}
			set
			{
				this.m_keepAlive = value;
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001109A File Offset: 0x0001009A
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040002EF RID: 751
		private string m_name;

		// Token: 0x040002F0 RID: 752
		private object m_clientHandle;

		// Token: 0x040002F1 RID: 753
		private bool m_active = true;

		// Token: 0x040002F2 RID: 754
		private int m_bufferTime;

		// Token: 0x040002F3 RID: 755
		private int m_maxSize;

		// Token: 0x040002F4 RID: 756
		private int m_keepAlive;
	}
}
