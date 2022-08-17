using System;
using System.Collections;

namespace Opc.Dx
{
	// Token: 0x0200007E RID: 126
	public class SourceServerCollection : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x170000AA RID: 170
		public SourceServer this[int index]
		{
			get
			{
				return (SourceServer)this.m_servers[index];
			}
		}

		// Token: 0x170000AB RID: 171
		public SourceServer this[string name]
		{
			get
			{
				foreach (object obj in this.m_servers)
				{
					SourceServer sourceServer = (SourceServer)obj;
					if (sourceServer.Name == name)
					{
						return sourceServer;
					}
				}
				return null;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00008818 File Offset: 0x00007818
		internal SourceServerCollection()
		{
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000882C File Offset: 0x0000782C
		internal void Initialize(ICollection sourceServers)
		{
			this.m_servers.Clear();
			if (sourceServers != null)
			{
				foreach (object obj in sourceServers)
				{
					SourceServer value = (SourceServer)obj;
					this.m_servers.Add(value);
				}
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00008894 File Offset: 0x00007894
		public virtual object Clone()
		{
			SourceServerCollection sourceServerCollection = (SourceServerCollection)base.MemberwiseClone();
			sourceServerCollection.m_servers = new ArrayList();
			foreach (object obj in this.m_servers)
			{
				SourceServer sourceServer = (SourceServer)obj;
				sourceServerCollection.m_servers.Add(sourceServer.Clone());
			}
			return sourceServerCollection;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00008910 File Offset: 0x00007910
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00008913 File Offset: 0x00007913
		public int Count
		{
			get
			{
				if (this.m_servers == null)
				{
					return 0;
				}
				return this.m_servers.Count;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000892A File Offset: 0x0000792A
		public void CopyTo(Array array, int index)
		{
			if (this.m_servers != null)
			{
				this.m_servers.CopyTo(array, index);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00008941 File Offset: 0x00007941
		public void CopyTo(SourceServer[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000894B File Offset: 0x0000794B
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000894E File Offset: 0x0000794E
		public IEnumerator GetEnumerator()
		{
			return this.m_servers.GetEnumerator();
		}

		// Token: 0x04000193 RID: 403
		private ArrayList m_servers = new ArrayList();
	}
}
