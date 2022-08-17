using System;
using System.Collections;

namespace Opc.Da
{
	// Token: 0x020000BA RID: 186
	[Serializable]
	public class ItemCollection : ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x17000163 RID: 355
		public Item this[int index]
		{
			get
			{
				return (Item)this.m_items[index];
			}
			set
			{
				this.m_items[index] = value;
			}
		}

		// Token: 0x17000164 RID: 356
		public Item this[ItemIdentifier itemID]
		{
			get
			{
				foreach (object obj in this.m_items)
				{
					Item item = (Item)obj;
					if (itemID.Key == item.Key)
					{
						return item;
					}
				}
				return null;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000109D4 File Offset: 0x0000F9D4
		public ItemCollection()
		{
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000109E8 File Offset: 0x0000F9E8
		public ItemCollection(ItemCollection items)
		{
			if (items != null)
			{
				foreach (object obj in items)
				{
					Item value = (Item)obj;
					this.Add(value);
				}
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00010A54 File Offset: 0x0000FA54
		public virtual object Clone()
		{
			ItemCollection itemCollection = (ItemCollection)base.MemberwiseClone();
			itemCollection.m_items = new ArrayList();
			foreach (object obj in this.m_items)
			{
				Item item = (Item)obj;
				itemCollection.m_items.Add(item.Clone());
			}
			return itemCollection;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00010AD0 File Offset: 0x0000FAD0
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00010AD3 File Offset: 0x0000FAD3
		public int Count
		{
			get
			{
				if (this.m_items == null)
				{
					return 0;
				}
				return this.m_items.Count;
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00010AEA File Offset: 0x0000FAEA
		public void CopyTo(Array array, int index)
		{
			if (this.m_items != null)
			{
				this.m_items.CopyTo(array, index);
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00010B01 File Offset: 0x0000FB01
		public void CopyTo(Item[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00010B0B File Offset: 0x0000FB0B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00010B0E File Offset: 0x0000FB0E
		public IEnumerator GetEnumerator()
		{
			return this.m_items.GetEnumerator();
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00010B1B File Offset: 0x0000FB1B
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000169 RID: 361
		object IList.this[int index]
		{
			get
			{
				return this.m_items[index];
			}
			set
			{
				if (!typeof(Item).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add Item objects into the collection.");
				}
				this.m_items[index] = value;
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00010B58 File Offset: 0x0000FB58
		public void RemoveAt(int index)
		{
			this.m_items.RemoveAt(index);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00010B66 File Offset: 0x0000FB66
		public void Insert(int index, object value)
		{
			if (!typeof(Item).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Item objects into the collection.");
			}
			this.m_items.Insert(index, value);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00010B92 File Offset: 0x0000FB92
		public void Remove(object value)
		{
			this.m_items.Remove(value);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00010BA0 File Offset: 0x0000FBA0
		public bool Contains(object value)
		{
			return this.m_items.Contains(value);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00010BAE File Offset: 0x0000FBAE
		public void Clear()
		{
			this.m_items.Clear();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00010BBB File Offset: 0x0000FBBB
		public int IndexOf(object value)
		{
			return this.m_items.IndexOf(value);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00010BC9 File Offset: 0x0000FBC9
		public int Add(object value)
		{
			if (!typeof(Item).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Item objects into the collection.");
			}
			return this.m_items.Add(value);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00010BF4 File Offset: 0x0000FBF4
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00010BF7 File Offset: 0x0000FBF7
		public void Insert(int index, Item value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00010C01 File Offset: 0x0000FC01
		public void Remove(Item value)
		{
			this.Remove(value);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00010C0A File Offset: 0x0000FC0A
		public bool Contains(Item value)
		{
			return this.Contains(value);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00010C13 File Offset: 0x0000FC13
		public int IndexOf(Item value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00010C1C File Offset: 0x0000FC1C
		public int Add(Item value)
		{
			return this.Add(value);
		}

		// Token: 0x040002B9 RID: 697
		private ArrayList m_items = new ArrayList();
	}
}
