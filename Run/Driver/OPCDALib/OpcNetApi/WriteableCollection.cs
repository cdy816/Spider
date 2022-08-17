using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class WriteableCollection : IList, ICollection, IEnumerable, ICloneable, ISerializable
	{
		// Token: 0x17000034 RID: 52
		public virtual object this[int index]
		{
			get
			{
				return this.m_array[index];
			}
			set
			{
				this.m_array[index] = value;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005C08 File Offset: 0x00004C08
		public virtual Array ToArray()
		{
			return this.m_array.ToArray(this.m_elementType);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005C1C File Offset: 0x00004C1C
		public virtual void AddRange(ICollection collection)
		{
			if (collection != null)
			{
				foreach (object element in collection)
				{
					this.ValidateElement(element);
				}
				this.m_array.AddRange(collection);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005C7C File Offset: 0x00004C7C
		protected WriteableCollection(ICollection array, System.Type elementType)
		{
			if (array != null)
			{
				this.m_array = new ArrayList(array);
			}
			else
			{
				this.m_array = new ArrayList();
			}
			this.m_elementType = typeof(object);
			if (elementType != null)
			{
				foreach (object element in this.m_array)
				{
					this.ValidateElement(element);
				}
				this.m_elementType = elementType;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00005D0C File Offset: 0x00004D0C
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00005D14 File Offset: 0x00004D14
		protected virtual ArrayList Array
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
					this.m_array = new ArrayList();
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00005D30 File Offset: 0x00004D30
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00005D38 File Offset: 0x00004D38
		protected virtual System.Type ElementType
		{
			get
			{
				return this.m_elementType;
			}
			set
			{
				foreach (object element in this.m_array)
				{
					this.ValidateElement(element);
				}
				this.m_elementType = value;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005D94 File Offset: 0x00004D94
		protected virtual void ValidateElement(object element)
		{
			if (element == null)
			{
				throw new ArgumentException(string.Format("The value '{0}' cannot be added to the collection.", element));
			}
			if (!this.m_elementType.IsInstanceOfType(element))
			{
				throw new ArgumentException(string.Format("A value with type '{0}' cannot be added to the collection.", element.GetType()));
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005DD0 File Offset: 0x00004DD0
		protected WriteableCollection(SerializationInfo info, StreamingContext context)
		{
			this.m_elementType = (System.Type)info.GetValue("ET", typeof(Type));
			int num = (int)info.GetValue("CT", typeof(int));
			this.m_array = new ArrayList(num);
			for (int i = 0; i < num; i++)
			{
				this.m_array.Add(info.GetValue("EL" + i.ToString(), typeof(object)));
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005E64 File Offset: 0x00004E64
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ET", this.m_elementType);
			info.AddValue("CT", this.m_array.Count);
			for (int i = 0; i < this.m_array.Count; i++)
			{
				info.AddValue("EL" + i.ToString(), this.m_array[i]);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00005ED1 File Offset: 0x00004ED1
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005ED4 File Offset: 0x00004ED4
		public virtual int Count
		{
			get
			{
				return this.m_array.Count;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005EE1 File Offset: 0x00004EE1
		public virtual void CopyTo(Array array, int index)
		{
			if (this.m_array != null)
			{
				this.m_array.CopyTo(array, index);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00005EF8 File Offset: 0x00004EF8
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005EFB File Offset: 0x00004EFB
		public IEnumerator GetEnumerator()
		{
			return this.m_array.GetEnumerator();
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00005F08 File Offset: 0x00004F08
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003B RID: 59
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005F1E File Offset: 0x00004F1E
		public virtual void RemoveAt(int index)
		{
			this.m_array.RemoveAt(index);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005F2C File Offset: 0x00004F2C
		public virtual void Insert(int index, object value)
		{
			this.ValidateElement(value);
			this.m_array.Insert(index, value);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005F42 File Offset: 0x00004F42
		public virtual void Remove(object value)
		{
			this.m_array.Remove(value);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005F50 File Offset: 0x00004F50
		public virtual bool Contains(object value)
		{
			return this.m_array.Contains(value);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005F5E File Offset: 0x00004F5E
		public virtual void Clear()
		{
			this.m_array.Clear();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005F6B File Offset: 0x00004F6B
		public virtual int IndexOf(object value)
		{
			return this.m_array.IndexOf(value);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005F79 File Offset: 0x00004F79
		public virtual int Add(object value)
		{
			this.ValidateElement(value);
			return this.m_array.Add(value);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005F8E File Offset: 0x00004F8E
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005F94 File Offset: 0x00004F94
		public virtual object Clone()
		{
			WriteableCollection writeableCollection = (WriteableCollection)base.MemberwiseClone();
			writeableCollection.m_array = new ArrayList();
			for (int i = 0; i < this.m_array.Count; i++)
			{
				writeableCollection.Add(Convert.Clone(this.m_array[i]));
			}
			return writeableCollection;
		}

		// Token: 0x040000D4 RID: 212
		protected const string INVALID_VALUE = "The value '{0}' cannot be added to the collection.";

		// Token: 0x040000D5 RID: 213
		protected const string INVALID_TYPE = "A value with type '{0}' cannot be added to the collection.";

		// Token: 0x040000D6 RID: 214
		private ArrayList m_array;

		// Token: 0x040000D7 RID: 215
		private System.Type m_elementType;

		// Token: 0x02000036 RID: 54
		private class Names
		{
			// Token: 0x040000D8 RID: 216
			internal const string COUNT = "CT";

			// Token: 0x040000D9 RID: 217
			internal const string ELEMENT = "EL";

			// Token: 0x040000DA RID: 218
			internal const string ELEMENT_TYPE = "ET";
		}
	}
}
