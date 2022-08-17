using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000DC RID: 220
	[Serializable]
	public class ItemAttributeCollection : ItemIdentifier, IResult, IActualTime, IList, ICollection, IEnumerable
	{
		// Token: 0x170001C6 RID: 454
		public AttributeValueCollection this[int index]
		{
			get
			{
				return (AttributeValueCollection)this.m_attributes[index];
			}
			set
			{
				this.m_attributes[index] = value;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00012693 File Offset: 0x00011693
		public ItemAttributeCollection()
		{
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000126C7 File Offset: 0x000116C7
		public ItemAttributeCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000126FC File Offset: 0x000116FC
		public ItemAttributeCollection(ItemAttributeCollection item) : base(item)
		{
			this.m_attributes = new ArrayList(item.m_attributes.Count);
			foreach (object obj in item.m_attributes)
			{
				AttributeValueCollection attributeValueCollection = (AttributeValueCollection)obj;
				if (attributeValueCollection != null)
				{
					this.m_attributes.Add(attributeValueCollection.Clone());
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x000127AC File Offset: 0x000117AC
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x000127B4 File Offset: 0x000117B4
		public ResultID ResultID
		{
			get
			{
				return this.m_resultID;
			}
			set
			{
				this.m_resultID = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x000127BD File Offset: 0x000117BD
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x000127C5 File Offset: 0x000117C5
		public string DiagnosticInfo
		{
			get
			{
				return this.m_diagnosticInfo;
			}
			set
			{
				this.m_diagnosticInfo = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000127CE File Offset: 0x000117CE
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x000127D6 File Offset: 0x000117D6
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000127DF File Offset: 0x000117DF
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x000127E7 File Offset: 0x000117E7
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

		// Token: 0x06000787 RID: 1927 RVA: 0x000127F0 File Offset: 0x000117F0
		public override object Clone()
		{
			ItemAttributeCollection itemAttributeCollection = (ItemAttributeCollection)base.Clone();
			itemAttributeCollection.m_attributes = new ArrayList(this.m_attributes.Count);
			foreach (object obj in this.m_attributes)
			{
				AttributeValueCollection attributeValueCollection = (AttributeValueCollection)obj;
				itemAttributeCollection.m_attributes.Add(attributeValueCollection.Clone());
			}
			return itemAttributeCollection;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00012878 File Offset: 0x00011878
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001287B File Offset: 0x0001187B
		public int Count
		{
			get
			{
				if (this.m_attributes == null)
				{
					return 0;
				}
				return this.m_attributes.Count;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00012892 File Offset: 0x00011892
		public void CopyTo(Array array, int index)
		{
			if (this.m_attributes != null)
			{
				this.m_attributes.CopyTo(array, index);
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000128A9 File Offset: 0x000118A9
		public void CopyTo(AttributeValueCollection[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000128B3 File Offset: 0x000118B3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000128B6 File Offset: 0x000118B6
		public IEnumerator GetEnumerator()
		{
			return this.m_attributes.GetEnumerator();
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000128C3 File Offset: 0x000118C3
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CF RID: 463
		object IList.this[int index]
		{
			get
			{
				return this.m_attributes[index];
			}
			set
			{
				if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
				}
				this.m_attributes[index] = value;
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00012900 File Offset: 0x00011900
		public void RemoveAt(int index)
		{
			this.m_attributes.RemoveAt(index);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001290E File Offset: 0x0001190E
		public void Insert(int index, object value)
		{
			if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
			}
			this.m_attributes.Insert(index, value);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001293A File Offset: 0x0001193A
		public void Remove(object value)
		{
			this.m_attributes.Remove(value);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00012948 File Offset: 0x00011948
		public bool Contains(object value)
		{
			return this.m_attributes.Contains(value);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00012956 File Offset: 0x00011956
		public void Clear()
		{
			this.m_attributes.Clear();
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00012963 File Offset: 0x00011963
		public int IndexOf(object value)
		{
			return this.m_attributes.IndexOf(value);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00012971 File Offset: 0x00011971
		public int Add(object value)
		{
			if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
			}
			return this.m_attributes.Add(value);
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001299C File Offset: 0x0001199C
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001299F File Offset: 0x0001199F
		public void Insert(int index, AttributeValueCollection value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000129A9 File Offset: 0x000119A9
		public void Remove(AttributeValueCollection value)
		{
			this.Remove(value);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000129B2 File Offset: 0x000119B2
		public bool Contains(AttributeValueCollection value)
		{
			return this.Contains(value);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000129BB File Offset: 0x000119BB
		public int IndexOf(AttributeValueCollection value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000129C4 File Offset: 0x000119C4
		public int Add(AttributeValueCollection value)
		{
			return this.Add(value);
		}

		// Token: 0x04000351 RID: 849
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x04000352 RID: 850
		private DateTime m_endTime = DateTime.MinValue;

		// Token: 0x04000353 RID: 851
		private ArrayList m_attributes = new ArrayList();

		// Token: 0x04000354 RID: 852
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000355 RID: 853
		private string m_diagnosticInfo;
	}
}
