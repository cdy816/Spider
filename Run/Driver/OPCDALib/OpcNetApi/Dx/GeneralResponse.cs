using System;
using System.Collections;

namespace Opc.Dx
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	public class GeneralResponse : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000A608 File Offset: 0x00009608
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000A610 File Offset: 0x00009610
		public string Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				this.m_version = value;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000A619 File Offset: 0x00009619
		public GeneralResponse()
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000A62D File Offset: 0x0000962D
		public GeneralResponse(string version, ICollection results)
		{
			this.Version = version;
			this.Init(results);
		}

		// Token: 0x170000BE RID: 190
		public IdentifiedResult this[int index]
		{
			get
			{
				return this.m_results[index];
			}
			set
			{
				this.m_results[index] = value;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000A664 File Offset: 0x00009664
		public void Clear()
		{
			this.m_results = new IdentifiedResult[0];
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000A672 File Offset: 0x00009672
		public virtual object Clone()
		{
			return new IdentifiedResultCollection(this);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000A67A File Offset: 0x0000967A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000A67D File Offset: 0x0000967D
		public int Count
		{
			get
			{
				if (this.m_results == null)
				{
					return 0;
				}
				return this.m_results.Length;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000A691 File Offset: 0x00009691
		public void CopyTo(Array array, int index)
		{
			if (this.m_results != null)
			{
				this.m_results.CopyTo(array, index);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000A6A8 File Offset: 0x000096A8
		public void CopyTo(IdentifiedResult[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000A6B2 File Offset: 0x000096B2
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000A6B5 File Offset: 0x000096B5
		public IEnumerator GetEnumerator()
		{
			return this.m_results.GetEnumerator();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000A6C4 File Offset: 0x000096C4
		private void Init(ICollection collection)
		{
			this.Clear();
			if (collection != null)
			{
				ArrayList arrayList = new ArrayList(collection.Count);
				foreach (object obj in collection)
				{
					if (typeof(IdentifiedResult).IsInstanceOfType(obj))
					{
						arrayList.Add(((IdentifiedResult)obj).Clone());
					}
				}
				this.m_results = (IdentifiedResult[])arrayList.ToArray(typeof(IdentifiedResult));
			}
		}

		// Token: 0x040001AE RID: 430
		private string m_version;

		// Token: 0x040001AF RID: 431
		private IdentifiedResult[] m_results = new IdentifiedResult[0];
	}
}
