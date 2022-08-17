using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	public class TrendCollection : ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x1700011C RID: 284
		public Trend this[int index]
		{
			get
			{
				return (Trend)this.m_trends[index];
			}
		}

		// Token: 0x1700011D RID: 285
		public Trend this[string name]
		{
			get
			{
				foreach (object obj in this.m_trends)
				{
					Trend trend = (Trend)obj;
					if (trend.Name == name)
					{
						return trend;
					}
				}
				return null;
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000F600 File Offset: 0x0000E600
		public TrendCollection()
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000F614 File Offset: 0x0000E614
		public TrendCollection(TrendCollection items)
		{
			if (items != null)
			{
				foreach (object obj in items)
				{
					Trend value = (Trend)obj;
					this.Add(value);
				}
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000F680 File Offset: 0x0000E680
		public virtual object Clone()
		{
			TrendCollection trendCollection = (TrendCollection)base.MemberwiseClone();
			trendCollection.m_trends = new ArrayList();
			foreach (object obj in this.m_trends)
			{
				Trend trend = (Trend)obj;
				trendCollection.m_trends.Add(trend.Clone());
			}
			return trendCollection;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0000F6FC File Offset: 0x0000E6FC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0000F6FF File Offset: 0x0000E6FF
		public int Count
		{
			get
			{
				if (this.m_trends == null)
				{
					return 0;
				}
				return this.m_trends.Count;
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000F716 File Offset: 0x0000E716
		public void CopyTo(Array array, int index)
		{
			if (this.m_trends != null)
			{
				this.m_trends.CopyTo(array, index);
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000F72D File Offset: 0x0000E72D
		public void CopyTo(Trend[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0000F737 File Offset: 0x0000E737
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000F73A File Offset: 0x0000E73A
		public IEnumerator GetEnumerator()
		{
			return this.m_trends.GetEnumerator();
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0000F747 File Offset: 0x0000E747
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000122 RID: 290
		object IList.this[int index]
		{
			get
			{
				return this.m_trends[index];
			}
			set
			{
				if (!typeof(Trend).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add Trend objects into the collection.");
				}
				this.m_trends[index] = value;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0000F784 File Offset: 0x0000E784
		public void RemoveAt(int index)
		{
			this.m_trends.RemoveAt(index);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0000F792 File Offset: 0x0000E792
		public void Insert(int index, object value)
		{
			if (!typeof(Trend).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Trend objects into the collection.");
			}
			this.m_trends.Insert(index, value);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000F7BE File Offset: 0x0000E7BE
		public void Remove(object value)
		{
			this.m_trends.Remove(value);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000F7CC File Offset: 0x0000E7CC
		public bool Contains(object value)
		{
			return this.m_trends.Contains(value);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000F7DA File Offset: 0x0000E7DA
		public void Clear()
		{
			this.m_trends.Clear();
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000F7E7 File Offset: 0x0000E7E7
		public int IndexOf(object value)
		{
			return this.m_trends.IndexOf(value);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000F7F5 File Offset: 0x0000E7F5
		public int Add(object value)
		{
			if (!typeof(Trend).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Trend objects into the collection.");
			}
			return this.m_trends.Add(value);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0000F820 File Offset: 0x0000E820
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000F823 File Offset: 0x0000E823
		public void Insert(int index, Trend value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0000F82D File Offset: 0x0000E82D
		public void Remove(Trend value)
		{
			this.Remove(value);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000F836 File Offset: 0x0000E836
		public bool Contains(Trend value)
		{
			return this.Contains(value);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000F83F File Offset: 0x0000E83F
		public int IndexOf(Trend value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000F848 File Offset: 0x0000E848
		public int Add(Trend value)
		{
			return this.Add(value);
		}

		// Token: 0x04000280 RID: 640
		private ArrayList m_trends = new ArrayList();
	}
}
