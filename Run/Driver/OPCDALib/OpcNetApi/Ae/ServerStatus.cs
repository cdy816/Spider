using System;

namespace Opc.Ae
{
	// Token: 0x020000CE RID: 206
	[Serializable]
	public class ServerStatus : ICloneable
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00011594 File Offset: 0x00010594
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001159C File Offset: 0x0001059C
		public string VendorInfo
		{
			get
			{
				return this.m_vendorInfo;
			}
			set
			{
				this.m_vendorInfo = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x000115A5 File Offset: 0x000105A5
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x000115AD File Offset: 0x000105AD
		public string ProductVersion
		{
			get
			{
				return this.m_productVersion;
			}
			set
			{
				this.m_productVersion = value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x000115B6 File Offset: 0x000105B6
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x000115BE File Offset: 0x000105BE
		public ServerState ServerState
		{
			get
			{
				return this.m_serverState;
			}
			set
			{
				this.m_serverState = value;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x000115C7 File Offset: 0x000105C7
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x000115CF File Offset: 0x000105CF
		public string StatusInfo
		{
			get
			{
				return this.m_statusInfo;
			}
			set
			{
				this.m_statusInfo = value;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000115D8 File Offset: 0x000105D8
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x000115E0 File Offset: 0x000105E0
		public DateTime StartTime
		{
			get
			{
				return this.m_startTime;
			}
			set
			{
				this.m_startTime = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000115E9 File Offset: 0x000105E9
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x000115F1 File Offset: 0x000105F1
		public DateTime CurrentTime
		{
			get
			{
				return this.m_currentTime;
			}
			set
			{
				this.m_currentTime = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000115FA File Offset: 0x000105FA
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x00011602 File Offset: 0x00010602
		public DateTime LastUpdateTime
		{
			get
			{
				return this.m_lastUpdateTime;
			}
			set
			{
				this.m_lastUpdateTime = value;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001160B File Offset: 0x0001060B
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x04000323 RID: 803
		private string m_vendorInfo;

		// Token: 0x04000324 RID: 804
		private string m_productVersion;

		// Token: 0x04000325 RID: 805
		private ServerState m_serverState;

		// Token: 0x04000326 RID: 806
		private string m_statusInfo;

		// Token: 0x04000327 RID: 807
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x04000328 RID: 808
		private DateTime m_currentTime = DateTime.MinValue;

		// Token: 0x04000329 RID: 809
		private DateTime m_lastUpdateTime = DateTime.MinValue;
	}
}
