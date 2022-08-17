using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Opc.Dx
{
	// Token: 0x02000087 RID: 135
	[Serializable]
	public class DXConnectionCollection : ICloneable, IList, ICollection, IEnumerable, ISerializable
	{
		// Token: 0x170000DD RID: 221
		public DXConnection this[int index]
		{
			get
			{
				return (DXConnection)this.m_connections[index];
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000AB7F File Offset: 0x00009B7F
		public DXConnection[] ToArray()
		{
			return (DXConnection[])this.m_connections.ToArray(typeof(DXConnection));
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000AB9B File Offset: 0x00009B9B
		internal DXConnectionCollection()
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000ABB0 File Offset: 0x00009BB0
		internal DXConnectionCollection(ICollection connections)
		{
			if (connections != null)
			{
				foreach (object obj in connections)
				{
					DXConnection value = (DXConnection)obj;
					this.m_connections.Add(value);
				}
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000AC20 File Offset: 0x00009C20
		protected DXConnectionCollection(SerializationInfo info, StreamingContext context)
		{
			DXConnection[] array = (DXConnection[])info.GetValue("Connections", typeof(DXConnection[]));
			if (array != null)
			{
				foreach (DXConnection value in array)
				{
					this.m_connections.Add(value);
				}
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000AC80 File Offset: 0x00009C80
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			DXConnection[] array = null;
			if (this.m_connections.Count > 0)
			{
				array = new DXConnection[this.m_connections.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (DXConnection)this.m_connections[i];
				}
			}
			info.AddValue("Connections", array);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000ACDC File Offset: 0x00009CDC
		public virtual object Clone()
		{
			DXConnectionCollection dxconnectionCollection = (DXConnectionCollection)base.MemberwiseClone();
			dxconnectionCollection.m_connections = new ArrayList();
			foreach (object obj in this.m_connections)
			{
				DXConnection dxconnection = (DXConnection)obj;
				dxconnectionCollection.m_connections.Add(dxconnection.Clone());
			}
			return dxconnectionCollection;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000AD58 File Offset: 0x00009D58
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000AD5B File Offset: 0x00009D5B
		public int Count
		{
			get
			{
				if (this.m_connections == null)
				{
					return 0;
				}
				return this.m_connections.Count;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000AD72 File Offset: 0x00009D72
		public void CopyTo(Array array, int index)
		{
			if (this.m_connections != null)
			{
				this.m_connections.CopyTo(array, index);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000AD89 File Offset: 0x00009D89
		public void CopyTo(DXConnection[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000AD93 File Offset: 0x00009D93
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000AD96 File Offset: 0x00009D96
		public IEnumerator GetEnumerator()
		{
			return this.m_connections.GetEnumerator();
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000ADA3 File Offset: 0x00009DA3
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E2 RID: 226
		object IList.this[int index]
		{
			get
			{
				return this.m_connections[index];
			}
			set
			{
				this.Insert(index, value);
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000ADBE File Offset: 0x00009DBE
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.m_connections.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.m_connections[index]);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000ADEF File Offset: 0x00009DEF
		public void Insert(int index, object value)
		{
			if (!typeof(DXConnection).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only add DXConnection objects into the collection.");
			}
			this.m_connections.Insert(index, (DXConnection)value);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000AE20 File Offset: 0x00009E20
		public void Remove(object value)
		{
			if (!typeof(ItemIdentifier).IsInstanceOfType(value))
			{
				throw new ArgumentException("May only delete Opc.Dx.ItemIdentifier obejcts from the collection.");
			}
			foreach (object obj in this.m_connections)
			{
				ItemIdentifier itemIdentifier = (ItemIdentifier)obj;
				if (itemIdentifier.Equals(value))
				{
					this.m_connections.Remove(itemIdentifier);
					break;
				}
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000AEA8 File Offset: 0x00009EA8
		public bool Contains(object value)
		{
			foreach (object obj in this.m_connections)
			{
				ItemIdentifier itemIdentifier = (ItemIdentifier)obj;
				if (itemIdentifier.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000AF0C File Offset: 0x00009F0C
		public void Clear()
		{
			this.m_connections.Clear();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000AF19 File Offset: 0x00009F19
		public int IndexOf(object value)
		{
			return this.m_connections.IndexOf(value);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000AF27 File Offset: 0x00009F27
		public int Add(object value)
		{
			this.Insert(this.m_connections.Count, value);
			return this.m_connections.Count - 1;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000AF48 File Offset: 0x00009F48
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000AF4B File Offset: 0x00009F4B
		public void Insert(int index, DXConnection value)
		{
			this.Insert(index, value);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000AF55 File Offset: 0x00009F55
		public void Remove(DXConnection value)
		{
			this.Remove(value);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000AF5E File Offset: 0x00009F5E
		public bool Contains(DXConnection value)
		{
			return this.Contains(value);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000AF67 File Offset: 0x00009F67
		public int IndexOf(DXConnection value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000AF70 File Offset: 0x00009F70
		public int Add(DXConnection value)
		{
			return this.Add(value);
		}

		// Token: 0x040001CA RID: 458
		private ArrayList m_connections = new ArrayList();

		// Token: 0x02000088 RID: 136
		private class Names
		{
			// Token: 0x040001CB RID: 459
			internal const string CONNECTIONS = "Connections";
		}
	}
}
