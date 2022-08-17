using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc.Hda
{
    // Token: 0x02000097 RID: 151
    [Serializable]
    public class Server : Opc.Server, IServer, Opc.IServer, IDisposable
    {
        // Token: 0x0600045B RID: 1115 RVA: 0x0000D166 File Offset: 0x0000C166
        public Server(Factory factory, URL url) : base(factory, url)
        {
        }

        // Token: 0x170000F1 RID: 241
        // (get) Token: 0x0600045C RID: 1116 RVA: 0x0000D19C File Offset: 0x0000C19C
        public AttributeCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        // Token: 0x170000F2 RID: 242
        // (get) Token: 0x0600045D RID: 1117 RVA: 0x0000D1A4 File Offset: 0x0000C1A4
        public AggregateCollection Aggregates
        {
            get
            {
                return this.m_aggregates;
            }
        }

        // Token: 0x170000F3 RID: 243
        // (get) Token: 0x0600045E RID: 1118 RVA: 0x0000D1AC File Offset: 0x0000C1AC
        public ItemIdentifierCollection Items
        {
            get
            {
                return new ItemIdentifierCollection(this.m_items.Values);
            }
        }

        // Token: 0x170000F4 RID: 244
        // (get) Token: 0x0600045F RID: 1119 RVA: 0x0000D1BE File Offset: 0x0000C1BE
        public TrendCollection Trends
        {
            get
            {
                return this.m_trends;
            }
        }

        // Token: 0x06000460 RID: 1120 RVA: 0x0000D1C8 File Offset: 0x0000C1C8
        public override void Connect(URL url, ConnectData connectData)
        {
            base.Connect(url, connectData);
            this.GetAttributes();
            this.GetAggregates();
            foreach (object obj in this.m_trends)
            {
                Trend trend = (Trend)obj;
                ArrayList arrayList = new ArrayList();
                foreach (object obj2 in trend.Items)
                {
                    Item itemID = (Item)obj2;
                    arrayList.Add(new ItemIdentifier(itemID));
                }
                IdentifiedResult[] array = this.CreateItems((ItemIdentifier[])arrayList.ToArray(typeof(ItemIdentifier)));
                if (array != null)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        trend.Items[i].ServerHandle = null;
                        if (array[i].ResultID.Succeeded())
                        {
                            trend.Items[i].ServerHandle = array[i].ServerHandle;
                        }
                    }
                }
            }
        }

        // Token: 0x06000461 RID: 1121 RVA: 0x0000D30C File Offset: 0x0000C30C
        public override void Disconnect()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            if (this.m_items.Count > 0)
            {
                try
                {
                    ArrayList arrayList = new ArrayList(this.m_items.Count);
                    arrayList.AddRange(this.m_items);
                    ((IServer)this.m_server).ReleaseItems((ItemIdentifier[])arrayList.ToArray(typeof(ItemIdentifier)));
                }
                catch
                {
                }
                this.m_items.Clear();
            }
            foreach (object obj in this.m_trends)
            {
                Trend trend = (Trend)obj;
                foreach (object obj2 in trend.Items)
                {
                    Item item = (Item)obj2;
                    item.ServerHandle = null;
                }
            }
            base.Disconnect();
        }

        // Token: 0x06000462 RID: 1122 RVA: 0x0000D434 File Offset: 0x0000C434
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

        // Token: 0x06000463 RID: 1123 RVA: 0x0000D490 File Offset: 0x0000C490
        public Attribute[] GetAttributes()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_attributes.Clear();
            Attribute[] attributes = ((IServer)this.m_server).GetAttributes();
            if (attributes != null)
            {
                this.m_attributes.Init(attributes);
            }
            return attributes;
        }

        // Token: 0x06000464 RID: 1124 RVA: 0x0000D4D8 File Offset: 0x0000C4D8
        public Aggregate[] GetAggregates()
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_aggregates.Clear();
            Aggregate[] aggregates = ((IServer)this.m_server).GetAggregates();
            if (aggregates != null)
            {
                this.m_aggregates.Init(aggregates);
            }
            return aggregates;
        }

        // Token: 0x06000465 RID: 1125 RVA: 0x0000D51F File Offset: 0x0000C51F
        public IBrowser CreateBrowser(BrowseFilter[] filters, out ResultID[] results)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).CreateBrowser(filters, out results);
        }

        // Token: 0x06000466 RID: 1126 RVA: 0x0000D544 File Offset: 0x0000C544
        public IdentifiedResult[] CreateItems(ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            IdentifiedResult[] array = ((IServer)this.m_server).CreateItems(items);
            if (array != null)
            {
                foreach (IdentifiedResult identifiedResult in array)
                {
                    if (identifiedResult.ResultID.Succeeded())
                    {
                        this.m_items.Add(identifiedResult.ServerHandle, new ItemIdentifier(identifiedResult));
                    }
                }
            }
            return array;
        }

        // Token: 0x06000467 RID: 1127 RVA: 0x0000D5B4 File Offset: 0x0000C5B4
        public IdentifiedResult[] ReleaseItems(ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            IdentifiedResult[] array = ((IServer)this.m_server).ReleaseItems(items);
            if (array != null)
            {
                foreach (IdentifiedResult identifiedResult in array)
                {
                    if (identifiedResult.ResultID.Succeeded())
                    {
                        this.m_items.Remove(identifiedResult.ServerHandle);
                    }
                }
            }
            return array;
        }

        // Token: 0x06000468 RID: 1128 RVA: 0x0000D61B File Offset: 0x0000C61B
        public IdentifiedResult[] ValidateItems(ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ValidateItems(items);
        }

        // Token: 0x06000469 RID: 1129 RVA: 0x0000D63C File Offset: 0x0000C63C
        public ItemValueCollection[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadRaw(startTime, endTime, maxValues, includeBounds, items);
        }

        // Token: 0x0600046A RID: 1130 RVA: 0x0000D664 File Offset: 0x0000C664
        public IdentifiedResult[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadRaw(startTime, endTime, maxValues, includeBounds, items, requestHandle, callback, out request);
        }

        // Token: 0x0600046B RID: 1131 RVA: 0x0000D69C File Offset: 0x0000C69C
        public IdentifiedResult[] AdviseRaw(Time startTime, decimal updateInterval, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).AdviseRaw(startTime, updateInterval, items, requestHandle, callback, out request);
        }

        // Token: 0x0600046C RID: 1132 RVA: 0x0000D6C8 File Offset: 0x0000C6C8
        public IdentifiedResult[] PlaybackRaw(Time startTime, Time endTime, int maxValues, decimal updateInterval, decimal playbackDuration, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).PlaybackRaw(startTime, endTime, maxValues, updateInterval, playbackDuration, items, requestHandle, callback, out request);
        }

        // Token: 0x0600046D RID: 1133 RVA: 0x0000D702 File Offset: 0x0000C702
        public ItemValueCollection[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadProcessed(startTime, endTime, resampleInterval, items);
        }

        // Token: 0x0600046E RID: 1134 RVA: 0x0000D728 File Offset: 0x0000C728
        public IdentifiedResult[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadProcessed(startTime, endTime, resampleInterval, items, requestHandle, callback, out request);
        }

        // Token: 0x0600046F RID: 1135 RVA: 0x0000D760 File Offset: 0x0000C760
        public IdentifiedResult[] AdviseProcessed(Time startTime, decimal resampleInterval, int numberOfIntervals, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).AdviseProcessed(startTime, resampleInterval, numberOfIntervals, items, requestHandle, callback, out request);
        }

        // Token: 0x06000470 RID: 1136 RVA: 0x0000D798 File Offset: 0x0000C798
        public IdentifiedResult[] PlaybackProcessed(Time startTime, Time endTime, decimal resampleInterval, int numberOfIntervals, decimal updateInterval, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).PlaybackProcessed(startTime, endTime, resampleInterval, numberOfIntervals, updateInterval, items, requestHandle, callback, out request);
        }

        // Token: 0x06000471 RID: 1137 RVA: 0x0000D7D4 File Offset: 0x0000C7D4
        public ItemValueCollection[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAtTime(timestamps, items);
        }

        // Token: 0x06000472 RID: 1138 RVA: 0x0000D7F8 File Offset: 0x0000C7F8
        public IdentifiedResult[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAtTime(timestamps, items, requestHandle, callback, out request);
        }

        // Token: 0x06000473 RID: 1139 RVA: 0x0000D82C File Offset: 0x0000C82C
        public ModifiedValueCollection[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadModified(startTime, endTime, maxValues, items);
        }

        // Token: 0x06000474 RID: 1140 RVA: 0x0000D854 File Offset: 0x0000C854
        public IdentifiedResult[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadModified(startTime, endTime, maxValues, items, requestHandle, callback, out request);
        }

        // Token: 0x06000475 RID: 1141 RVA: 0x0000D88C File Offset: 0x0000C88C
        public ItemAttributeCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAttributes(startTime, endTime, item, attributeIDs);
        }

        // Token: 0x06000476 RID: 1142 RVA: 0x0000D8B4 File Offset: 0x0000C8B4
        public ResultCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAttributes(startTime, endTime, item, attributeIDs, requestHandle, callback, out request);
        }

        // Token: 0x06000477 RID: 1143 RVA: 0x0000D8EC File Offset: 0x0000C8EC
        public AnnotationValueCollection[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAnnotations(startTime, endTime, items);
        }

        // Token: 0x06000478 RID: 1144 RVA: 0x0000D910 File Offset: 0x0000C910
        public IdentifiedResult[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).ReadAnnotations(startTime, endTime, items, requestHandle, callback, out request);
        }

        // Token: 0x06000479 RID: 1145 RVA: 0x0000D946 File Offset: 0x0000C946
        public ResultCollection[] InsertAnnotations(AnnotationValueCollection[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).InsertAnnotations(items);
        }

        // Token: 0x0600047A RID: 1146 RVA: 0x0000D968 File Offset: 0x0000C968
        public IdentifiedResult[] InsertAnnotations(AnnotationValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).InsertAnnotations(items, requestHandle, callback, out request);
        }

        // Token: 0x0600047B RID: 1147 RVA: 0x0000D99A File Offset: 0x0000C99A
        public ResultCollection[] Insert(ItemValueCollection[] items, bool replace)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Insert(items, replace);
        }

        // Token: 0x0600047C RID: 1148 RVA: 0x0000D9BC File Offset: 0x0000C9BC
        public IdentifiedResult[] Insert(ItemValueCollection[] items, bool replace, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Insert(items, replace, requestHandle, callback, out request);
        }

        // Token: 0x0600047D RID: 1149 RVA: 0x0000D9F0 File Offset: 0x0000C9F0
        public ResultCollection[] Replace(ItemValueCollection[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Replace(items);
        }

        // Token: 0x0600047E RID: 1150 RVA: 0x0000DA14 File Offset: 0x0000CA14
        public IdentifiedResult[] Replace(ItemValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Replace(items, requestHandle, callback, out request);
        }

        // Token: 0x0600047F RID: 1151 RVA: 0x0000DA46 File Offset: 0x0000CA46
        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Delete(startTime, endTime, items);
        }

        // Token: 0x06000480 RID: 1152 RVA: 0x0000DA6C File Offset: 0x0000CA6C
        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).Delete(startTime, endTime, items, requestHandle, callback, out request);
        }

        // Token: 0x06000481 RID: 1153 RVA: 0x0000DAA2 File Offset: 0x0000CAA2
        public ResultCollection[] DeleteAtTime(ItemTimeCollection[] items)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).DeleteAtTime(items);
        }

        // Token: 0x06000482 RID: 1154 RVA: 0x0000DAC4 File Offset: 0x0000CAC4
        public IdentifiedResult[] DeleteAtTime(ItemTimeCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((IServer)this.m_server).DeleteAtTime(items, requestHandle, callback, out request);
        }

        // Token: 0x06000483 RID: 1155 RVA: 0x0000DAF6 File Offset: 0x0000CAF6
        public void CancelRequest(IRequest request)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            ((IServer)this.m_server).CancelRequest(request);
        }

        // Token: 0x06000484 RID: 1156 RVA: 0x0000DB17 File Offset: 0x0000CB17
        public void CancelRequest(IRequest request, CancelCompleteEventHandler callback)
        {
            if (this.m_server == null)
            {
                throw new NotConnectedException();
            }
            ((IServer)this.m_server).CancelRequest(request, callback);
        }

        // Token: 0x06000485 RID: 1157 RVA: 0x0000DB3C File Offset: 0x0000CB3C
        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Trend[] array = (Trend[])info.GetValue("Trends", typeof(Trend[]));
            if (array != null)
            {
                foreach (Trend trend in array)
                {
                    trend.SetServer(this);
                    this.m_trends.Add(trend);
                }
            }
        }

        // Token: 0x06000486 RID: 1158 RVA: 0x0000DBC4 File Offset: 0x0000CBC4
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            Trend[] array = null;
            if (this.m_trends.Count > 0)
            {
                array = new Trend[this.m_trends.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = this.m_trends[i];
                }
            }
            info.AddValue("Trends", array);
        }

        // Token: 0x06000487 RID: 1159 RVA: 0x0000DC24 File Offset: 0x0000CC24
        public override object Clone()
        {
            return (Server)base.Clone();
        }

        // Token: 0x0400024E RID: 590
        private Hashtable m_items = new Hashtable();

        // Token: 0x0400024F RID: 591
        private AttributeCollection m_attributes = new AttributeCollection();

        // Token: 0x04000250 RID: 592
        private AggregateCollection m_aggregates = new AggregateCollection();

        // Token: 0x04000251 RID: 593
        private TrendCollection m_trends = new TrendCollection();

        // Token: 0x02000098 RID: 152
        private class Names
        {
            // Token: 0x04000252 RID: 594
            internal const string TRENDS = "Trends";
        }
    }
}
