using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000DB RID: 219
	[Serializable]
	public class AttributeValueCollection : IResult, ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0001235A File Offset: 0x0001135A
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00012362 File Offset: 0x00011362
		public int AttributeID
		{
			get
			{
				return this.m_attributeID;
			}
			set
			{
				this.m_attributeID = value;
			}
		}

		// Token: 0x170001BD RID: 445
		public AttributeValue this[int index]
		{
			get
			{
				return (AttributeValue)this.m_values[index];
			}
			set
			{
				this.m_values[index] = value;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001238D File Offset: 0x0001138D
		public AttributeValueCollection()
		{
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000123AB File Offset: 0x000113AB
		public AttributeValueCollection(Attribute attribute)
		{
			this.m_attributeID = attribute.ID;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000123D8 File Offset: 0x000113D8
		public AttributeValueCollection(AttributeValueCollection collection)
		{
			this.m_values = new ArrayList(collection.m_values.Count);
			foreach (object obj in collection.m_values)
			{
				AttributeValue attributeValue = (AttributeValue)obj;
				this.m_values.Add(attributeValue.Clone());
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00012470 File Offset: 0x00011470
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x00012478 File Offset: 0x00011478
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00012481 File Offset: 0x00011481
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x00012489 File Offset: 0x00011489
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

		// Token: 0x06000763 RID: 1891 RVA: 0x00012494 File Offset: 0x00011494
		public virtual object Clone()
		{
			AttributeValueCollection attributeValueCollection = (AttributeValueCollection)base.MemberwiseClone();
			attributeValueCollection.m_values = new ArrayList(this.m_values.Count);
			foreach (object obj in this.m_values)
			{
				AttributeValue attributeValue = (AttributeValue)obj;
				attributeValueCollection.m_values.Add(attributeValue.Clone());
			}
			return attributeValueCollection;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001251C File Offset: 0x0001151C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0001251F File Offset: 0x0001151F
		public int Count
		{
			get
			{
				if (this.m_values == null)
				{
					return 0;
				}
				return this.m_values.Count;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00012536 File Offset: 0x00011536
		public void CopyTo(Array array, int index)
		{
			if (this.m_values != null)
			{
				this.m_values.CopyTo(array, index);
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001254D File Offset: 0x0001154D
		public void CopyTo(AttributeValue[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00012557 File Offset: 0x00011557
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001255A File Offset: 0x0001155A
		public IEnumerator GetEnumerator()
		{
			return this.m_values.GetEnumerator();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00012567 File Offset: 0x00011567
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001C4 RID: 452
		object IList.this[int index]
		{
			get
			{
				return this.m_values[index];
			}
			set
			{
				if (!typeof(AttributeValue).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add AttributeValue objects into the collection.");
				}
				this.m_values[index] = value;
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000125A4 File Offset: 0x000115A4
		public void RemoveAt(int index)
		{
			this.m_values.RemoveAt(index);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000125B2 File Offset: 0x000115B2
		public void Insert(int index, object value)
		{
			if (!typeof(AttributeValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AttributeValue objects into the collection.");
			}
			this.m_values.Insert(index, value);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000125DE File Offset: 0x000115DE
		public void Remove(object value)
		{
			this.m_values.Remove(value);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000125EC File Offset: 0x000115EC
		public bool Contains(object value)
		{
			return this.m_values.Contains(value);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000125FA File Offset: 0x000115FA
		public void Clear()
		{
			this.m_values.Clear();
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00012607 File Offset: 0x00011607
		public int IndexOf(object value)
		{
			return this.m_values.IndexOf(value);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00012615 File Offset: 0x00011615
		public int Add(object value)
		{
			if (!typeof(AttributeValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AttributeValue objects into the collection.");
			}
			return this.m_values.Add(value);
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00012640 File Offset: 0x00011640
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00012643 File Offset: 0x00011643
		public void Insert(int index, AttributeValue value)
		{
			this.Insert(index, value);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001264D File Offset: 0x0001164D
		public void Remove(AttributeValue value)
		{
			this.Remove(value);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00012656 File Offset: 0x00011656
		public bool Contains(AttributeValue value)
		{
			return this.Contains(value);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001265F File Offset: 0x0001165F
		public int IndexOf(AttributeValue value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00012668 File Offset: 0x00011668
		public int Add(AttributeValue value)
		{
			return this.Add(value);
		}

		// Token: 0x0400034D RID: 845
		private int m_attributeID;

		// Token: 0x0400034E RID: 846
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x0400034F RID: 847
		private string m_diagnosticInfo;

		// Token: 0x04000350 RID: 848
		private ArrayList m_values = new ArrayList();
	}
}
