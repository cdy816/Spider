using System;

namespace Opc.Da
{
	// Token: 0x020000E5 RID: 229
	[Serializable]
	public class SubscriptionState : ICloneable
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00012A0A File Offset: 0x00011A0A
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x00012A12 File Offset: 0x00011A12
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

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00012A1B File Offset: 0x00011A1B
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x00012A23 File Offset: 0x00011A23
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00012A2C File Offset: 0x00011A2C
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x00012A34 File Offset: 0x00011A34
		public object ServerHandle
		{
			get
			{
				return this.m_serverHandle;
			}
			set
			{
				this.m_serverHandle = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00012A3D File Offset: 0x00011A3D
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00012A45 File Offset: 0x00011A45
		public string Locale
		{
			get
			{
				return this.m_locale;
			}
			set
			{
				this.m_locale = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00012A4E File Offset: 0x00011A4E
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x00012A56 File Offset: 0x00011A56
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

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x00012A5F File Offset: 0x00011A5F
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00012A67 File Offset: 0x00011A67
		public int UpdateRate
		{
			get
			{
				return this.m_updateRate;
			}
			set
			{
				this.m_updateRate = value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00012A70 File Offset: 0x00011A70
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00012A78 File Offset: 0x00011A78
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

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00012A81 File Offset: 0x00011A81
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x00012A89 File Offset: 0x00011A89
		public float Deadband
		{
			get
			{
				return this.m_deadband;
			}
			set
			{
				this.m_deadband = value;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00012AA1 File Offset: 0x00011AA1
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x04000377 RID: 887
		private string m_name;

		// Token: 0x04000378 RID: 888
		private object m_clientHandle;

		// Token: 0x04000379 RID: 889
		private object m_serverHandle;

		// Token: 0x0400037A RID: 890
		private string m_locale;

		// Token: 0x0400037B RID: 891
		private bool m_active = true;

		// Token: 0x0400037C RID: 892
		private int m_updateRate;

		// Token: 0x0400037D RID: 893
		private int m_keepAlive;

		// Token: 0x0400037E RID: 894
		private float m_deadband;
	}
}
