using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000A9 RID: 169
	[Serializable]
	public class ItemValueCollection : Item, IResult, IActualTime, ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x17000132 RID: 306
		public ItemValue this[int index]
		{
			get
			{
				return (ItemValue)this.m_values[index];
			}
			set
			{
				this.m_values[index] = value;
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000FC7A File Offset: 0x0000EC7A
		public ItemValueCollection()
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0000FCAE File Offset: 0x0000ECAE
		public ItemValueCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0000FCE3 File Offset: 0x0000ECE3
		public ItemValueCollection(Item item) : base(item)
		{
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0000FD18 File Offset: 0x0000ED18
		public ItemValueCollection(ItemValueCollection item) : base(item)
		{
			this.m_values = new ArrayList(item.m_values.Count);
			foreach (object obj in item.m_values)
			{
				ItemValue itemValue = (ItemValue)obj;
				if (itemValue != null)
				{
					this.m_values.Add(itemValue.Clone());
				}
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0000FDC8 File Offset: 0x0000EDC8
		public void AddRange(ItemValueCollection collection)
		{
			if (collection != null)
			{
				foreach (object obj in collection)
				{
					ItemValue itemValue = (ItemValue)obj;
					if (itemValue != null)
					{
						this.m_values.Add(itemValue.Clone());
					}
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000FE30 File Offset: 0x0000EE30
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x0000FE38 File Offset: 0x0000EE38
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0000FE41 File Offset: 0x0000EE41
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0000FE49 File Offset: 0x0000EE49
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0000FE52 File Offset: 0x0000EE52
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x0000FE5A File Offset: 0x0000EE5A
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000FE63 File Offset: 0x0000EE63
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x0000FE6B File Offset: 0x0000EE6B
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

		// Token: 0x0600059C RID: 1436 RVA: 0x0000FE74 File Offset: 0x0000EE74
		public override object Clone()
		{
			ItemValueCollection itemValueCollection = (ItemValueCollection)base.Clone();
			itemValueCollection.m_values = new ArrayList(this.m_values.Count);
			foreach (object obj in this.m_values)
			{
				ItemValue itemValue = (ItemValue)obj;
				itemValueCollection.m_values.Add(itemValue.Clone());
			}
			return itemValueCollection;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0000FEFC File Offset: 0x0000EEFC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0000FEFF File Offset: 0x0000EEFF
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

		// Token: 0x0600059F RID: 1439 RVA: 0x0000FF16 File Offset: 0x0000EF16
		public void CopyTo(Array array, int index)
		{
			if (this.m_values != null)
			{
				this.m_values.CopyTo(array, index);
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000FF2D File Offset: 0x0000EF2D
		public void CopyTo(ItemValue[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0000FF37 File Offset: 0x0000EF37
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0000FF3A File Offset: 0x0000EF3A
		public IEnumerator GetEnumerator()
		{
			return this.m_values.GetEnumerator();
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0000FF47 File Offset: 0x0000EF47
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700013B RID: 315
		object IList.this[int index]
		{
			get
			{
				return this.m_values[index];
			}
			set
			{
				if (!typeof(ItemValue).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add ItemValue objects into the collection.");
				}
				this.m_values[index] = value;
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0000FF84 File Offset: 0x0000EF84
		public void RemoveAt(int index)
		{
			this.m_values.RemoveAt(index);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0000FF92 File Offset: 0x0000EF92
		public void Insert(int index, object value)
		{
			if (!typeof(ItemValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add ItemValue objects into the collection.");
			}
			this.m_values.Insert(index, value);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000FFBE File Offset: 0x0000EFBE
		public void Remove(object value)
		{
			this.m_values.Remove(value);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0000FFCC File Offset: 0x0000EFCC
		public bool Contains(object value)
		{
			return this.m_values.Contains(value);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0000FFDA File Offset: 0x0000EFDA
		public void Clear()
		{
			this.m_values.Clear();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0000FFE7 File Offset: 0x0000EFE7
		public int IndexOf(object value)
		{
			return this.m_values.IndexOf(value);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0000FFF5 File Offset: 0x0000EFF5
		public int Add(object value)
		{
			if (!typeof(ItemValue).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add ItemValue objects into the collection.");
			}
			return this.m_values.Add(value);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00010020 File Offset: 0x0000F020
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00010023 File Offset: 0x0000F023
		public void Insert(int index, ItemValue value)
		{
			this.Insert(index, value);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001002D File Offset: 0x0000F02D
		public void Remove(ItemValue value)
		{
			this.Remove(value);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00010036 File Offset: 0x0000F036
		public bool Contains(ItemValue value)
		{
			return this.Contains(value);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001003F File Offset: 0x0000F03F
		public int IndexOf(ItemValue value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00010048 File Offset: 0x0000F048
		public int Add(ItemValue value)
		{
			return this.Add(value);
		}

		// Token: 0x04000292 RID: 658
		private DateTime m_startTime = DateTime.MinValue;

		// Token: 0x04000293 RID: 659
		private DateTime m_endTime = DateTime.MinValue;

		// Token: 0x04000294 RID: 660
		private ArrayList m_values = new ArrayList();

		// Token: 0x04000295 RID: 661
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x04000296 RID: 662
		private string m_diagnosticInfo;
	}
}
