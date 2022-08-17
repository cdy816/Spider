using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x02000054 RID: 84
	[Serializable]
	public class AnnotationValueCollection : Item, IResult, IActualTime, ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x1700005D RID: 93
		public AnnotationValue this[int index]
		{
			get
			{
				return (AnnotationValue)this.m_values[index];
			}
			set
			{
				this.m_values[index] = value;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000067B0 File Offset: 0x000057B0
		public AnnotationValueCollection()
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000067E4 File Offset: 0x000057E4
		public AnnotationValueCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006819 File Offset: 0x00005819
		public AnnotationValueCollection(Item item) : base(item)
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00006850 File Offset: 0x00005850
		public AnnotationValueCollection(AnnotationValueCollection item) : base(item)
		{
			this.m_values = new ArrayList(item.m_values.Count);
			foreach (object obj in item.m_values)
			{
				ItemValue itemValue = (ItemValue)obj;
				this.m_values.Add(itemValue.Clone());
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00006900 File Offset: 0x00005900
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00006908 File Offset: 0x00005908
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00006911 File Offset: 0x00005911
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00006919 File Offset: 0x00005919
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00006922 File Offset: 0x00005922
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000692A File Offset: 0x0000592A
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00006933 File Offset: 0x00005933
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000693B File Offset: 0x0000593B
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

		// Token: 0x0600020C RID: 524 RVA: 0x00006944 File Offset: 0x00005944
		public override object Clone()
		{
			AnnotationValueCollection annotationValueCollection = (AnnotationValueCollection)base.Clone();
			annotationValueCollection.m_values = new ArrayList(this.m_values.Count);
			foreach (object obj in this.m_values)
			{
				AnnotationValue annotationValue = (AnnotationValue)obj;
				annotationValueCollection.m_values.Add(annotationValue.Clone());
			}
			return annotationValueCollection;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600020D RID: 525 RVA: 0x000069CC File Offset: 0x000059CC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000069CF File Offset: 0x000059CF
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

		// Token: 0x0600020F RID: 527 RVA: 0x000069E6 File Offset: 0x000059E6
		public void CopyTo(Array array, int index)
		{
			if (this.m_values != null)
			{
				this.m_values.CopyTo(array, index);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000069FD File Offset: 0x000059FD
		public void CopyTo(AnnotationValue[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00006A07 File Offset: 0x00005A07
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00006A0A File Offset: 0x00005A0A
		public IEnumerator GetEnumerator()
		{
			return this.m_values.GetEnumerator();
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00006A17 File Offset: 0x00005A17
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000066 RID: 102
		object IList.this[int index]
		{
			get
			{
				return this.m_values[index];
			}
			set
			{
				if (!typeof(AnnotationValue).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add AnnotationValue objects into the collection.");
				}
				this.m_values[index] = value;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00006A54 File Offset: 0x00005A54
		public void RemoveAt(int index)
		{
			this.m_values.RemoveAt(index);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00006A62 File Offset: 0x00005A62
		public void Insert(int index, object value)
		{
			if (!typeof(AnnotationValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AnnotationValue objects into the collection.");
			}
			this.m_values.Insert(index, value);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006A8E File Offset: 0x00005A8E
		public void Remove(object value)
		{
			this.m_values.Remove(value);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006A9C File Offset: 0x00005A9C
		public bool Contains(object value)
		{
			return this.m_values.Contains(value);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00006AAA File Offset: 0x00005AAA
		public void Clear()
		{
			this.m_values.Clear();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00006AB7 File Offset: 0x00005AB7
		public int IndexOf(object value)
		{
			return this.m_values.IndexOf(value);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006AC5 File Offset: 0x00005AC5
		public int Add(object value)
		{
			if (!typeof(AnnotationValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add AnnotationValue objects into the collection.");
			}
			return this.m_values.Add(value);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00006AF0 File Offset: 0x00005AF0
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006AF3 File Offset: 0x00005AF3
		public void Insert(int index, AnnotationValue value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006AFD File Offset: 0x00005AFD
		public void Remove(AnnotationValue value)
		{
			this.Remove(value);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00006B06 File Offset: 0x00005B06
		public bool Contains(AnnotationValue value)
		{
			return this.Contains(value);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006B0F File Offset: 0x00005B0F
		public int IndexOf(AnnotationValue value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00006B18 File Offset: 0x00005B18
		public int Add(AnnotationValue value)
		{
			return this.Add(value);
		}

		// Token: 0x04000103 RID: 259
		private ArrayList m_values = new ArrayList();

		// Token: 0x04000104 RID: 260
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x04000105 RID: 261
		private DateTime m_endTime = DateTime.MinValue;

		// Token: 0x04000106 RID: 262
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000107 RID: 263
		private string m_diagnosticInfo;
	}
}
