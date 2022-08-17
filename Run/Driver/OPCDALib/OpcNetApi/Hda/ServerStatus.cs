using System;

namespace Opc.Hda
{
	// Token: 0x020000B1 RID: 177
	[Serializable]
	public class ServerStatus : ICloneable
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x000106E2 File Offset: 0x0000F6E2
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x000106EA File Offset: 0x0000F6EA
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x000106F3 File Offset: 0x0000F6F3
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x000106FB File Offset: 0x0000F6FB
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00010704 File Offset: 0x0000F704
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0001070C File Offset: 0x0000F70C
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00010715 File Offset: 0x0000F715
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0001071D File Offset: 0x0000F71D
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00010726 File Offset: 0x0000F726
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0001072E File Offset: 0x0000F72E
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00010737 File Offset: 0x0000F737
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x0001073F File Offset: 0x0000F73F
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00010748 File Offset: 0x0000F748
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00010750 File Offset: 0x0000F750
		public int MaxReturnValues
		{
			get
			{
				return this.m_maxReturnValues;
			}
			set
			{
				this.m_maxReturnValues = value;
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00010759 File Offset: 0x0000F759
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040002A7 RID: 679
		private string m_vendorInfo;

		// Token: 0x040002A8 RID: 680
		private string m_productVersion;

		// Token: 0x040002A9 RID: 681
		private DateTime m_currentTime = DateTime.MinValue;

		// Token: 0x040002AA RID: 682
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x040002AB RID: 683
		private ServerState m_serverState = ServerState.Indeterminate;

		// Token: 0x040002AC RID: 684
		private string m_statusInfo;

		// Token: 0x040002AD RID: 685
		private int m_maxReturnValues;
	}
}
