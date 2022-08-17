using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x02000030 RID: 48
	[Serializable]
	public class AggregateCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00005425 File Offset: 0x00004425
		public AggregateCollection()
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005439 File Offset: 0x00004439
		public AggregateCollection(ICollection collection)
		{
			this.Init(collection);
		}

		// Token: 0x17000019 RID: 25
		public Aggregate this[int index]
		{
			get
			{
				return this.m_aggregates[index];
			}
			set
			{
				this.m_aggregates[index] = value;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000546C File Offset: 0x0000446C
		public Aggregate Find(int id)
		{
			foreach (Aggregate aggregate in this.m_aggregates)
			{
				if (aggregate.ID == id)
				{
					return aggregate;
				}
			}
			return null;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000054A4 File Offset: 0x000044A4
		public void Init(ICollection collection)
		{
			this.Clear();
			if (collection != null)
			{
				ArrayList arrayList = new ArrayList(collection.Count);
				foreach (object obj in collection)
				{
					if (obj.GetType() == typeof(Aggregate))
					{
						arrayList.Add(Convert.Clone(obj));
					}
				}
				this.m_aggregates = (Aggregate[])arrayList.ToArray(typeof(Aggregate));
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000553C File Offset: 0x0000453C
		public void Clear()
		{
			this.m_aggregates = new Aggregate[0];
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000554A File Offset: 0x0000454A
		public virtual object Clone()
		{
			return new AggregateCollection(this);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005552 File Offset: 0x00004552
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005555 File Offset: 0x00004555
		public int Count
		{
			get
			{
				if (this.m_aggregates == null)
				{
					return 0;
				}
				return this.m_aggregates.Length;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005569 File Offset: 0x00004569
		public void CopyTo(Array array, int index)
		{
			if (this.m_aggregates != null)
			{
				this.m_aggregates.CopyTo(array, index);
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005580 File Offset: 0x00004580
		public void CopyTo(Aggregate[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000558A File Offset: 0x0000458A
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000558D File Offset: 0x0000458D
		public IEnumerator GetEnumerator()
		{
			return this.m_aggregates.GetEnumerator();
		}

		// Token: 0x040000AA RID: 170
		private Aggregate[] m_aggregates = new Aggregate[0];
	}
}
