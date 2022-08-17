using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000076 RID: 118
	[Serializable]
	public class ReadOnlyDictionary : IDictionary, ICollection, IEnumerable, ISerializable
	{
		// Token: 0x060002FB RID: 763 RVA: 0x00008052 File Offset: 0x00007052
		protected ReadOnlyDictionary(Hashtable dictionary)
		{
			this.Dictionary = dictionary;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000806C File Offset: 0x0000706C
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00008074 File Offset: 0x00007074
		protected virtual Hashtable Dictionary
		{
			get
			{
				return this.m_dictionary;
			}
			set
			{
				this.m_dictionary = value;
				if (this.m_dictionary == null)
				{
					this.m_dictionary = new Hashtable();
				}
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00008090 File Offset: 0x00007090
		protected ReadOnlyDictionary(SerializationInfo info, StreamingContext context)
		{
			int num = (int)info.GetValue("CT", typeof(int));
			this.m_dictionary = new Hashtable();
			for (int i = 0; i < num; i++)
			{
				object value = info.GetValue("KY" + i.ToString(), typeof(object));
				object value2 = info.GetValue("VA" + i.ToString(), typeof(object));
				if (value != null)
				{
					this.m_dictionary[value] = value2;
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00008134 File Offset: 0x00007134
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("CT", this.m_dictionary.Count);
			int num = 0;
			IDictionaryEnumerator enumerator = this.m_dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				info.AddValue("KY" + num.ToString(), enumerator.Key);
				info.AddValue("VA" + num.ToString(), enumerator.Value);
				num++;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000300 RID: 768 RVA: 0x000081AD File Offset: 0x000071AD
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000081B0 File Offset: 0x000071B0
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		// Token: 0x1700009A RID: 154
		public virtual object this[object key]
		{
			get
			{
				return this.m_dictionary[key];
			}
			set
			{
				throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000081D7 File Offset: 0x000071D7
		public virtual void Remove(object key)
		{
			throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000081E3 File Offset: 0x000071E3
		public virtual bool Contains(object key)
		{
			return this.m_dictionary.Contains(key);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000081F1 File Offset: 0x000071F1
		public virtual void Clear()
		{
			throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000307 RID: 775 RVA: 0x000081FD File Offset: 0x000071FD
		public virtual ICollection Values
		{
			get
			{
				return this.m_dictionary.Values;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000820A File Offset: 0x0000720A
		public void Add(object key, object value)
		{
			throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00008216 File Offset: 0x00007216
		public virtual ICollection Keys
		{
			get
			{
				return this.m_dictionary.Keys;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00008223 File Offset: 0x00007223
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00008226 File Offset: 0x00007226
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00008229 File Offset: 0x00007229
		public virtual int Count
		{
			get
			{
				return this.m_dictionary.Count;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00008236 File Offset: 0x00007236
		public virtual void CopyTo(Array array, int index)
		{
			if (this.m_dictionary != null)
			{
				this.m_dictionary.CopyTo(array, index);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000824D File Offset: 0x0000724D
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00008250 File Offset: 0x00007250
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00008258 File Offset: 0x00007258
		public virtual object Clone()
		{
			ReadOnlyDictionary readOnlyDictionary = (ReadOnlyDictionary)base.MemberwiseClone();
			Hashtable hashtable = new Hashtable();
			IDictionaryEnumerator enumerator = this.m_dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				hashtable.Add(Convert.Clone(enumerator.Key), Convert.Clone(enumerator.Value));
			}
			readOnlyDictionary.m_dictionary = hashtable;
			return readOnlyDictionary;
		}

		// Token: 0x04000163 RID: 355
		private const string READ_ONLY_DICTIONARY = "Cannot change the contents of a read-only dictionary";

		// Token: 0x04000164 RID: 356
		private Hashtable m_dictionary = new Hashtable();

		// Token: 0x02000077 RID: 119
		private class Names
		{
			// Token: 0x04000165 RID: 357
			internal const string COUNT = "CT";

			// Token: 0x04000166 RID: 358
			internal const string KEY = "KY";

			// Token: 0x04000167 RID: 359
			internal const string VALUE = "VA";
		}
	}
}
