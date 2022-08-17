using System;
using System.Collections;

namespace Opc.Dx
{
	// Token: 0x02000033 RID: 51
	public class DXConnectionQueryCollection : ICloneable, IList, ICollection, IEnumerable
	{
		// Token: 0x17000021 RID: 33
		public DXConnectionQuery this[int index]
		{
			get
			{
				return (DXConnectionQuery)this.m_queries[index];
			}
		}

		// Token: 0x17000022 RID: 34
		public DXConnectionQuery this[string name]
		{
			get
			{
				foreach (object obj in this.m_queries)
				{
					DXConnectionQuery dxconnectionQuery = (DXConnectionQuery)obj;
					if (dxconnectionQuery.Name == name)
					{
						return dxconnectionQuery;
					}
				}
				return null;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000057C0 File Offset: 0x000047C0
		internal DXConnectionQueryCollection()
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000057D4 File Offset: 0x000047D4
		internal void Initialize(ICollection queries)
		{
			this.m_queries.Clear();
			if (queries != null)
			{
				foreach (object obj in queries)
				{
					DXConnectionQuery value = (DXConnectionQuery)obj;
					this.m_queries.Add(value);
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000583C File Offset: 0x0000483C
		public virtual object Clone()
		{
			DXConnectionQueryCollection dxconnectionQueryCollection = (DXConnectionQueryCollection)base.MemberwiseClone();
			dxconnectionQueryCollection.m_queries = new ArrayList();
			foreach (object obj in this.m_queries)
			{
				DXConnectionQuery dxconnectionQuery = (DXConnectionQuery)obj;
				dxconnectionQueryCollection.m_queries.Add(dxconnectionQuery.Clone());
			}
			return dxconnectionQueryCollection;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000058B8 File Offset: 0x000048B8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000058BB File Offset: 0x000048BB
		public int Count
		{
			get
			{
				if (this.m_queries == null)
				{
					return 0;
				}
				return this.m_queries.Count;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000058D2 File Offset: 0x000048D2
		public void CopyTo(Array array, int index)
		{
			if (this.m_queries != null)
			{
				this.m_queries.CopyTo(array, index);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000058E9 File Offset: 0x000048E9
		public void CopyTo(DXConnectionQuery[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000058F3 File Offset: 0x000048F3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000058F6 File Offset: 0x000048F6
		public IEnumerator GetEnumerator()
		{
			return this.m_queries.GetEnumerator();
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00005903 File Offset: 0x00004903
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		object IList.this[int index]
		{
			get
			{
				return this.m_queries[index];
			}
			set
			{
				this.Insert(index, value);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000591E File Offset: 0x0000491E
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.m_queries.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.m_queries[index]);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000594F File Offset: 0x0000494F
		public void Insert(int index, object value)
		{
			if (!typeof(DXConnectionQuery).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add DXConnectionQuery objects into the collection.");
			}
			this.m_queries.Insert(index, value);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000597B File Offset: 0x0000497B
		public void Remove(object value)
		{
			if (!typeof(DXConnectionQuery).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only delete DXConnectionQuery obejcts from the collection.");
			}
			this.m_queries.Remove(value);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000059A8 File Offset: 0x000049A8
		public bool Contains(object value)
		{
			foreach (object obj in this.m_queries)
			{
				ItemIdentifier itemIdentifier = (ItemIdentifier)obj;
				if (itemIdentifier.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005A0C File Offset: 0x00004A0C
		public void Clear()
		{
			this.m_queries.Clear();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005A19 File Offset: 0x00004A19
		public int IndexOf(object value)
		{
			return this.m_queries.IndexOf(value);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005A27 File Offset: 0x00004A27
		public int Add(object value)
		{
			this.Insert(this.m_queries.Count, value);
			return this.m_queries.Count - 1;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005A48 File Offset: 0x00004A48
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005A4B File Offset: 0x00004A4B
		public void Insert(int index, DXConnectionQuery value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005A55 File Offset: 0x00004A55
		public void Remove(DXConnectionQuery value)
		{
			this.Remove(value);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005A5E File Offset: 0x00004A5E
		public bool Contains(DXConnectionQuery value)
		{
			return this.Contains(value);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005A67 File Offset: 0x00004A67
		public int IndexOf(DXConnectionQuery value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005A70 File Offset: 0x00004A70
		public int Add(DXConnectionQuery value)
		{
			return this.Add(value);
		}

		// Token: 0x040000C8 RID: 200
		private ArrayList m_queries = new ArrayList();
	}
}
