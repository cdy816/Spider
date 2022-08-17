using System;
using System.Collections;

namespace Opc
{
	// Token: 0x0200009C RID: 156
	[Serializable]
	public class IdentifiedResultCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x170000FB RID: 251
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

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000DE8B File Offset: 0x0000CE8B
		public IdentifiedResultCollection()
		{
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000DE9F File Offset: 0x0000CE9F
		public IdentifiedResultCollection(ICollection collection)
		{
			this.Init(collection);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000DEBC File Offset: 0x0000CEBC
		public void Init(ICollection collection)
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

		// Token: 0x060004AA RID: 1194 RVA: 0x0000DF58 File Offset: 0x0000CF58
		public void Clear()
		{
			this.m_results = new IdentifiedResult[0];
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000DF66 File Offset: 0x0000CF66
		public virtual object Clone()
		{
			return new IdentifiedResultCollection(this);
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0000DF6E File Offset: 0x0000CF6E
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0000DF71 File Offset: 0x0000CF71
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

		// Token: 0x060004AE RID: 1198 RVA: 0x0000DF85 File Offset: 0x0000CF85
		public void CopyTo(Array array, int index)
		{
			if (this.m_results != null)
			{
				this.m_results.CopyTo(array, index);
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000DF9C File Offset: 0x0000CF9C
		public void CopyTo(IdentifiedResult[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0000DFA6 File Offset: 0x0000CFA6
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000DFA9 File Offset: 0x0000CFA9
		public IEnumerator GetEnumerator()
		{
			return this.m_results.GetEnumerator();
		}

		// Token: 0x04000256 RID: 598
		private IdentifiedResult[] m_results = new IdentifiedResult[0];
	}
}
