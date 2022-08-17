using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000AE RID: 174
	[Serializable]
	public class ResultCollection : ItemIdentifier, ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x17000143 RID: 323
		public Result this[int index]
		{
			get
			{
				return (Result)this.m_results[index];
			}
			set
			{
				this.m_results[index] = value;
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001017F File Offset: 0x0000F17F
		public ResultCollection()
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00010192 File Offset: 0x0000F192
		public ResultCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000101A8 File Offset: 0x0000F1A8
		public ResultCollection(ResultCollection item) : base(item)
		{
			this.m_results = new ArrayList(item.m_results.Count);
			foreach (object obj in item.m_results)
			{
				Result result = (Result)obj;
				this.m_results.Add(result.Clone());
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00010234 File Offset: 0x0000F234
		public override object Clone()
		{
			ResultCollection resultCollection = (ResultCollection)base.Clone();
			resultCollection.m_results = new ArrayList(this.m_results.Count);
			foreach (object obj in this.m_results)
			{
				ResultCollection resultCollection2 = (ResultCollection)obj;
				resultCollection.m_results.Add(resultCollection2.Clone());
			}
			return resultCollection;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000102BC File Offset: 0x0000F2BC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000102BF File Offset: 0x0000F2BF
		public int Count
		{
			get
			{
				if (this.m_results == null)
				{
					return 0;
				}
				return this.m_results.Count;
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000102D6 File Offset: 0x0000F2D6
		public void CopyTo(Array array, int index)
		{
			if (this.m_results != null)
			{
				this.m_results.CopyTo(array, index);
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000102ED File Offset: 0x0000F2ED
		public void CopyTo(Result[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x000102F7 File Offset: 0x0000F2F7
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000102FA File Offset: 0x0000F2FA
		public IEnumerator GetEnumerator()
		{
			return this.m_results.GetEnumerator();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00010307 File Offset: 0x0000F307
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000148 RID: 328
		object IList.this[int index]
		{
			get
			{
				return this.m_results[index];
			}
			set
			{
				if (!typeof(Result).IsInstanceOfType(value))
				{
					throw new ArgumentException("May only add Result objects into the collection.");
				}
				this.m_results[index] = value;
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00010344 File Offset: 0x0000F344
		public void RemoveAt(int index)
		{
			this.m_results.RemoveAt(index);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00010352 File Offset: 0x0000F352
		public void Insert(int index, object value)
		{
			if (!typeof(Result).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Result objects into the collection.");
			}
			this.m_results.Insert(index, value);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001037E File Offset: 0x0000F37E
		public void Remove(object value)
		{
			this.m_results.Remove(value);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001038C File Offset: 0x0000F38C
		public bool Contains(object value)
		{
			return this.m_results.Contains(value);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001039A File Offset: 0x0000F39A
		public void Clear()
		{
			this.m_results.Clear();
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000103A7 File Offset: 0x0000F3A7
		public int IndexOf(object value)
		{
			return this.m_results.IndexOf(value);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000103B5 File Offset: 0x0000F3B5
		public int Add(object value)
		{
			if (!typeof(Result).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add Result objects into the collection.");
			}
			return this.m_results.Add(value);
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x000103E0 File Offset: 0x0000F3E0
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000103E3 File Offset: 0x0000F3E3
		public void Insert(int index, Result value)
		{
			this.Insert(index, value);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000103ED File Offset: 0x0000F3ED
		public void Remove(Result value)
		{
			this.Remove(value);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000103F6 File Offset: 0x0000F3F6
		public bool Contains(Result value)
		{
			return this.Contains(value);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000103FF File Offset: 0x0000F3FF
		public int IndexOf(Result value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00010408 File Offset: 0x0000F408
		public int Add(Result value)
		{
			return this.Add(value);
		}

		// Token: 0x040002A1 RID: 673
		private ArrayList m_results = new ArrayList();
	}
}
