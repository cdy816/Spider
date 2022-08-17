using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc.Ae
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class Server : Opc.Server, IServer, Opc.IServer, IDisposable, ISerializable
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003E07 File Offset: 0x00002E07
		public Server(Factory factory, URL url) : base(factory, url)
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003E1C File Offset: 0x00002E1C
		protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			int num = (int)info.GetValue("CT", typeof(int));
			this.m_subscriptions = new Server.SubscriptionCollection();
			for (int i = 0; i < num; i++)
			{
				Subscription subscription = (Subscription)info.GetValue("SU" + i.ToString(), typeof(Subscription));
				this.m_subscriptions.Add(subscription);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003EA4 File Offset: 0x00002EA4
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("CT", this.m_subscriptions.Count);
			for (int i = 0; i < this.m_subscriptions.Count; i++)
			{
				info.AddValue("SU" + i.ToString(), this.m_subscriptions[i]);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003F08 File Offset: 0x00002F08
		public int AvailableFilters
		{
			get
			{
				return this.m_filters;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003F10 File Offset: 0x00002F10
		public Server.SubscriptionCollection Subscriptions
		{
			get
			{
				return this.m_subscriptions;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003F18 File Offset: 0x00002F18
		public override void Connect(URL url, ConnectData connectData)
		{
			base.Connect(url, connectData);
			if (this.m_subscriptions.Count == 0)
			{
				return;
			}
			Server.SubscriptionCollection subscriptionCollection = new Server.SubscriptionCollection();
			foreach (object obj in this.m_subscriptions)
			{
				Subscription template = (Subscription)obj;
				try
				{
					subscriptionCollection.Add(this.EstablishSubscription(template));
				}
				catch
				{
				}
			}
			this.m_subscriptions = subscriptionCollection;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003FAC File Offset: 0x00002FAC
		public override void Disconnect()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_disposing = true;
			foreach (object obj in this.m_subscriptions)
			{
				Subscription subscription = (Subscription)obj;
				subscription.Dispose();
			}
			this.m_disposing = false;
			base.Disconnect();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004028 File Offset: 0x00003028
		public ServerStatus GetStatus()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			ServerStatus status = ((IServer)this.m_server).GetStatus();
			if (status.StatusInfo == null)
			{
				status.StatusInfo = base.GetString("serverState." + status.ServerState.ToString());
			}
			return status;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004084 File Offset: 0x00003084
		public ISubscription CreateSubscription(SubscriptionState state)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			ISubscription subscription = ((IServer)this.m_server).CreateSubscription(state);
			if (subscription != null)
			{
				Subscription subscription2 = new Subscription(this, subscription, state);
				this.m_subscriptions.Add(subscription2);
				return subscription2;
			}
			return null;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000040CC File Offset: 0x000030CC
		public int QueryAvailableFilters()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_filters = ((IServer)this.m_server).QueryAvailableFilters();
			return this.m_filters;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000040F8 File Offset: 0x000030F8
		public Category[] QueryEventCategories(int eventType)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).QueryEventCategories(eventType);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004128 File Offset: 0x00003128
		public string[] QueryConditionNames(int eventCategory)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).QueryConditionNames(eventCategory);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004158 File Offset: 0x00003158
		public string[] QuerySubConditionNames(string conditionName)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).QuerySubConditionNames(conditionName);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004188 File Offset: 0x00003188
		public string[] QueryConditionNames(string sourceName)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).QueryConditionNames(sourceName);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000041B8 File Offset: 0x000031B8
		public Attribute[] QueryEventAttributes(int eventCategory)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).QueryEventAttributes(eventCategory);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000041E8 File Offset: 0x000031E8
		public ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).TranslateToItemIDs(sourceName, eventCategory, conditionName, subConditionName, attributeIDs);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000421C File Offset: 0x0000321C
		public Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).GetConditionState(sourceName, conditionName, attributeIDs);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000424C File Offset: 0x0000324C
		public ResultID[] EnableConditionByArea(string[] areas)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).EnableConditionByArea(areas);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000427C File Offset: 0x0000327C
		public ResultID[] DisableConditionByArea(string[] areas)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).DisableConditionByArea(areas);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000042AC File Offset: 0x000032AC
		public ResultID[] EnableConditionBySource(string[] sources)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).EnableConditionBySource(sources);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000042DC File Offset: 0x000032DC
		public ResultID[] DisableConditionBySource(string[] sources)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).DisableConditionBySource(sources);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000430C File Offset: 0x0000330C
		public EnabledStateResult[] GetEnableStateByArea(string[] areas)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).GetEnableStateByArea(areas);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000433C File Offset: 0x0000333C
		public EnabledStateResult[] GetEnableStateBySource(string[] sources)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).GetEnableStateBySource(sources);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000436A File Offset: 0x0000336A
		public ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).AcknowledgeCondition(acknowledgerID, comment, conditions);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000438D File Offset: 0x0000338D
		public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).Browse(areaID, browseType, browseFilter);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000043B0 File Offset: 0x000033B0
		public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).Browse(areaID, browseType, browseFilter, maxElements, out position);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000043D7 File Offset: 0x000033D7
		public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).BrowseNext(maxElements, ref position);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000043F9 File Offset: 0x000033F9
		internal void SubscriptionDisposed(Subscription subscription)
		{
			if (!this.m_disposing)
			{
				this.m_subscriptions.Remove(subscription);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004410 File Offset: 0x00003410
		private Subscription EstablishSubscription(Subscription template)
		{
			ISubscription subscription = null;
			try
			{
				subscription = ((IServer)this.m_server).CreateSubscription(template.State);
				if (subscription == null)
				{
					return null;
				}
				Subscription subscription2 = new Subscription(this, subscription, template.State);
				subscription2.SetFilters(template.Filters);
				IDictionaryEnumerator enumerator = template.Attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					subscription2.SelectReturnedAttributes((int)enumerator.Key, ((Subscription.AttributeCollection)enumerator.Value).ToArray());
				}
				return subscription2;
			}
			catch
			{
				if (subscription != null)
				{
					subscription.Dispose();
					subscription = null;
				}
			}
			return null;
		}

		// Token: 0x04000015 RID: 21
		private int m_filters;

		// Token: 0x04000016 RID: 22
		private bool m_disposing;

		// Token: 0x04000017 RID: 23
		private Server.SubscriptionCollection m_subscriptions = new Server.SubscriptionCollection();

		// Token: 0x0200000E RID: 14
		private class Names
		{
			// Token: 0x04000018 RID: 24
			internal const string COUNT = "CT";

			// Token: 0x04000019 RID: 25
			internal const string SUBSCRIPTION = "SU";
		}

		// Token: 0x02000011 RID: 17
		public class SubscriptionCollection : ReadOnlyCollection
		{
			// Token: 0x17000012 RID: 18
			public Subscription this[int index]
			{
				get
				{
					return (Subscription)this.Array.GetValue(index);
				}
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00004638 File Offset: 0x00003638
			public new Subscription[] ToArray()
			{
				return (Subscription[])this.Array;
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00004648 File Offset: 0x00003648
			internal void Add(Subscription subscription)
			{
				Subscription[] array = new Subscription[this.Count + 1];
				this.Array.CopyTo(array, 0);
				array[this.Count] = subscription;
				this.Array = array;
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00004680 File Offset: 0x00003680
			internal void Remove(Subscription subscription)
			{
				Subscription[] array = new Subscription[this.Count - 1];
				int num = 0;
				for (int i = 0; i < this.Array.Length; i++)
				{
					Subscription subscription2 = (Subscription)this.Array.GetValue(i);
					if (subscription != subscription2)
					{
						array[num++] = subscription2;
					}
				}
				this.Array = array;
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x000046D8 File Offset: 0x000036D8
			internal SubscriptionCollection() : base(new Subscription[0])
			{
			}
		}
	}
}
