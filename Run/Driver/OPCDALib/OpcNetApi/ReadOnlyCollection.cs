using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class ReadOnlyCollection : ICollection, IEnumerable, ICloneable, ISerializable
	{
		// Token: 0x1700000D RID: 13
		public virtual object this[int index]
		{
			get
			{
				return this.m_array.GetValue(index);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000044CA File Offset: 0x000034CA
		public virtual Array ToArray()
		{
			return (Array)Convert.Clone(this.m_array);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000044DC File Offset: 0x000034DC
		protected ReadOnlyCollection(Array array)
		{
			this.Array = array;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000044EB File Offset: 0x000034EB
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000044F3 File Offset: 0x000034F3
		protected virtual Array Array
		{
			get
			{
				return this.m_array;
			}
			set
			{
				this.m_array = value;
				if (this.m_array == null)
				{
					this.m_array = new object[0];
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004510 File Offset: 0x00003510
		protected ReadOnlyCollection(SerializationInfo info, StreamingContext context)
		{
			this.m_array = (Array)info.GetValue("AR", typeof(Array));
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004538 File Offset: 0x00003538
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AR", this.m_array);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000454B File Offset: 0x0000354B
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000454E File Offset: 0x0000354E
		public virtual int Count
		{
			get
			{
				return this.m_array.Length;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000455B File Offset: 0x0000355B
		public virtual void CopyTo(Array array, int index)
		{
			if (this.m_array != null)
			{
				this.m_array.CopyTo(array, index);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004572 File Offset: 0x00003572
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004575 File Offset: 0x00003575
		public virtual IEnumerator GetEnumerator()
		{
			return this.m_array.GetEnumerator();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004584 File Offset: 0x00003584
		public virtual object Clone()
		{
			ReadOnlyCollection readOnlyCollection = (ReadOnlyCollection)base.MemberwiseClone();
			ArrayList arrayList = new ArrayList(this.m_array.Length);
			System.Type type = null;
			for (int i = 0; i < this.m_array.Length; i++)
			{
				object value = this.m_array.GetValue(i);
				if (type == null)
				{
					type = value.GetType();
				}
				else if (type != typeof(object))
				{
					while (!type.IsInstanceOfType(value))
					{
						type = type.BaseType;
					}
				}
				arrayList.Add(Convert.Clone(value));
			}
			readOnlyCollection.Array = arrayList.ToArray(type);
			return readOnlyCollection;
		}

		// Token: 0x0400001A RID: 26
		private Array m_array;

		// Token: 0x02000010 RID: 16
		private class Names
		{
			// Token: 0x0400001B RID: 27
			internal const string ARRAY = "AR";
		}
	}
}
