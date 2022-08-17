using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class WriteableDictionary : IDictionary, ICollection, IEnumerable, ISerializable
	{
		// Token: 0x0600029F RID: 671 RVA: 0x000073CC File Offset: 0x000063CC
		protected WriteableDictionary(IDictionary dictionary, System.Type keyType, System.Type valueType)
		{
			this.m_keyType = ((keyType == null) ? typeof(object) : keyType);
			this.m_valueType = ((valueType == null) ? typeof(object) : valueType);
			this.Dictionary = dictionary;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000741D File Offset: 0x0000641D
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00007428 File Offset: 0x00006428
		protected virtual IDictionary Dictionary
		{
			get
			{
				return this.m_dictionary;
			}
			set
			{
				if (value != null)
				{
					if (this.m_keyType != null)
					{
						foreach (object element in value.Keys)
						{
							this.ValidateKey(element, this.m_keyType);
						}
					}
					if (this.m_valueType != null)
					{
						foreach (object element2 in value.Values)
						{
							this.ValidateValue(element2, this.m_valueType);
						}
					}
					this.m_dictionary = new Hashtable(value);
					return;
				}
				this.m_dictionary = new Hashtable();
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00007500 File Offset: 0x00006500
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00007508 File Offset: 0x00006508
		protected System.Type KeyType
		{
			get
			{
				return this.m_keyType;
			}
			set
			{
				foreach (object element in this.m_dictionary.Keys)
				{
					this.ValidateKey(element, value);
				}
				this.m_keyType = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000756C File Offset: 0x0000656C
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00007574 File Offset: 0x00006574
		protected System.Type ValueType
		{
			get
			{
				return this.m_valueType;
			}
			set
			{
				foreach (object element in this.m_dictionary.Values)
				{
					this.ValidateValue(element, value);
				}
				this.m_valueType = value;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000075D8 File Offset: 0x000065D8
		protected virtual void ValidateKey(object element, System.Type type)
		{
			if (element == null)
			{
				throw new ArgumentException(string.Format("The {1} '{0}' cannot be added to the dictionary.", element, "key"));
			}
			if (!type.IsInstanceOfType(element))
			{
				throw new ArgumentException(string.Format("A {1} with type '{0}' cannot be added to the dictionary.", element.GetType(), "key"));
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00007617 File Offset: 0x00006617
		protected virtual void ValidateValue(object element, System.Type type)
		{
			if (element != null && !type.IsInstanceOfType(element))
			{
				throw new ArgumentException(string.Format("A {1} with type '{0}' cannot be added to the dictionary.", element.GetType(), "value"));
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00007640 File Offset: 0x00006640
		protected WriteableDictionary(SerializationInfo info, StreamingContext context)
		{
			this.m_keyType = (System.Type)info.GetValue("KT", typeof(Type));
			this.m_valueType = (System.Type)info.GetValue("VT", typeof(Type));
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

		// Token: 0x060002A9 RID: 681 RVA: 0x00007724 File Offset: 0x00006724
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("KT", this.m_keyType);
			info.AddValue("VT", this.m_valueType);
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002AA RID: 682 RVA: 0x000077BF File Offset: 0x000067BF
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000077C2 File Offset: 0x000067C2
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.m_dictionary.GetEnumerator();
		}

		// Token: 0x1700007D RID: 125
		public virtual object this[object key]
		{
			get
			{
				return this.m_dictionary[key];
			}
			set
			{
				this.ValidateKey(key, this.m_keyType);
				this.ValidateValue(value, this.m_valueType);
				this.m_dictionary[key] = value;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00007806 File Offset: 0x00006806
		public virtual void Remove(object key)
		{
			this.m_dictionary.Remove(key);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00007814 File Offset: 0x00006814
		public virtual bool Contains(object key)
		{
			return this.m_dictionary.Contains(key);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00007822 File Offset: 0x00006822
		public virtual void Clear()
		{
			this.m_dictionary.Clear();
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000782F File Offset: 0x0000682F
		public virtual ICollection Values
		{
			get
			{
				return this.m_dictionary.Values;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000783C File Offset: 0x0000683C
		public virtual void Add(object key, object value)
		{
			this.ValidateKey(key, this.m_keyType);
			this.ValidateValue(value, this.m_valueType);
			this.m_dictionary.Add(key, value);
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00007865 File Offset: 0x00006865
		public virtual ICollection Keys
		{
			get
			{
				return this.m_dictionary.Keys;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00007872 File Offset: 0x00006872
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00007875 File Offset: 0x00006875
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00007878 File Offset: 0x00006878
		public virtual int Count
		{
			get
			{
				return this.m_dictionary.Count;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00007885 File Offset: 0x00006885
		public virtual void CopyTo(Array array, int index)
		{
			if (this.m_dictionary != null)
			{
				this.m_dictionary.CopyTo(array, index);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000789C File Offset: 0x0000689C
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000789F File Offset: 0x0000689F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000078A8 File Offset: 0x000068A8
		public virtual object Clone()
		{
			WriteableDictionary writeableDictionary = (WriteableDictionary)base.MemberwiseClone();
			Hashtable hashtable = new Hashtable();
			IDictionaryEnumerator enumerator = this.m_dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				hashtable.Add(Convert.Clone(enumerator.Key), Convert.Clone(enumerator.Value));
			}
			writeableDictionary.m_dictionary = hashtable;
			return writeableDictionary;
		}

		// Token: 0x04000140 RID: 320
		protected const string INVALID_VALUE = "The {1} '{0}' cannot be added to the dictionary.";

		// Token: 0x04000141 RID: 321
		protected const string INVALID_TYPE = "A {1} with type '{0}' cannot be added to the dictionary.";

		// Token: 0x04000142 RID: 322
		private Hashtable m_dictionary = new Hashtable();

		// Token: 0x04000143 RID: 323
		private System.Type m_keyType;

		// Token: 0x04000144 RID: 324
		private System.Type m_valueType;

		// Token: 0x0200006D RID: 109
		private class Names
		{
			// Token: 0x04000145 RID: 325
			internal const string COUNT = "CT";

			// Token: 0x04000146 RID: 326
			internal const string KEY = "KY";

			// Token: 0x04000147 RID: 327
			internal const string VALUE = "VA";

			// Token: 0x04000148 RID: 328
			internal const string KEY_TYPE = "KT";

			// Token: 0x04000149 RID: 329
			internal const string VALUE_VALUE = "VT";
		}
	}
}
