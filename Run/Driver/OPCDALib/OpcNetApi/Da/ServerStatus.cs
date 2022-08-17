using System;

namespace Opc.Da
{
	// Token: 0x020000C0 RID: 192
	[Serializable]
	public class ServerStatus : ICloneable
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00010F7D File Offset: 0x0000FF7D
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x00010F85 File Offset: 0x0000FF85
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

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x00010F8E File Offset: 0x0000FF8E
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x00010F96 File Offset: 0x0000FF96
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00010F9F File Offset: 0x0000FF9F
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x00010FA7 File Offset: 0x0000FFA7
		public serverState ServerState
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00010FB0 File Offset: 0x0000FFB0
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x00010FB8 File Offset: 0x0000FFB8
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00010FC1 File Offset: 0x0000FFC1
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x00010FC9 File Offset: 0x0000FFC9
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00010FD2 File Offset: 0x0000FFD2
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x00010FDA File Offset: 0x0000FFDA
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00010FE3 File Offset: 0x0000FFE3
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x00010FEB File Offset: 0x0000FFEB
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

		// Token: 0x06000694 RID: 1684 RVA: 0x00010FF4 File Offset: 0x0000FFF4
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040002D4 RID: 724
		private string m_vendorInfo;

		// Token: 0x040002D5 RID: 725
		private string m_productVersion;

		// Token: 0x040002D6 RID: 726
		private serverState m_serverState;

		// Token: 0x040002D7 RID: 727
		private string m_statusInfo;

		// Token: 0x040002D8 RID: 728
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x040002D9 RID: 729
		private DateTime m_currentTime = DateTime.MinValue;

		// Token: 0x040002DA RID: 730
		private DateTime m_lastUpdateTime = DateTime.MinValue;
	}
}
