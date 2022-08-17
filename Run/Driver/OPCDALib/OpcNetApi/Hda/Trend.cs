using System;
using System.Runtime.Serialization;

namespace Opc.Hda
{
	// Token: 0x020000A2 RID: 162
	[Serializable]
	public class Trend : ISerializable, ICloneable
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x0000E768 File Offset: 0x0000D768
		public Trend(Server server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			this.m_server = server;
			do
			{
				this.Name = string.Format("Trend{0,2:00}", ++Trend.m_count);
			}
			while (this.m_server.Trends[this.Name] != null);
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0000E7E0 File Offset: 0x0000D7E0
		public Server Server
		{
			get
			{
				return this.m_server;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0000E7E8 File Offset: 0x0000D7E8
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x0000E7F0 File Offset: 0x0000D7F0
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000E7F9 File Offset: 0x0000D7F9
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x0000E801 File Offset: 0x0000D801
		public int AggregateID
		{
			get
			{
				return this.m_aggregateID;
			}
			set
			{
				this.m_aggregateID = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000E80A File Offset: 0x0000D80A
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0000E812 File Offset: 0x0000D812
		public Time StartTime
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000E81B File Offset: 0x0000D81B
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x0000E823 File Offset: 0x0000D823
		public Time EndTime
		{
			get
			{
				return this.m_endTime;
			}
			set
			{
				this.m_endTime = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000E82C File Offset: 0x0000D82C
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x0000E834 File Offset: 0x0000D834
		public int MaxValues
		{
			get
			{
				return this.m_maxValues;
			}
			set
			{
				this.m_maxValues = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0000E83D File Offset: 0x0000D83D
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x0000E845 File Offset: 0x0000D845
		public bool IncludeBounds
		{
			get
			{
				return this.m_includeBounds;
			}
			set
			{
				this.m_includeBounds = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0000E84E File Offset: 0x0000D84E
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x0000E856 File Offset: 0x0000D856
		public decimal ResampleInterval
		{
			get
			{
				return this.m_resampleInterval;
			}
			set
			{
				this.m_resampleInterval = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000E85F File Offset: 0x0000D85F
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0000E867 File Offset: 0x0000D867
		public ItemTimeCollection Timestamps
		{
			get
			{
				return this.m_timestamps;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_timestamps = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000E87E File Offset: 0x0000D87E
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x0000E886 File Offset: 0x0000D886
		public decimal UpdateInterval
		{
			get
			{
				return this.m_updateInterval;
			}
			set
			{
				this.m_updateInterval = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x0000E88F File Offset: 0x0000D88F
		public bool SubscriptionActive
		{
			get
			{
				return this.m_subscription != null;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0000E89D File Offset: 0x0000D89D
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0000E8A5 File Offset: 0x0000D8A5
		public decimal PlaybackInterval
		{
			get
			{
				return this.m_playbackInterval;
			}
			set
			{
				this.m_playbackInterval = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0000E8AE File Offset: 0x0000D8AE
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0000E8B6 File Offset: 0x0000D8B6
		public decimal PlaybackDuration
		{
			get
			{
				return this.m_playbackDuration;
			}
			set
			{
				this.m_playbackDuration = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0000E8BF File Offset: 0x0000D8BF
		public bool PlaybackActive
		{
			get
			{
				return this.m_playback != null;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0000E8CD File Offset: 0x0000D8CD
		public ItemCollection Items
		{
			get
			{
				return this.m_items;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000E8D8 File Offset: 0x0000D8D8
		public Item[] GetItems()
		{
			Item[] array = new Item[this.m_items.Count];
			for (int i = 0; i < this.m_items.Count; i++)
			{
				array[i] = this.m_items[i];
			}
			return array;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000E91C File Offset: 0x0000D91C
		public Item AddItem(ItemIdentifier itemID)
		{
			if (itemID == null)
			{
				throw new ArgumentNullException("itemID");
			}
			if (itemID.ClientHandle == null)
			{
				itemID.ClientHandle = Guid.NewGuid().ToString();
			}
			IdentifiedResult[] array = this.m_server.CreateItems(new ItemIdentifier[]
			{
				itemID
			});
			if (array == null || array.Length != 1)
			{
				throw new InvalidResponseException();
			}
			if (array[0].ResultID.Failed())
			{
				throw new ResultIDException(array[0].ResultID, "Could not add item to trend.");
			}
			Item item = new Item(array[0]);
			this.m_items.Add(item);
			return item;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000E9BC File Offset: 0x0000D9BC
		public void RemoveItem(Item item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			for (int i = 0; i < this.m_items.Count; i++)
			{
				if (item.Equals(this.m_items[i]))
				{
					this.m_server.ReleaseItems(new ItemIdentifier[]
					{
						item
					});
					this.m_items.RemoveAt(i);
					return;
				}
			}
			throw new ArgumentOutOfRangeException("item", item.Key, "Item not found in collection.");
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000EA3B File Offset: 0x0000DA3B
		public void ClearItems()
		{
			this.m_server.ReleaseItems(this.GetItems());
			this.m_items.Clear();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000EA5A File Offset: 0x0000DA5A
		public ItemValueCollection[] Read()
		{
			return this.Read(this.GetItems());
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000EA68 File Offset: 0x0000DA68
		public ItemValueCollection[] Read(Item[] items)
		{
			if (this.AggregateID == 0)
			{
				return this.ReadRaw(items);
			}
			return this.ReadProcessed(items);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000EA81 File Offset: 0x0000DA81
		public IdentifiedResult[] Read(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.Read(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000EA92 File Offset: 0x0000DA92
		public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			if (this.AggregateID == 0)
			{
				return this.ReadRaw(items, requestHandle, callback, out request);
			}
			return this.ReadProcessed(items, requestHandle, callback, out request);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000EAB3 File Offset: 0x0000DAB3
		public ItemValueCollection[] ReadRaw()
		{
			return this.ReadRaw(this.GetItems());
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000EAC4 File Offset: 0x0000DAC4
		public ItemValueCollection[] ReadRaw(Item[] items)
		{
			return this.m_server.ReadRaw(this.StartTime, this.EndTime, this.MaxValues, this.IncludeBounds, items);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000EAF7 File Offset: 0x0000DAF7
		public IdentifiedResult[] ReadRaw(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.Read(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000EB08 File Offset: 0x0000DB08
		public IdentifiedResult[] ReadRaw(ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.m_server.ReadRaw(this.StartTime, this.EndTime, this.MaxValues, this.IncludeBounds, items, requestHandle, callback, out request);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000EB3F File Offset: 0x0000DB3F
		public ItemValueCollection[] ReadProcessed()
		{
			return this.ReadProcessed(this.GetItems());
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000EB50 File Offset: 0x0000DB50
		public ItemValueCollection[] ReadProcessed(Item[] items)
		{
			Item[] items2 = this.ApplyDefaultAggregate(items);
			return this.m_server.ReadProcessed(this.StartTime, this.EndTime, this.ResampleInterval, items2);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000EB85 File Offset: 0x0000DB85
		public IdentifiedResult[] ReadProcessed(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.ReadProcessed(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000EB98 File Offset: 0x0000DB98
		public IdentifiedResult[] ReadProcessed(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			Item[] items2 = this.ApplyDefaultAggregate(items);
			return this.m_server.ReadProcessed(this.StartTime, this.EndTime, this.ResampleInterval, items2, requestHandle, callback, out request);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000EBD4 File Offset: 0x0000DBD4
		public IdentifiedResult[] Subscribe(object subscriptionHandle, DataUpdateEventHandler callback)
		{
			IdentifiedResult[] result;
			if (this.AggregateID == 0)
			{
				result = this.m_server.AdviseRaw(this.StartTime, this.UpdateInterval, this.GetItems(), subscriptionHandle, callback, out this.m_subscription);
			}
			else
			{
				Item[] items = this.ApplyDefaultAggregate(this.GetItems());
				result = this.m_server.AdviseProcessed(this.StartTime, this.ResampleInterval, (int)this.UpdateInterval, items, subscriptionHandle, callback, out this.m_subscription);
			}
			return result;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000EC4D File Offset: 0x0000DC4D
		public void SubscribeCancel()
		{
			if (this.m_subscription != null)
			{
				this.m_server.CancelRequest(this.m_subscription);
				this.m_subscription = null;
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000EC70 File Offset: 0x0000DC70
		public IdentifiedResult[] Playback(object playbackHandle, DataUpdateEventHandler callback)
		{
			IdentifiedResult[] result;
			if (this.AggregateID == 0)
			{
				result = this.m_server.PlaybackRaw(this.StartTime, this.EndTime, this.MaxValues, this.PlaybackInterval, this.PlaybackDuration, this.GetItems(), playbackHandle, callback, out this.m_playback);
			}
			else
			{
				Item[] items = this.ApplyDefaultAggregate(this.GetItems());
				result = this.m_server.PlaybackProcessed(this.StartTime, this.EndTime, this.ResampleInterval, (int)this.PlaybackDuration, this.PlaybackInterval, items, playbackHandle, callback, out this.m_playback);
			}
			return result;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000ED07 File Offset: 0x0000DD07
		public void PlaybackCancel()
		{
			if (this.m_playback != null)
			{
				this.m_server.CancelRequest(this.m_playback);
				this.m_playback = null;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000ED29 File Offset: 0x0000DD29
		public ModifiedValueCollection[] ReadModified()
		{
			return this.ReadModified(this.GetItems());
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000ED38 File Offset: 0x0000DD38
		public ModifiedValueCollection[] ReadModified(Item[] items)
		{
			return this.m_server.ReadModified(this.StartTime, this.EndTime, this.MaxValues, items);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000ED65 File Offset: 0x0000DD65
		public IdentifiedResult[] ReadModified(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.ReadModified(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000ED78 File Offset: 0x0000DD78
		public IdentifiedResult[] ReadModified(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.m_server.ReadModified(this.StartTime, this.EndTime, this.MaxValues, items, requestHandle, callback, out request);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000EDA9 File Offset: 0x0000DDA9
		public ItemValueCollection[] ReadAtTime()
		{
			return this.ReadAtTime(this.GetItems());
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000EDB8 File Offset: 0x0000DDB8
		public ItemValueCollection[] ReadAtTime(Item[] items)
		{
			DateTime[] array = new DateTime[this.Timestamps.Count];
			for (int i = 0; i < this.Timestamps.Count; i++)
			{
				array[i] = this.Timestamps[i];
			}
			return this.m_server.ReadAtTime(array, items);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000EE11 File Offset: 0x0000DE11
		public IdentifiedResult[] ReadAtTime(object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			return this.ReadAtTime(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000EE24 File Offset: 0x0000DE24
		public IdentifiedResult[] ReadAtTime(Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
		{
			DateTime[] array = new DateTime[this.Timestamps.Count];
			for (int i = 0; i < this.Timestamps.Count; i++)
			{
				array[i] = this.Timestamps[i];
			}
			return this.m_server.ReadAtTime(array, items, requestHandle, callback, out request);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000EE81 File Offset: 0x0000DE81
		public ItemAttributeCollection ReadAttributes(ItemIdentifier item, int[] attributeIDs)
		{
			return this.m_server.ReadAttributes(this.StartTime, this.EndTime, item, attributeIDs);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000EE9C File Offset: 0x0000DE9C
		public ResultCollection ReadAttributes(ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
		{
			return this.m_server.ReadAttributes(this.StartTime, this.EndTime, item, attributeIDs, requestHandle, callback, out request);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000EEC9 File Offset: 0x0000DEC9
		public AnnotationValueCollection[] ReadAnnotations()
		{
			return this.ReadAnnotations(this.GetItems());
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000EED8 File Offset: 0x0000DED8
		public AnnotationValueCollection[] ReadAnnotations(Item[] items)
		{
			return this.m_server.ReadAnnotations(this.StartTime, this.EndTime, items);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000EEFF File Offset: 0x0000DEFF
		public IdentifiedResult[] ReadAnnotations(object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
		{
			return this.ReadAnnotations(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000EF10 File Offset: 0x0000DF10
		public IdentifiedResult[] ReadAnnotations(Item[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
		{
			return this.m_server.ReadAnnotations(this.StartTime, this.EndTime, items, requestHandle, callback, out request);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000EF3B File Offset: 0x0000DF3B
		public IdentifiedResult[] Delete()
		{
			return this.Delete(this.GetItems());
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000EF4C File Offset: 0x0000DF4C
		public IdentifiedResult[] Delete(Item[] items)
		{
			return this.m_server.Delete(this.StartTime, this.EndTime, items);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000EF73 File Offset: 0x0000DF73
		public IdentifiedResult[] Delete(object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
		{
			return this.Delete(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000EF84 File Offset: 0x0000DF84
		public IdentifiedResult[] Delete(ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
		{
			return this.m_server.Delete(this.StartTime, this.EndTime, items, requestHandle, callback, out request);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000EFAF File Offset: 0x0000DFAF
		public ResultCollection[] DeleteAtTime()
		{
			return this.DeleteAtTime(this.GetItems());
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000EFC0 File Offset: 0x0000DFC0
		public ResultCollection[] DeleteAtTime(Item[] items)
		{
			ItemTimeCollection[] array = new ItemTimeCollection[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = (ItemTimeCollection)this.Timestamps.Clone();
				array[i].ItemName = items[i].ItemName;
				array[i].ItemPath = items[i].ItemPath;
				array[i].ClientHandle = items[i].ClientHandle;
				array[i].ServerHandle = items[i].ServerHandle;
			}
			return this.m_server.DeleteAtTime(array);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000F043 File Offset: 0x0000E043
		public IdentifiedResult[] DeleteAtTime(object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
		{
			return this.DeleteAtTime(this.GetItems(), requestHandle, callback, out request);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000F054 File Offset: 0x0000E054
		public IdentifiedResult[] DeleteAtTime(Item[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
		{
			ItemTimeCollection[] array = new ItemTimeCollection[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = (ItemTimeCollection)this.Timestamps.Clone();
				array[i].ItemName = items[i].ItemName;
				array[i].ItemPath = items[i].ItemPath;
				array[i].ClientHandle = items[i].ClientHandle;
				array[i].ServerHandle = items[i].ServerHandle;
			}
			return this.m_server.DeleteAtTime(array, requestHandle, callback, out request);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000F0DC File Offset: 0x0000E0DC
		protected Trend(SerializationInfo info, StreamingContext context)
		{
			this.m_name = (string)info.GetValue("Name", typeof(string));
			this.m_aggregateID = (int)info.GetValue("AggregateID", typeof(int));
			this.m_startTime = (Time)info.GetValue("StartTime", typeof(Time));
			this.m_endTime = (Time)info.GetValue("EndTime", typeof(Time));
			this.m_maxValues = (int)info.GetValue("MaxValues", typeof(int));
			this.m_includeBounds = (bool)info.GetValue("IncludeBounds", typeof(bool));
			this.m_resampleInterval = (decimal)info.GetValue("ResampleInterval", typeof(decimal));
			this.m_updateInterval = (decimal)info.GetValue("UpdateInterval", typeof(decimal));
			this.m_playbackInterval = (decimal)info.GetValue("PlaybackInterval", typeof(decimal));
			this.m_playbackDuration = (decimal)info.GetValue("PlaybackDuration", typeof(decimal));
			DateTime[] array = (DateTime[])info.GetValue("Timestamps", typeof(DateTime[]));
			if (array != null)
			{
				foreach (DateTime value in array)
				{
					this.m_timestamps.Add(value);
				}
			}
			Item[] array3 = (Item[])info.GetValue("Items", typeof(Item[]));
			if (array3 != null)
			{
				foreach (Item value2 in array3)
				{
					this.m_items.Add(value2);
				}
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000F2DC File Offset: 0x0000E2DC
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", this.m_name);
			info.AddValue("AggregateID", this.m_aggregateID);
			info.AddValue("StartTime", this.m_startTime);
			info.AddValue("EndTime", this.m_endTime);
			info.AddValue("MaxValues", this.m_maxValues);
			info.AddValue("IncludeBounds", this.m_includeBounds);
			info.AddValue("ResampleInterval", this.m_resampleInterval);
			info.AddValue("UpdateInterval", this.m_updateInterval);
			info.AddValue("PlaybackInterval", this.m_playbackInterval);
			info.AddValue("PlaybackDuration", this.m_playbackDuration);
			DateTime[] array = null;
			if (this.m_timestamps.Count > 0)
			{
				array = new DateTime[this.m_timestamps.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.m_timestamps[i];
				}
			}
			info.AddValue("Timestamps", array);
			Item[] array2 = null;
			if (this.m_items.Count > 0)
			{
				array2 = new Item[this.m_items.Count];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = this.m_items[j];
				}
			}
			info.AddValue("Items", array2);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000F430 File Offset: 0x0000E430
		internal void SetServer(Server server)
		{
			this.m_server = server;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000F43C File Offset: 0x0000E43C
		public virtual object Clone()
		{
			Trend trend = (Trend)base.MemberwiseClone();
			trend.m_items = new ItemCollection();
			foreach (object obj in this.m_items)
			{
				Item item = (Item)obj;
				trend.m_items.Add(item.Clone());
			}
			trend.m_timestamps = new ItemTimeCollection();
			foreach (object obj2 in this.m_timestamps)
			{
				DateTime value = (DateTime)obj2;
				trend.m_timestamps.Add(value);
			}
			trend.m_subscription = null;
			trend.m_playback = null;
			return trend;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000F52C File Offset: 0x0000E52C
		private Item[] ApplyDefaultAggregate(Item[] items)
		{
			int num = this.AggregateID;
			if (num == 0)
			{
				num = 1;
			}
			Item[] array = new Item[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new Item(items[i]);
				if (array[i].AggregateID == 0)
				{
					array[i].AggregateID = num;
				}
			}
			return array;
		}

		// Token: 0x04000264 RID: 612
		private static int m_count;

		// Token: 0x04000265 RID: 613
		private Server m_server;

		// Token: 0x04000266 RID: 614
		private string m_name;

		// Token: 0x04000267 RID: 615
		private int m_aggregateID;

		// Token: 0x04000268 RID: 616
		private Time m_startTime;

		// Token: 0x04000269 RID: 617
		private Time m_endTime;

		// Token: 0x0400026A RID: 618
		private int m_maxValues;

		// Token: 0x0400026B RID: 619
		private bool m_includeBounds;

		// Token: 0x0400026C RID: 620
		private decimal m_resampleInterval;

		// Token: 0x0400026D RID: 621
		private ItemTimeCollection m_timestamps = new ItemTimeCollection();

		// Token: 0x0400026E RID: 622
		private ItemCollection m_items = new ItemCollection();

		// Token: 0x0400026F RID: 623
		private decimal m_updateInterval;

		// Token: 0x04000270 RID: 624
		private decimal m_playbackInterval;

		// Token: 0x04000271 RID: 625
		private decimal m_playbackDuration;

		// Token: 0x04000272 RID: 626
		private IRequest m_subscription;

		// Token: 0x04000273 RID: 627
		private IRequest m_playback;

		// Token: 0x020000A3 RID: 163
		private class Names
		{
			// Token: 0x04000274 RID: 628
			internal const string NAME = "Name";

			// Token: 0x04000275 RID: 629
			internal const string AGGREGATE_ID = "AggregateID";

			// Token: 0x04000276 RID: 630
			internal const string START_TIME = "StartTime";

			// Token: 0x04000277 RID: 631
			internal const string END_TIME = "EndTime";

			// Token: 0x04000278 RID: 632
			internal const string MAX_VALUES = "MaxValues";

			// Token: 0x04000279 RID: 633
			internal const string INCLUDE_BOUNDS = "IncludeBounds";

			// Token: 0x0400027A RID: 634
			internal const string RESAMPLE_INTERVAL = "ResampleInterval";

			// Token: 0x0400027B RID: 635
			internal const string UPDATE_INTERVAL = "UpdateInterval";

			// Token: 0x0400027C RID: 636
			internal const string PLAYBACK_INTERVAL = "PlaybackInterval";

			// Token: 0x0400027D RID: 637
			internal const string PLAYBACK_DURATION = "PlaybackDuration";

			// Token: 0x0400027E RID: 638
			internal const string TIMESTAMPS = "Timestamps";

			// Token: 0x0400027F RID: 639
			internal const string ITEMS = "Items";
		}
	}
}
