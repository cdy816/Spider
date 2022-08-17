using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc.Da
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	public class Subscription : ISubscription, IDisposable, ISerializable, ICloneable
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0000E0A8 File Offset: 0x0000D0A8
		public Subscription(Server server, ISubscription subscription)
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
			this.GetResultFilters();
			this.GetState();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000E10D File Offset: 0x0000D10D
		public void Dispose()
		{
			if (this.m_subscription != null)
			{
				this.m_subscription.Dispose();
				this.m_server = null;
				this.m_subscription = null;
				this.m_items = null;
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000E138 File Offset: 0x0000D138
		protected Subscription(SerializationInfo info, StreamingContext context)
		{
			this.m_state = (SubscriptionState)info.GetValue("State", typeof(SubscriptionState));
			this.m_filters = (int)info.GetValue("Filters", typeof(int));
			this.m_items = (Item[])info.GetValue("Items", typeof(Item[]));
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000E1C5 File Offset: 0x0000D1C5
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("State", this.m_state);
			info.AddValue("Filters", this.m_filters);
			info.AddValue("Items", this.m_items);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000E1FC File Offset: 0x0000D1FC
		public virtual object Clone()
		{
			Subscription subscription = (Subscription)base.MemberwiseClone();
			subscription.m_server = null;
			subscription.m_subscription = null;
			subscription.m_state = (SubscriptionState)this.m_state.Clone();
			subscription.m_state.ServerHandle = null;
			subscription.m_state.Active = false;
			if (subscription.m_items != null)
			{
				ArrayList arrayList = new ArrayList();
				foreach (Item item in subscription.m_items)
				{
					arrayList.Add(item.Clone());
				}
				subscription.m_items = (Item[])arrayList.ToArray(typeof(Item));
			}
			return subscription;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000E2A6 File Offset: 0x0000D2A6
		public Server Server
		{
			get
			{
				return this.m_server;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000E2AE File Offset: 0x0000D2AE
		public string Name
		{
			get
			{
				return this.m_state.Name;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000E2BB File Offset: 0x0000D2BB
		public object ClientHandle
		{
			get
			{
				return this.m_state.ClientHandle;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000E2C8 File Offset: 0x0000D2C8
		public object ServerHandle
		{
			get
			{
				return this.m_state.ServerHandle;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0000E2D5 File Offset: 0x0000D2D5
		public bool Active
		{
			get
			{
				return this.m_state.Active;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000E2E2 File Offset: 0x0000D2E2
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0000E2EA File Offset: 0x0000D2EA
		public string Locale
		{
			get
			{
				return this.m_state.Locale;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000E2F7 File Offset: 0x0000D2F7
		public int Filters
		{
			get
			{
				return this.m_filters;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0000E2FF File Offset: 0x0000D2FF
		public SubscriptionState State
		{
			get
			{
				return (SubscriptionState)this.m_state.Clone();
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000E314 File Offset: 0x0000D314
		public Item[] Items
		{
			get
			{
				if (this.m_items == null)
				{
					return new Item[0];
				}
				Item[] array = new Item[this.m_items.Length];
				for (int i = 0; i < this.m_items.Length; i++)
				{
					array[i] = (Item)this.m_items[i].Clone();
				}
				return array;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060004DE RID: 1246 RVA: 0x0000E367 File Offset: 0x0000D367
		// (remove) Token: 0x060004DF RID: 1247 RVA: 0x0000E375 File Offset: 0x0000D375
		public event DataChangedEventHandler DataChanged
		{
			add
			{
				this.m_subscription.DataChanged += value;
			}
			remove
			{
				this.m_subscription.DataChanged -= value;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000E383 File Offset: 0x0000D383
		public int GetResultFilters()
		{
			this.m_filters = this.m_subscription.GetResultFilters();
			return this.m_filters;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000E39C File Offset: 0x0000D39C
		public void SetResultFilters(int filters)
		{
			this.m_subscription.SetResultFilters(filters);
			this.m_filters = filters;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000E3B1 File Offset: 0x0000D3B1
		public SubscriptionState GetState()
		{
			this.m_state = this.m_subscription.GetState();
			return this.m_state;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000E3CA File Offset: 0x0000D3CA
		public SubscriptionState ModifyState(int masks, SubscriptionState state)
		{
			this.m_state = this.m_subscription.ModifyState(masks, state);
			return this.m_state;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000E3E8 File Offset: 0x0000D3E8
		public virtual ItemResult[] AddItems(Item[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Length == 0)
			{
				return new ItemResult[0];
			}
			ItemResult[] array = this.m_subscription.AddItems(items);
			if (array == null || array.Length == 0)
			{
				throw new InvalidResponseException();
			}
			ArrayList arrayList = new ArrayList();
			if (this.m_items != null)
			{
				arrayList.AddRange(this.m_items);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].ResultID.Failed())
				{
					arrayList.Add(new Item(array[i])
					{
						ItemName = items[i].ItemName,
						ItemPath = items[i].ItemPath,
						ClientHandle = items[i].ClientHandle
					});
				}
			}
			this.m_items = (Item[])arrayList.ToArray(typeof(Item));
			this.GetState();
			return array;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000E4C4 File Offset: 0x0000D4C4
		public virtual ItemResult[] ModifyItems(int masks, Item[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Length == 0)
			{
				return new ItemResult[0];
			}
			ItemResult[] array = this.m_subscription.ModifyItems(masks, items);
			if (array == null || array.Length == 0)
			{
				throw new InvalidResponseException();
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].ResultID.Failed())
				{
					for (int j = 0; j < this.m_items.Length; j++)
					{
						if (this.m_items[j].ServerHandle.Equals(items[i].ServerHandle))
						{
							Item item = new Item(array[i]);
							item.ItemName = this.m_items[j].ItemName;
							item.ItemPath = this.m_items[j].ItemPath;
							item.ClientHandle = this.m_items[j].ClientHandle;
							this.m_items[j] = item;
							break;
						}
					}
				}
			}
			this.GetState();
			return array;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000E5B4 File Offset: 0x0000D5B4
		public virtual IdentifiedResult[] RemoveItems(ItemIdentifier[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Length == 0)
			{
				return new IdentifiedResult[0];
			}
			IdentifiedResult[] array = this.m_subscription.RemoveItems(items);
			if (array == null || array.Length == 0)
			{
				throw new InvalidResponseException();
			}
			ArrayList arrayList = new ArrayList();
			foreach (Item item in this.m_items)
			{
				bool flag = false;
				for (int j = 0; j < array.Length; j++)
				{
					if (item.ServerHandle.Equals(items[j].ServerHandle))
					{
						flag = array[j].ResultID.Succeeded();
						break;
					}
				}
				if (!flag)
				{
					arrayList.Add(item);
				}
			}
			this.m_items = (Item[])arrayList.ToArray(typeof(Item));
			this.GetState();
			return array;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000E68A File Offset: 0x0000D68A
		public ItemValueResult[] Read(Item[] items)
		{
			return this.m_subscription.Read(items);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000E698 File Offset: 0x0000D698
		public IdentifiedResult[] Write(ItemValue[] items)
		{
			return this.m_subscription.Write(items);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000E6A6 File Offset: 0x0000D6A6
		public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request)
		{
			return this.m_subscription.Read(items, requestHandle, callback, out request);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000E6B8 File Offset: 0x0000D6B8
		public IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request)
		{
			return this.m_subscription.Write(items, requestHandle, callback, out request);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000E6CA File Offset: 0x0000D6CA
		public void Cancel(IRequest request, CancelCompleteEventHandler callback)
		{
			this.m_subscription.Cancel(request, callback);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000E6D9 File Offset: 0x0000D6D9
		public void Refresh()
		{
			this.m_subscription.Refresh();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000E6E6 File Offset: 0x0000D6E6
		public void Refresh(object requestHandle, out IRequest request)
		{
			this.m_subscription.Refresh(requestHandle, out request);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000E6F5 File Offset: 0x0000D6F5
		public void SetEnabled(bool enabled)
		{
			this.m_subscription.SetEnabled(enabled);
			this.m_enabled = enabled;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000E70A File Offset: 0x0000D70A
		public bool GetEnabled()
		{
			this.m_enabled = this.m_subscription.GetEnabled();
			return this.m_enabled;
		}

		// Token: 0x04000259 RID: 601
		internal Server m_server;

		// Token: 0x0400025A RID: 602
		internal ISubscription m_subscription;

		// Token: 0x0400025B RID: 603
		private SubscriptionState m_state = new SubscriptionState();

		// Token: 0x0400025C RID: 604
		private Item[] m_items;

		// Token: 0x0400025D RID: 605
		private bool m_enabled = true;

		// Token: 0x0400025E RID: 606
		private int m_filters = 9;

		// Token: 0x020000A0 RID: 160
		private class Names
		{
			// Token: 0x0400025F RID: 607
			internal const string STATE = "State";

			// Token: 0x04000260 RID: 608
			internal const string FILTERS = "Filters";

			// Token: 0x04000261 RID: 609
			internal const string ITEMS = "Items";
		}
	}
}
