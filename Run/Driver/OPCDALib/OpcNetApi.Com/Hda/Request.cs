using System;
using System.Collections;
using Opc;
using Opc.Hda;

namespace OpcCom.Hda
{
	// Token: 0x02000004 RID: 4
	internal class Request : IRequest, IActualTime
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002716 File Offset: 0x00001716
		public int RequestID
		{
			get
			{
				return this.m_requestID;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000271E File Offset: 0x0000171E
		public int CancelID
		{
			get
			{
				return this.m_cancelID;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000012 RID: 18 RVA: 0x00002728 File Offset: 0x00001728
		// (remove) Token: 0x06000013 RID: 19 RVA: 0x00002770 File Offset: 0x00001770
		public event CancelCompleteEventHandler CancelComplete
		{
			add
			{
				lock (this)
				{
					this.m_cancelComplete = (CancelCompleteEventHandler)Delegate.Combine(this.m_cancelComplete, value);
				}
			}
			remove
			{
				lock (this)
				{
					this.m_cancelComplete = (CancelCompleteEventHandler)Delegate.Remove(this.m_cancelComplete, value);
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027B8 File Offset: 0x000017B8
		public Request(object requestHandle, Delegate callback, int requestID)
		{
			this.m_requestHandle = requestHandle;
			this.m_callback = callback;
			this.m_requestID = requestID;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000027EC File Offset: 0x000017EC
		public bool Update(int cancelID, ItemIdentifier[] results)
		{
			bool result;
			lock (this)
			{
				this.m_cancelID = cancelID;
				this.m_items = new Hashtable();
				foreach (ItemIdentifier itemIdentifier in results)
				{
					if (!typeof(IResult).IsInstanceOfType(itemIdentifier) || ((IResult)itemIdentifier).ResultID.Succeeded())
					{
						this.m_items[itemIdentifier.ServerHandle] = new ItemIdentifier(itemIdentifier);
					}
				}
				if (this.m_items.Count == 0)
				{
					result = true;
				}
				else
				{
					bool flag = false;
					if (this.m_results != null)
					{
						foreach (object results2 in this.m_results)
						{
							flag = this.InvokeCallback(results2);
						}
					}
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028F4 File Offset: 0x000018F4
		public bool InvokeCallback(object results)
		{
			bool result;
			lock (this)
			{
				if (this.m_items == null)
				{
					if (this.m_results == null)
					{
						this.m_results = new ArrayList();
					}
					this.m_results.Add(results);
					result = false;
				}
				else if (typeof(DataUpdateEventHandler).IsInstanceOfType(this.m_callback))
				{
					result = this.InvokeCallback((DataUpdateEventHandler)this.m_callback, results);
				}
				else if (typeof(ReadValuesEventHandler).IsInstanceOfType(this.m_callback))
				{
					result = this.InvokeCallback((ReadValuesEventHandler)this.m_callback, results);
				}
				else if (typeof(ReadAttributesEventHandler).IsInstanceOfType(this.m_callback))
				{
					result = this.InvokeCallback((ReadAttributesEventHandler)this.m_callback, results);
				}
				else if (typeof(ReadAnnotationsEventHandler).IsInstanceOfType(this.m_callback))
				{
					result = this.InvokeCallback((ReadAnnotationsEventHandler)this.m_callback, results);
				}
				else if (typeof(UpdateCompleteEventHandler).IsInstanceOfType(this.m_callback))
				{
					result = this.InvokeCallback((UpdateCompleteEventHandler)this.m_callback, results);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A44 File Offset: 0x00001A44
		public void OnCancelComplete()
		{
			lock (this)
			{
				if (this.m_cancelComplete != null)
				{
					this.m_cancelComplete(this);
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002A88 File Offset: 0x00001A88
		public object Handle
		{
			get
			{
				return this.m_requestHandle;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002A90 File Offset: 0x00001A90
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002A98 File Offset: 0x00001A98
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002AA1 File Offset: 0x00001AA1
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002AA9 File Offset: 0x00001AA9
		public DateTime EndTime
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

		// Token: 0x0600001D RID: 29 RVA: 0x00002AB4 File Offset: 0x00001AB4
		private bool InvokeCallback(DataUpdateEventHandler callback, object results)
		{
			if (!typeof(ItemValueCollection[]).IsInstanceOfType(results))
			{
				return false;
			}
			ItemValueCollection[] results2 = (ItemValueCollection[])results;
			this.UpdateResults(results2);
			try
			{
				callback(this, results2);
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B04 File Offset: 0x00001B04
		private bool InvokeCallback(ReadValuesEventHandler callback, object results)
		{
			if (!typeof(ItemValueCollection[]).IsInstanceOfType(results))
			{
				return false;
			}
			ItemValueCollection[] array = (ItemValueCollection[])results;
			this.UpdateResults(array);
			try
			{
				callback(this, array);
			}
			catch
			{
			}
			foreach (ItemValueCollection itemValueCollection in array)
			{
				if (itemValueCollection.ResultID == ResultID.Hda.S_MOREDATA)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B84 File Offset: 0x00001B84
		private bool InvokeCallback(ReadAttributesEventHandler callback, object results)
		{
			if (!typeof(ItemAttributeCollection).IsInstanceOfType(results))
			{
				return false;
			}
			ItemAttributeCollection itemAttributeCollection = (ItemAttributeCollection)results;
			this.UpdateResults(new ItemAttributeCollection[]
			{
				itemAttributeCollection
			});
			try
			{
				callback(this, itemAttributeCollection);
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BDC File Offset: 0x00001BDC
		private bool InvokeCallback(ReadAnnotationsEventHandler callback, object results)
		{
			if (!typeof(AnnotationValueCollection[]).IsInstanceOfType(results))
			{
				return false;
			}
			AnnotationValueCollection[] results2 = (AnnotationValueCollection[])results;
			this.UpdateResults(results2);
			try
			{
				callback(this, results2);
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C2C File Offset: 0x00001C2C
		private bool InvokeCallback(UpdateCompleteEventHandler callback, object results)
		{
			if (!typeof(ResultCollection[]).IsInstanceOfType(results))
			{
				return false;
			}
			ResultCollection[] results2 = (ResultCollection[])results;
			this.UpdateResults(results2);
			try
			{
				callback(this, results2);
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C7C File Offset: 0x00001C7C
		private void UpdateResults(ItemIdentifier[] results)
		{
			foreach (ItemIdentifier itemIdentifier in results)
			{
				if (typeof(IActualTime).IsInstanceOfType(itemIdentifier))
				{
					((IActualTime)itemIdentifier).StartTime = this.StartTime;
					((IActualTime)itemIdentifier).EndTime = this.EndTime;
				}
				ItemIdentifier itemIdentifier2 = (ItemIdentifier)this.m_items[itemIdentifier.ServerHandle];
				if (itemIdentifier2 != null)
				{
					itemIdentifier.ItemName = itemIdentifier2.ItemName;
					itemIdentifier.ItemPath = itemIdentifier2.ItemPath;
					itemIdentifier.ServerHandle = itemIdentifier2.ServerHandle;
					itemIdentifier.ClientHandle = itemIdentifier2.ClientHandle;
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000023 RID: 35 RVA: 0x00002D21 File Offset: 0x00001D21
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x00002D3A File Offset: 0x00001D3A
		private event CancelCompleteEventHandler m_cancelComplete;

		// Token: 0x04000006 RID: 6
		private object m_requestHandle;

		// Token: 0x04000007 RID: 7
		private Delegate m_callback;

		// Token: 0x04000008 RID: 8
		private int m_requestID;

		// Token: 0x04000009 RID: 9
		private int m_cancelID;

		// Token: 0x0400000A RID: 10
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x0400000B RID: 11
		private DateTime m_endTime = DateTime.MinValue;

		// Token: 0x0400000C RID: 12
		private Hashtable m_items;

		// Token: 0x0400000D RID: 13
		private ArrayList m_results;
	}
}
