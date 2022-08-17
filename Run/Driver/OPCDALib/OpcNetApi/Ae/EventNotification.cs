using System;
using Opc.Da;

namespace Opc.Ae
{
	// Token: 0x020000CB RID: 203
	[Serializable]
	public class EventNotification : ICloneable
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000113C8 File Offset: 0x000103C8
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x000113D0 File Offset: 0x000103D0
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000113D9 File Offset: 0x000103D9
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x000113E1 File Offset: 0x000103E1
		public string SourceID
		{
			get
			{
				return this.m_sourceID;
			}
			set
			{
				this.m_sourceID = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x000113EA File Offset: 0x000103EA
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x000113F2 File Offset: 0x000103F2
		public DateTime Time
		{
			get
			{
				return this.m_time;
			}
			set
			{
				this.m_time = value;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x000113FB File Offset: 0x000103FB
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00011403 File Offset: 0x00010403
		public string Message
		{
			get
			{
				return this.m_message;
			}
			set
			{
				this.m_message = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0001140C File Offset: 0x0001040C
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x00011414 File Offset: 0x00010414
		public EventType EventType
		{
			get
			{
				return this.m_eventType;
			}
			set
			{
				this.m_eventType = value;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0001141D File Offset: 0x0001041D
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x00011425 File Offset: 0x00010425
		public int EventCategory
		{
			get
			{
				return this.m_eventCategory;
			}
			set
			{
				this.m_eventCategory = value;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0001142E File Offset: 0x0001042E
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x00011436 File Offset: 0x00010436
		public int Severity
		{
			get
			{
				return this.m_severity;
			}
			set
			{
				this.m_severity = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0001143F File Offset: 0x0001043F
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x00011447 File Offset: 0x00010447
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

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00011450 File Offset: 0x00010450
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x00011458 File Offset: 0x00010458
		public string SubConditionName
		{
			get
			{
				return this.m_subConditionName;
			}
			set
			{
				this.m_subConditionName = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00011461 File Offset: 0x00010461
		public EventNotification.AttributeCollection Attributes
		{
			get
			{
				return this.m_attributes;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00011469 File Offset: 0x00010469
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00011471 File Offset: 0x00010471
		public int ChangeMask
		{
			get
			{
				return this.m_changeMask;
			}
			set
			{
				this.m_changeMask = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001147A File Offset: 0x0001047A
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00011482 File Offset: 0x00010482
		public int NewState
		{
			get
			{
				return this.m_newState;
			}
			set
			{
				this.m_newState = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001148B File Offset: 0x0001048B
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00011493 File Offset: 0x00010493
		public Quality Quality
		{
			get
			{
				return this.m_quality;
			}
			set
			{
				this.m_quality = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001149C File Offset: 0x0001049C
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x000114A4 File Offset: 0x000104A4
		public bool AckRequired
		{
			get
			{
				return this.m_ackRequired;
			}
			set
			{
				this.m_ackRequired = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x000114AD File Offset: 0x000104AD
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x000114B5 File Offset: 0x000104B5
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x000114BE File Offset: 0x000104BE
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x000114C6 File Offset: 0x000104C6
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x000114CF File Offset: 0x000104CF
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x000114D7 File Offset: 0x000104D7
		public string ActorID
		{
			get
			{
				return this.m_actorID;
			}
			set
			{
				this.m_actorID = value;
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000114E0 File Offset: 0x000104E0
		public void SetAttributes(object[] attributes)
		{
			if (attributes == null)
			{
				this.m_attributes = new EventNotification.AttributeCollection();
				return;
			}
			this.m_attributes = new EventNotification.AttributeCollection(attributes);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00011500 File Offset: 0x00010500
		public virtual object Clone()
		{
			EventNotification eventNotification = (EventNotification)base.MemberwiseClone();
			eventNotification.m_attributes = (EventNotification.AttributeCollection)this.m_attributes.Clone();
			return eventNotification;
		}

		// Token: 0x0400030A RID: 778
		private object m_clientHandle;

		// Token: 0x0400030B RID: 779
		private string m_sourceID;

		// Token: 0x0400030C RID: 780
		private DateTime m_time = DateTime.MinValue;

		// Token: 0x0400030D RID: 781
		private string m_message;

		// Token: 0x0400030E RID: 782
		private EventType m_eventType = EventType.Condition;

		// Token: 0x0400030F RID: 783
		private int m_eventCategory;

		// Token: 0x04000310 RID: 784
		private int m_severity = 1;

		// Token: 0x04000311 RID: 785
		private string m_conditionName;

		// Token: 0x04000312 RID: 786
		private string m_subConditionName;

		// Token: 0x04000313 RID: 787
		private EventNotification.AttributeCollection m_attributes = new EventNotification.AttributeCollection();

		// Token: 0x04000314 RID: 788
		private int m_changeMask;

		// Token: 0x04000315 RID: 789
		private int m_newState;

		// Token: 0x04000316 RID: 790
		private Quality m_quality = Quality.Bad;

		// Token: 0x04000317 RID: 791
		private bool m_ackRequired;

		// Token: 0x04000318 RID: 792
		private DateTime m_activeTime = DateTime.MinValue;

		// Token: 0x04000319 RID: 793
		private int m_cookie;

		// Token: 0x0400031A RID: 794
		private string m_actorID;

		// Token: 0x020000CC RID: 204
		[Serializable]
		public class AttributeCollection : ReadOnlyCollection
		{
			// Token: 0x060006E2 RID: 1762 RVA: 0x0001157D File Offset: 0x0001057D
			internal AttributeCollection() : base(new object[0])
			{
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x0001158B File Offset: 0x0001058B
			internal AttributeCollection(object[] attributes) : base(attributes)
			{
			}
		}
	}
}
