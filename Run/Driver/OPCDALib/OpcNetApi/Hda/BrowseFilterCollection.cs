using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x0200004F RID: 79
	[Serializable]
	public class BrowseFilterCollection : ItemIdentifier, ICollection, IEnumerable
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00006576 File Offset: 0x00005576
		public BrowseFilterCollection()
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000658A File Offset: 0x0000558A
		public BrowseFilterCollection(ICollection collection)
		{
			this.Init(collection);
		}

		// Token: 0x17000050 RID: 80
		public BrowseFilter this[int index]
		{
			get
			{
				return this.m_filters[index];
			}
			set
			{
				this.m_filters[index] = value;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000065BC File Offset: 0x000055BC
		public BrowseFilter Find(int id)
		{
			foreach (BrowseFilter browseFilter in this.m_filters)
			{
				if (browseFilter.AttributeID == id)
				{
					return browseFilter;
				}
			}
			return null;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000065F4 File Offset: 0x000055F4
		public void Init(ICollection collection)
		{
			this.Clear();
			if (collection != null)
			{
				ArrayList arrayList = new ArrayList(collection.Count);
				foreach (object obj in collection)
				{
					if (obj.GetType() == typeof(BrowseFilter))
					{
						arrayList.Add(Convert.Clone(obj));
					}
				}
				this.m_filters = (BrowseFilter[])arrayList.ToArray(typeof(BrowseFilter));
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000668C File Offset: 0x0000568C
		public void Clear()
		{
			this.m_filters = new BrowseFilter[0];
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000669A File Offset: 0x0000569A
		public override object Clone()
		{
			return new BrowseFilterCollection(this);
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000066A2 File Offset: 0x000056A2
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000066A5 File Offset: 0x000056A5
		public int Count
		{
			get
			{
				if (this.m_filters == null)
				{
					return 0;
				}
				return this.m_filters.Length;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000066B9 File Offset: 0x000056B9
		public void CopyTo(Array array, int index)
		{
			if (this.m_filters != null)
			{
				this.m_filters.CopyTo(array, index);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000066D0 File Offset: 0x000056D0
		public void CopyTo(BrowseFilter[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000066DA File Offset: 0x000056DA
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000066DD File Offset: 0x000056DD
		public IEnumerator GetEnumerator()
		{
			return this.m_filters.GetEnumerator();
		}

		// Token: 0x040000FD RID: 253
		private BrowseFilter[] m_filters = new BrowseFilter[0];
	}
}
