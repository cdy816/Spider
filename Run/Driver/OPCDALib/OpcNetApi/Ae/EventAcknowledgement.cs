using System;

namespace Opc.Ae
{
	// Token: 0x020000CF RID: 207
	[Serializable]
	public class EventAcknowledgement : ICloneable
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001163C File Offset: 0x0001063C
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00011644 File Offset: 0x00010644
		public string SourceName
		{
			get
			{
				return this.m_sourceName;
			}
			set
			{
				this.m_sourceName = value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001164D File Offset: 0x0001064D
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00011655 File Offset: 0x00010655
		public string ConditionName
		{
			get
			{
				return this.m_conditionName;
			}
			set
			{
				this.m_conditionName = value;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001165E File Offset: 0x0001065E
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00011666 File Offset: 0x00010666
		public DateTime ActiveTime
		{
			get
			{
				return this.m_activeTime;
			}
			set
			{
				this.m_activeTime = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001166F File Offset: 0x0001066F
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x00011677 File Offset: 0x00010677
		public int Cookie
		{
			get
			{
				return this.m_cookie;
			}
			set
			{
				this.m_cookie = value;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00011680 File Offset: 0x00010680
		public EventAcknowledgement()
		{
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00011694 File Offset: 0x00010694
		public EventAcknowledgement(EventNotification notification)
		{
			this.m_sourceName = notification.SourceID;
			this.m_conditionName = notification.ConditionName;
			this.m_activeTime = notification.ActiveTime;
			this.m_cookie = notification.Cookie;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000116E2 File Offset: 0x000106E2
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0400032A RID: 810
		private string m_sourceName;

		// Token: 0x0400032B RID: 811
		private string m_conditionName;

		// Token: 0x0400032C RID: 812
		private DateTime m_activeTime = DateTime.MinValue;

		// Token: 0x0400032D RID: 813
		private int m_cookie;
	}
}
