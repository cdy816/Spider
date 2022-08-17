using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc.Ae
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class Subscription : ISubscription, IDisposable, ISerializable, ICloneable
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x00007A28 File Offset: 0x00006A28
		public Subscription(Server server, ISubscription subscription, SubscriptionState state)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			if (subscription == null)
			{
				throw new ArgumentNullException("subscription");
			}
			this.m_server = server;
			this.m_subscription = subscription;
			this.m_state = (SubscriptionState)state.Clone();
			this.m_name = state.Name;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00007AC4 File Offset: 0x00006AC4
		~Subscription()
		{
			this.Dispose(false);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00007AF4 File Offset: 0x00006AF4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00007B03 File Offset: 0x00006B03
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (disposing && this.m_subscription != null)
				{
					this.m_server.SubscriptionDisposed(this);
					this.m_subscription.Dispose();
				}
				this.m_disposed = true;
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00007B38 File Offset: 0x00006B38
		protected Subscription(SerializationInfo info, StreamingContext context)
		{
			this.m_state = (SubscriptionState)info.GetValue("ST", typeof(SubscriptionState));
			this.m_filters = (SubscriptionFilters)info.GetValue("FT", typeof(SubscriptionFilters));
			this.m_attributes = (Subscription.AttributeDictionary)info.GetValue("AT", typeof(Subscription.AttributeDictionary));
			this.m_name = this.m_state.Name;
			this.m_categories = new Subscription.CategoryCollection(this.m_filters.Categories.ToArray());
			this.m_areas = new Subscription.StringCollection(this.m_filters.Areas.ToArray());
			this.m_sources = new Subscription.StringCollection(this.m_filters.Sources.ToArray());
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00007C4F File Offset: 0x00006C4F
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ST", this.m_state);
			info.AddValue("FT", this.m_filters);
			info.AddValue("AT", this.m_attributes);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00007C84 File Offset: 0x00006C84
		public virtual object Clone()
		{
			return (Subscription)base.MemberwiseClone();
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00007C9E File Offset: 0x00006C9E
		public Server Server
		{
			get
			{
				return this.m_server;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00007CA6 File Offset: 0x00006CA6
		public string Name
		{
			get
			{
				return this.m_state.Name;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00007CB3 File Offset: 0x00006CB3
		public object ClientHandle
		{
			get
			{
				return this.m_state.ClientHandle;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00007CC0 File Offset: 0x00006CC0
		public bool Active
		{
			get
			{
				return this.m_state.Active;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00007CCD File Offset: 0x00006CCD
		public int BufferTime
		{
			get
			{
				return this.m_state.BufferTime;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00007CDA File Offset: 0x00006CDA
		public int MaxSize
		{
			get
			{
				return this.m_state.MaxSize;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00007CE7 File Offset: 0x00006CE7
		public int KeepAlive
		{
			get
			{
				return this.m_state.KeepAlive;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00007CF4 File Offset: 0x00006CF4
		public int EventTypes
		{
			get
			{
				return this.m_filters.EventTypes;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00007D01 File Offset: 0x00006D01
		public int HighSeverity
		{
			get
			{
				return this.m_filters.HighSeverity;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00007D0E File Offset: 0x00006D0E
		public int LowSeverity
		{
			get
			{
				return this.m_filters.LowSeverity;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00007D1B File Offset: 0x00006D1B
		public Subscription.CategoryCollection Categories
		{
			get
			{
				return this.m_categories;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00007D23 File Offset: 0x00006D23
		public Subscription.StringCollection Areas
		{
			get
			{
				return this.m_areas;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00007D2B File Offset: 0x00006D2B
		public Subscription.StringCollection Sources
		{
			get
			{
				return this.m_sources;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00007D33 File Offset: 0x00006D33
		public Subscription.AttributeDictionary Attributes
		{
			get
			{
				return this.m_attributes;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00007D3C File Offset: 0x00006D3C
		public Opc.Ae.AttributeDictionary GetAttributes()
		{
			Opc.Ae.AttributeDictionary attributeDictionary = new Opc.Ae.AttributeDictionary();
			IDictionaryEnumerator enumerator = this.m_attributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				int key = (int)enumerator.Key;
				Subscription.AttributeCollection attributeCollection = (Subscription.AttributeCollection)enumerator.Value;
				attributeDictionary.Add(key, attributeCollection.ToArray());
			}
			return attributeDictionary;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002E6 RID: 742 RVA: 0x00007D8B File Offset: 0x00006D8B
		// (remove) Token: 0x060002E7 RID: 743 RVA: 0x00007D99 File Offset: 0x00006D99
		public event EventChangedEventHandler EventChanged
		{
			add
			{
				this.m_subscription.EventChanged += value;
			}
			remove
			{
				this.m_subscription.EventChanged -= value;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00007DA8 File Offset: 0x00006DA8
		public SubscriptionState GetState()
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_state = this.m_subscription.GetState();
			this.m_state.Name = this.m_name;
			return (SubscriptionState)this.m_state.Clone();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00007DF8 File Offset: 0x00006DF8
		public SubscriptionState ModifyState(int masks, SubscriptionState state)
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_state = this.m_subscription.ModifyState(masks, state);
			if ((masks & 1) != 0)
			{
				this.m_state.Name = (this.m_name = state.Name);
			}
			else
			{
				this.m_state.Name = this.m_name;
			}
			return (SubscriptionState)this.m_state.Clone();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00007E68 File Offset: 0x00006E68
		public SubscriptionFilters GetFilters()
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_filters = this.m_subscription.GetFilters();
			this.m_categories = new Subscription.CategoryCollection(this.m_filters.Categories.ToArray());
			this.m_areas = new Subscription.StringCollection(this.m_filters.Areas.ToArray());
			this.m_sources = new Subscription.StringCollection(this.m_filters.Sources.ToArray());
			return (SubscriptionFilters)this.m_filters.Clone();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00007EF5 File Offset: 0x00006EF5
		public void SetFilters(SubscriptionFilters filters)
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_subscription.SetFilters(filters);
			this.GetFilters();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00007F18 File Offset: 0x00006F18
		public int[] GetReturnedAttributes(int eventCategory)
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			int[] returnedAttributes = this.m_subscription.GetReturnedAttributes(eventCategory);
			this.m_attributes.Update(eventCategory, (int[])Convert.Clone(returnedAttributes));
			return returnedAttributes;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00007F58 File Offset: 0x00006F58
		public void SelectReturnedAttributes(int eventCategory, int[] attributeIDs)
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_subscription.SelectReturnedAttributes(eventCategory, attributeIDs);
			this.m_attributes.Update(eventCategory, (int[])Convert.Clone(attributeIDs));
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00007F8C File Offset: 0x00006F8C
		public void Refresh()
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_subscription.Refresh();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00007FA7 File Offset: 0x00006FA7
		public void CancelRefresh()
		{
			if (this.m_subscription == null)
			{
				throw new NotConnectedException();
			}
			this.m_subscription.CancelRefresh();
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00007FC2 File Offset: 0x00006FC2
		internal SubscriptionState State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00007FCA File Offset: 0x00006FCA
		internal SubscriptionFilters Filters
		{
			get
			{
				return this.m_filters;
			}
		}

		// Token: 0x04000156 RID: 342
		private bool m_disposed;

		// Token: 0x04000157 RID: 343
		private Server m_server;

		// Token: 0x04000158 RID: 344
		private ISubscription m_subscription;

		// Token: 0x04000159 RID: 345
		private SubscriptionState m_state = new SubscriptionState();

		// Token: 0x0400015A RID: 346
		private string m_name;

		// Token: 0x0400015B RID: 347
		private SubscriptionFilters m_filters = new SubscriptionFilters();

		// Token: 0x0400015C RID: 348
		private Subscription.CategoryCollection m_categories = new Subscription.CategoryCollection();

		// Token: 0x0400015D RID: 349
		private Subscription.StringCollection m_areas = new Subscription.StringCollection();

		// Token: 0x0400015E RID: 350
		private Subscription.StringCollection m_sources = new Subscription.StringCollection();

		// Token: 0x0400015F RID: 351
		private Subscription.AttributeDictionary m_attributes = new Subscription.AttributeDictionary();

		// Token: 0x02000073 RID: 115
		private class Names
		{
			// Token: 0x04000160 RID: 352
			internal const string STATE = "ST";

			// Token: 0x04000161 RID: 353
			internal const string FILTERS = "FT";

			// Token: 0x04000162 RID: 354
			internal const string ATTRIBUTES = "AT";
		}

		// Token: 0x02000074 RID: 116
		public class CategoryCollection : ReadOnlyCollection
		{
			// Token: 0x17000096 RID: 150
			public int this[int index]
			{
				get
				{
					return (int)this.Array.GetValue(index);
				}
			}

			// Token: 0x060002F4 RID: 756 RVA: 0x00007FED File Offset: 0x00006FED
			public new int[] ToArray()
			{
				return (int[])Convert.Clone(this.Array);
			}

			// Token: 0x060002F5 RID: 757 RVA: 0x00007FFF File Offset: 0x00006FFF
			internal CategoryCollection() : base(new int[0])
			{
			}

			// Token: 0x060002F6 RID: 758 RVA: 0x0000800D File Offset: 0x0000700D
			internal CategoryCollection(int[] categoryIDs) : base(categoryIDs)
			{
			}
		}

		// Token: 0x02000075 RID: 117
		public class StringCollection : ReadOnlyCollection
		{
			// Token: 0x17000097 RID: 151
			public string this[int index]
			{
				get
				{
					return (string)this.Array.GetValue(index);
				}
			}

			// Token: 0x060002F8 RID: 760 RVA: 0x00008029 File Offset: 0x00007029
			public new string[] ToArray()
			{
				return (string[])Convert.Clone(this.Array);
			}

			// Token: 0x060002F9 RID: 761 RVA: 0x0000803B File Offset: 0x0000703B
			internal StringCollection() : base(new string[0])
			{
			}

			// Token: 0x060002FA RID: 762 RVA: 0x00008049 File Offset: 0x00007049
			internal StringCollection(string[] strings) : base(strings)
			{
			}
		}

		// Token: 0x02000078 RID: 120
		[Serializable]
		public class AttributeDictionary : ReadOnlyDictionary
		{
			// Token: 0x170000A1 RID: 161
			public Subscription.AttributeCollection this[int categoryID]
			{
				get
				{
					return (Subscription.AttributeCollection)base[categoryID];
				}
			}

			// Token: 0x06000313 RID: 787 RVA: 0x000082CC File Offset: 0x000072CC
			internal AttributeDictionary() : base(null)
			{
			}

			// Token: 0x06000314 RID: 788 RVA: 0x000082D5 File Offset: 0x000072D5
			internal AttributeDictionary(Hashtable dictionary) : base(dictionary)
			{
			}

			// Token: 0x06000315 RID: 789 RVA: 0x000082DE File Offset: 0x000072DE
			internal void Update(int categoryID, int[] attributeIDs)
			{
				this.Dictionary[categoryID] = new Subscription.AttributeCollection(attributeIDs);
			}

			// Token: 0x06000316 RID: 790 RVA: 0x000082F7 File Offset: 0x000072F7
			protected AttributeDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		// Token: 0x02000079 RID: 121
		[Serializable]
		public class AttributeCollection : ReadOnlyCollection
		{
			// Token: 0x170000A2 RID: 162
			public int this[int index]
			{
				get
				{
					return (int)this.Array.GetValue(index);
				}
			}

			// Token: 0x06000318 RID: 792 RVA: 0x00008314 File Offset: 0x00007314
			public new int[] ToArray()
			{
				return (int[])Convert.Clone(this.Array);
			}

			// Token: 0x06000319 RID: 793 RVA: 0x00008326 File Offset: 0x00007326
			internal AttributeCollection() : base(new int[0])
			{
			}

			// Token: 0x0600031A RID: 794 RVA: 0x00008334 File Offset: 0x00007334
			internal AttributeCollection(int[] attributeIDs) : base(attributeIDs)
			{
			}

			// Token: 0x0600031B RID: 795 RVA: 0x0000833D File Offset: 0x0000733D
			protected AttributeCollection(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}
	}
}
