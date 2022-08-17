using System;
using System.Runtime.Serialization;

namespace Opc.Da
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class Server : Opc.Server, IServer, Opc.IServer, IDisposable
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003048 File Offset: 0x00002048
		public Server(Factory factory, URL url) : base(factory, url)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003068 File Offset: 0x00002068
		protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.m_filters = (int)info.GetValue("Filters", typeof(int));
			Subscription[] array = (Subscription[])info.GetValue("Subscription", typeof(Subscription[]));
			if (array != null)
			{
				foreach (Subscription value in array)
				{
					this.m_subscriptions.Add(value);
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000030F0 File Offset: 0x000020F0
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Filters", this.m_filters);
			Subscription[] array = null;
			if (this.m_subscriptions.Count > 0)
			{
				array = new Subscription[this.m_subscriptions.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.m_subscriptions[i];
				}
			}
			info.AddValue("Subscription", array);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003160 File Offset: 0x00002160
		public override object Clone()
		{
			Server server = (Server)base.Clone();
			if (server.m_subscriptions != null)
			{
				SubscriptionCollection subscriptionCollection = new SubscriptionCollection();
				foreach (object obj in server.m_subscriptions)
				{
					Subscription subscription = (Subscription)obj;
					subscriptionCollection.Add(subscription.Clone());
				}
				server.m_subscriptions = subscriptionCollection;
			}
			return server;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000031E4 File Offset: 0x000021E4
		public SubscriptionCollection Subscriptions
		{
			get
			{
				return this.m_subscriptions;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000031EC File Offset: 0x000021EC
		public int Filters
		{
			get
			{
				return this.m_filters;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000031F4 File Offset: 0x000021F4
		public override void Connect(URL url, ConnectData connectData)
		{
			base.Connect(url, connectData);
			if (this.m_subscriptions == null)
			{
				return;
			}
			SubscriptionCollection subscriptionCollection = new SubscriptionCollection();
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003284 File Offset: 0x00002284
		public override void Disconnect()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			if (this.m_subscriptions != null)
			{
				foreach (object obj in this.m_subscriptions)
				{
					Subscription subscription = (Subscription)obj;
					subscription.Dispose();
				}
				this.m_subscriptions = null;
			}
			base.Disconnect();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003300 File Offset: 0x00002300
		public int GetResultFilters()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_filters = ((IServer)this.m_server).GetResultFilters();
			return this.m_filters;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000332C File Offset: 0x0000232C
		public void SetResultFilters(int filters)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			((IServer)this.m_server).SetResultFilters(filters);
			this.m_filters = filters;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003354 File Offset: 0x00002354
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

		// Token: 0x06000044 RID: 68 RVA: 0x000033AF File Offset: 0x000023AF
		public ItemValueResult[] Read(Item[] items)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).Read(items);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000033D0 File Offset: 0x000023D0
		public IdentifiedResult[] Write(ItemValue[] items)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).Write(items);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000033F4 File Offset: 0x000023F4
		public virtual ISubscription CreateSubscription(SubscriptionState state)
		{
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			ISubscription subscription = ((IServer)this.m_server).CreateSubscription(state);
			subscription.SetResultFilters(this.m_filters);
			SubscriptionCollection subscriptionCollection = new SubscriptionCollection();
			if (this.m_subscriptions != null)
			{
				foreach (object obj in this.m_subscriptions)
				{
					Subscription value = (Subscription)obj;
					subscriptionCollection.Add(value);
				}
			}
			subscriptionCollection.Add(this.CreateSubscription(subscription));
			this.m_subscriptions = subscriptionCollection;
			return this.m_subscriptions[this.m_subscriptions.Count - 1];
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000034C8 File Offset: 0x000024C8
		protected virtual Subscription CreateSubscription(ISubscription subscription)
		{
			return new Subscription(this, subscription);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000034D4 File Offset: 0x000024D4
		public virtual void CancelSubscription(ISubscription subscription)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException("subscription");
			}
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			if (!typeof(Subscription).IsInstanceOfType(subscription))
			{
				throw new ArgumentException("Incorrect object type.", "subscription");
			}
			if (!this.Equals(((Subscription)subscription).Server))
			{
				throw new ArgumentException("Unknown subscription.", "subscription");
			}
			SubscriptionCollection subscriptionCollection = new SubscriptionCollection();
			foreach (object obj in this.m_subscriptions)
			{
				Subscription subscription2 = (Subscription)obj;
				if (!subscription.Equals(subscription2))
				{
					subscriptionCollection.Add(subscription2);
				}
			}
			if (subscriptionCollection.Count == this.m_subscriptions.Count)
			{
				throw new ArgumentException("Subscription not found.", "subscription");
			}
			this.m_subscriptions = subscriptionCollection;
			((IServer)this.m_server).CancelSubscription(((Subscription)subscription).m_subscription);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000035E4 File Offset: 0x000025E4
		public BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out BrowsePosition position)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).Browse(itemID, filters, out position);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003607 File Offset: 0x00002607
		public BrowseElement[] BrowseNext(ref BrowsePosition position)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).BrowseNext(ref position);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003628 File Offset: 0x00002628
		public ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return ((IServer)this.m_server).GetProperties(itemIDs, propertyIDs, returnValues);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000364C File Offset: 0x0000264C
		private Subscription EstablishSubscription(Subscription template)
		{
			Subscription subscription = new Subscription(this, ((IServer)this.m_server).CreateSubscription(template.State));
			subscription.SetResultFilters(template.Filters);
			try
			{
				subscription.AddItems(template.Items);
			}
			catch
			{
				subscription.Dispose();
				subscription = null;
			}
			return subscription;
		}

		// Token: 0x0400000D RID: 13
		private SubscriptionCollection m_subscriptions = new SubscriptionCollection();

		// Token: 0x0400000E RID: 14
		private int m_filters = 9;

		// Token: 0x02000008 RID: 8
		private class Names
		{
			// Token: 0x0400000F RID: 15
			internal const string FILTERS = "Filters";

			// Token: 0x04000010 RID: 16
			internal const string SUBSCRIPTIONS = "Subscription";
		}
	}
}
