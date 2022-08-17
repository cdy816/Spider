using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	public class ItemCollection : ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x17000126 RID: 294
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

		// Token: 0x17000127 RID: 295
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

		// Token: 0x0600056A RID: 1386 RVA: 0x0000F96C File Offset: 0x0000E96C
		public ItemCollection()
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0000F980 File Offset: 0x0000E980
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

		// Token: 0x0600056C RID: 1388 RVA: 0x0000F9EC File Offset: 0x0000E9EC
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000FA68 File Offset: 0x0000EA68
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0000FA6B File Offset: 0x0000EA6B
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

		// Token: 0x0600056F RID: 1391 RVA: 0x0000FA82 File Offset: 0x0000EA82
		public void CopyTo(Array array, int index)
		{
			if (this.m_items != null)
			{
				this.m_items.CopyTo(array, index);
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0000FA99 File Offset: 0x0000EA99
		public void CopyTo(Item[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0000FAA3 File Offset: 0x0000EAA3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000FAA6 File Offset: 0x0000EAA6
		public IEnumerator GetEnumerator()
		{
			return this.m_items.GetEnumerator();
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0000FAB3 File Offset: 0x0000EAB3
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012C RID: 300
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

		// Token: 0x06000576 RID: 1398 RVA: 0x0000FAF0 File Offset: 0x0000EAF0
		public void RemoveAt(int index)
		{
			this.m_items.RemoveAt(index);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000FAFE File Offset: 0x0000EAFE
		public void Insert(int index, object value)
		{
			if (!typeof(Item).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Item objects into the collection.");
			}
			this.m_items.Insert(index, value);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000FB2A File Offset: 0x0000EB2A
		public void Remove(object value)
		{
			this.m_items.Remove(value);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000FB38 File Offset: 0x0000EB38
		public bool Contains(object value)
		{
			return this.m_items.Contains(value);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000FB46 File Offset: 0x0000EB46
		public void Clear()
		{
			this.m_items.Clear();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000FB53 File Offset: 0x0000EB53
		public int IndexOf(object value)
		{
			return this.m_items.IndexOf(value);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000FB61 File Offset: 0x0000EB61
		public int Add(object value)
		{
			if (!typeof(Item).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Item objects into the collection.");
			}
			return this.m_items.Add(value);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000FB8C File Offset: 0x0000EB8C
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000FB8F File Offset: 0x0000EB8F
		public void Insert(int index, Item value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000FB99 File Offset: 0x0000EB99
		public void Remove(Item value)
		{
			this.Remove(value);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000FBA2 File Offset: 0x0000EBA2
		public bool Contains(Item value)
		{
			return this.Contains(value);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000FBAB File Offset: 0x0000EBAB
		public int IndexOf(Item value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000FBB4 File Offset: 0x0000EBB4
		public int Add(Item value)
		{
			return this.Add(value);
		}

		// Token: 0x04000283 RID: 643
		private ArrayList m_items = new ArrayList();
	}
}
