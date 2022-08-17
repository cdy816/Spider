using System;
using System.Collections;

namespace Opc.Hda
{
	// Token: 0x020000D9 RID: 217
	[Serializable]
	public class AttributeCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x00012188 File Offset: 0x00011188
		public AttributeCollection()
		{
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001219C File Offset: 0x0001119C
		public AttributeCollection(ICollection collection)
		{
			this.Init(collection);
		}

		// Token: 0x170001B6 RID: 438
		public Attribute this[int index]
		{
			get
			{
				return this.m_attributes[index];
			}
			set
			{
				this.m_attributes[index] = value;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000121CC File Offset: 0x000111CC
		public Attribute Find(int id)
		{
			foreach (Attribute attribute in this.m_attributes)
			{
				if (attribute.ID == id)
				{
					return attribute;
				}
			}
			return null;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00012204 File Offset: 0x00011204
		public void Init(ICollection collection)
		{
			this.Clear();
			if (collection != null)
			{
				ArrayList arrayList = new ArrayList(collection.Count);
				foreach (object obj in collection)
				{
					if (obj.GetType() == typeof(Attribute))
					{
						arrayList.Add(Convert.Clone(obj));
					}
				}
				this.m_attributes = (Attribute[])arrayList.ToArray(typeof(Attribute));
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001229C File Offset: 0x0001129C
		public void Clear()
		{
			this.m_attributes = new Attribute[0];
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000122AA File Offset: 0x000112AA
		public virtual object Clone()
		{
			return new AttributeCollection(this);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x000122B2 File Offset: 0x000112B2
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x000122B5 File Offset: 0x000112B5
		public int Count
		{
			get
			{
				if (this.m_attributes == null)
				{
					return 0;
				}
				return this.m_attributes.Length;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000122C9 File Offset: 0x000112C9
		public void CopyTo(Array array, int index)
		{
			if (this.m_attributes != null)
			{
				this.m_attributes.CopyTo(array, index);
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000122E0 File Offset: 0x000112E0
		public void CopyTo(Attribute[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x000122EA File Offset: 0x000112EA
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000122ED File Offset: 0x000112ED
		public IEnumerator GetEnumerator()
		{
			return this.m_attributes.GetEnumerator();
		}

		// Token: 0x0400034A RID: 842
		private Attribute[] m_attributes = new Attribute[0];
	}
}
