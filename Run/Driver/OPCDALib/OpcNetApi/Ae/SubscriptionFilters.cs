using System;
using System.Runtime.Serialization;

namespace Opc.Ae
{
	// Token: 0x020000C6 RID: 198
	[Serializable]
	public class SubscriptionFilters : ICloneable, ISerializable
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000110A2 File Offset: 0x000100A2
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x000110AA File Offset: 0x000100AA
		public int EventTypes
		{
			get
			{
				return this.m_eventTypes;
			}
			set
			{
				this.m_eventTypes = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x000110B3 File Offset: 0x000100B3
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x000110BB File Offset: 0x000100BB
		public int HighSeverity
		{
			get
			{
				return this.m_highSeverity;
			}
			set
			{
				this.m_highSeverity = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x000110C4 File Offset: 0x000100C4
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x000110CC File Offset: 0x000100CC
		public int LowSeverity
		{
			get
			{
				return this.m_lowSeverity;
			}
			set
			{
				this.m_lowSeverity = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x000110D5 File Offset: 0x000100D5
		public SubscriptionFilters.CategoryCollection Categories
		{
			get
			{
				return this.m_categories;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x000110DD File Offset: 0x000100DD
		public SubscriptionFilters.StringCollection Areas
		{
			get
			{
				return this.m_areas;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000110E5 File Offset: 0x000100E5
		public SubscriptionFilters.StringCollection Sources
		{
			get
			{
				return this.m_sources;
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000110F0 File Offset: 0x000100F0
		public SubscriptionFilters()
		{
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00011144 File Offset: 0x00010144
		protected SubscriptionFilters(SerializationInfo info, StreamingContext context)
		{
			this.m_eventTypes = (int)info.GetValue("ET", typeof(int));
			this.m_categories = (SubscriptionFilters.CategoryCollection)info.GetValue("CT", typeof(SubscriptionFilters.CategoryCollection));
			this.m_highSeverity = (int)info.GetValue("HS", typeof(int));
			this.m_lowSeverity = (int)info.GetValue("LS", typeof(int));
			this.m_areas = (SubscriptionFilters.StringCollection)info.GetValue("AR", typeof(SubscriptionFilters.StringCollection));
			this.m_sources = (SubscriptionFilters.StringCollection)info.GetValue("SR", typeof(SubscriptionFilters.StringCollection));
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00011258 File Offset: 0x00010258
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ET", this.m_eventTypes);
			info.AddValue("CT", this.m_categories);
			info.AddValue("HS", this.m_highSeverity);
			info.AddValue("LS", this.m_lowSeverity);
			info.AddValue("AR", this.m_areas);
			info.AddValue("SR", this.m_sources);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000112CC File Offset: 0x000102CC
		public virtual object Clone()
		{
			SubscriptionFilters subscriptionFilters = (SubscriptionFilters)base.MemberwiseClone();
			subscriptionFilters.m_categories = (SubscriptionFilters.CategoryCollection)this.m_categories.Clone();
			subscriptionFilters.m_areas = (SubscriptionFilters.StringCollection)this.m_areas.Clone();
			subscriptionFilters.m_sources = (SubscriptionFilters.StringCollection)this.m_sources.Clone();
			return subscriptionFilters;
		}

		// Token: 0x040002F5 RID: 757
		private int m_eventTypes = 65535;

		// Token: 0x040002F6 RID: 758
		private SubscriptionFilters.CategoryCollection m_categories = new SubscriptionFilters.CategoryCollection();

		// Token: 0x040002F7 RID: 759
		private int m_highSeverity = 1000;

		// Token: 0x040002F8 RID: 760
		private int m_lowSeverity = 1;

		// Token: 0x040002F9 RID: 761
		private SubscriptionFilters.StringCollection m_areas = new SubscriptionFilters.StringCollection();

		// Token: 0x040002FA RID: 762
		private SubscriptionFilters.StringCollection m_sources = new SubscriptionFilters.StringCollection();

		// Token: 0x020000C7 RID: 199
		[Serializable]
		public class CategoryCollection : WriteableCollection
		{
			// Token: 0x17000187 RID: 391
			public int this[int index]
			{
				get
				{
					return (int)this.Array[index];
				}
			}

			// Token: 0x060006B6 RID: 1718 RVA: 0x0001133B File Offset: 0x0001033B
			public new int[] ToArray()
			{
				return (int[])this.Array.ToArray(typeof(int));
			}

			// Token: 0x060006B7 RID: 1719 RVA: 0x00011357 File Offset: 0x00010357
			internal CategoryCollection() : base(null, typeof(int))
			{
			}

			// Token: 0x060006B8 RID: 1720 RVA: 0x0001136A File Offset: 0x0001036A
			protected CategoryCollection(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		// Token: 0x020000C8 RID: 200
		[Serializable]
		public class StringCollection : WriteableCollection
		{
			// Token: 0x17000188 RID: 392
			public string this[int index]
			{
				get
				{
					return (string)this.Array[index];
				}
			}

			// Token: 0x060006BA RID: 1722 RVA: 0x00011387 File Offset: 0x00010387
			public new string[] ToArray()
			{
				return (string[])this.Array.ToArray(typeof(string));
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x000113A3 File Offset: 0x000103A3
			internal StringCollection() : base(null, typeof(string))
			{
			}

			// Token: 0x060006BC RID: 1724 RVA: 0x000113B6 File Offset: 0x000103B6
			protected StringCollection(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		// Token: 0x020000C9 RID: 201
		private class Names
		{
			// Token: 0x040002FB RID: 763
			internal const string EVENT_TYPES = "ET";

			// Token: 0x040002FC RID: 764
			internal const string CATEGORIES = "CT";

			// Token: 0x040002FD RID: 765
			internal const string HIGH_SEVERITY = "HS";

			// Token: 0x040002FE RID: 766
			internal const string LOW_SEVERITY = "LS";

			// Token: 0x040002FF RID: 767
			internal const string AREAS = "AR";

			// Token: 0x04000300 RID: 768
			internal const string SOURCES = "SR";
		}
	}
}
