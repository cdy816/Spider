using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000AF RID: 175
	[Serializable]
	public class ItemTimeCollection : ItemIdentifier, ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x1700014A RID: 330
		public DateTime this[int index]
		{
			get
			{
				return (DateTime)this.m_times[index];
			}
			set
			{
				this.m_times[index] = value;
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00010438 File Offset: 0x0000F438
		public ItemTimeCollection()
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001044B File Offset: 0x0000F44B
		public ItemTimeCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00010460 File Offset: 0x0000F460
		public ItemTimeCollection(ItemTimeCollection item) : base(item)
		{
			this.m_times = new ArrayList(item.m_times.Count);
			foreach (object obj in item.m_times)
			{
				DateTime dateTime = (DateTime)obj;
				this.m_times.Add(dateTime);
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000104EC File Offset: 0x0000F4EC
		public override object Clone()
		{
			ItemTimeCollection itemTimeCollection = (ItemTimeCollection)base.Clone();
			itemTimeCollection.m_times = new ArrayList(this.m_times.Count);
			foreach (object obj in this.m_times)
			{
				DateTime dateTime = (DateTime)obj;
				itemTimeCollection.m_times.Add(dateTime);
			}
			return itemTimeCollection;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00010574 File Offset: 0x0000F574
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00010577 File Offset: 0x0000F577
		public int Count
		{
			get
			{
				if (this.m_times == null)
				{
					return 0;
				}
				return this.m_times.Count;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001058E File Offset: 0x0000F58E
		public void CopyTo(Array array, int index)
		{
			if (this.m_times != null)
			{
				this.m_times.CopyTo(array, index);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000105A5 File Offset: 0x0000F5A5
		public void CopyTo(DateTime[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x000105AF File Offset: 0x0000F5AF
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000105B2 File Offset: 0x0000F5B2
		public IEnumerator GetEnumerator()
		{
			return this.m_times.GetEnumerator();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x000105BF File Offset: 0x0000F5BF
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014F RID: 335
		object IList.this[int index]
		{
			get
			{
				return this.m_times[index];
			}
			set
			{
				if (!typeof(DateTime).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add DateTime objects into the collection.");
				}
				this.m_times[index] = value;
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000105FC File Offset: 0x0000F5FC
		public void RemoveAt(int index)
		{
			this.m_times.RemoveAt(index);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001060A File Offset: 0x0000F60A
		public void Insert(int index, object value)
		{
			if (!typeof(DateTime).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add DateTime objects into the collection.");
			}
			this.m_times.Insert(index, value);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00010636 File Offset: 0x0000F636
		public void Remove(object value)
		{
			this.m_times.Remove(value);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00010644 File Offset: 0x0000F644
		public bool Contains(object value)
		{
			return this.m_times.Contains(value);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00010652 File Offset: 0x0000F652
		public void Clear()
		{
			this.m_times.Clear();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001065F File Offset: 0x0000F65F
		public int IndexOf(object value)
		{
			return this.m_times.IndexOf(value);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001066D File Offset: 0x0000F66D
		public int Add(object value)
		{
			if (!typeof(DateTime).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add DateTime objects into the collection.");
			}
			return this.m_times.Add(value);
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00010698 File Offset: 0x0000F698
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001069B File Offset: 0x0000F69B
		public void Insert(int index, DateTime value)
		{
			this.Insert(index, value);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000106AA File Offset: 0x0000F6AA
		public void Remove(DateTime value)
		{
			this.Remove(value);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000106B8 File Offset: 0x0000F6B8
		public bool Contains(DateTime value)
		{
			return this.Contains(value);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000106C6 File Offset: 0x0000F6C6
		public int IndexOf(DateTime value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000106D4 File Offset: 0x0000F6D4
		public int Add(DateTime value)
		{
			return this.Add(value);
		}

		// Token: 0x040002A2 RID: 674
		private ArrayList m_times = new ArrayList();
	}
}
