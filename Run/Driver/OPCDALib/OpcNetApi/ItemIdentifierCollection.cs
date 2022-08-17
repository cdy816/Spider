using System;
using System.Collections;

namespace Opc
{
	// Token: 0x0200009B RID: 155
	[Serializable]
	public class ItemIdentifierCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x06000498 RID: 1176 RVA: 0x0000DD38 File Offset: 0x0000CD38
		public ItemIdentifierCollection()
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000DD4C File Offset: 0x0000CD4C
		public ItemIdentifierCollection(ICollection collection)
		{
			this.Init(collection);
		}

		// Token: 0x170000F7 RID: 247
		public ItemIdentifier this[int index]
		{
			get
			{
				return this.m_itemIDs[index];
			}
			set
			{
				this.m_itemIDs[index] = value;
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000DD7C File Offset: 0x0000CD7C
		public void Init(ICollection collection)
		{
			this.Clear();
			if (collection != null)
			{
				ArrayList arrayList = new ArrayList(collection.Count);
				foreach (object obj in collection)
				{
					if (typeof(ItemIdentifier).IsInstanceOfType(obj))
					{
						arrayList.Add(((ItemIdentifier)obj).Clone());
					}
				}
				this.m_itemIDs = (ItemIdentifier[])arrayList.ToArray(typeof(ItemIdentifier));
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000DE18 File Offset: 0x0000CE18
		public void Clear()
		{
			this.m_itemIDs = new ItemIdentifier[0];
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000DE26 File Offset: 0x0000CE26
		public virtual object Clone()
		{
			return new ItemIdentifierCollection(this);
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000DE2E File Offset: 0x0000CE2E
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0000DE31 File Offset: 0x0000CE31
		public int Count
		{
			get
			{
				if (this.m_itemIDs == null)
				{
					return 0;
				}
				return this.m_itemIDs.Length;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000DE45 File Offset: 0x0000CE45
		public void CopyTo(Array array, int index)
		{
			if (this.m_itemIDs != null)
			{
				this.m_itemIDs.CopyTo(array, index);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000DE5C File Offset: 0x0000CE5C
		public void CopyTo(ItemIdentifier[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000DE66 File Offset: 0x0000CE66
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000DE69 File Offset: 0x0000CE69
		public IEnumerator GetEnumerator()
		{
			return this.m_itemIDs.GetEnumerator();
		}

		// Token: 0x04000255 RID: 597
		private ItemIdentifier[] m_itemIDs = new ItemIdentifier[0];
	}
}
