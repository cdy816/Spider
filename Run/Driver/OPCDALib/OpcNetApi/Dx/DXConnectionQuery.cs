using System;

namespace Opc.Dx
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class DXConnectionQuery
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000055A2 File Offset: 0x000045A2
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000055AA File Offset: 0x000045AA
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000055B3 File Offset: 0x000045B3
		// (set) Token: 0x0600010B RID: 267 RVA: 0x000055BB File Offset: 0x000045BB
		public string BrowsePath
		{
			get
			{
				return this.m_browsePath;
			}
			set
			{
				this.m_browsePath = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000055C4 File Offset: 0x000045C4
		// (set) Token: 0x0600010D RID: 269 RVA: 0x000055CC File Offset: 0x000045CC
		public bool Recursive
		{
			get
			{
				return this.m_recursive;
			}
			set
			{
				this.m_recursive = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000055D5 File Offset: 0x000045D5
		public DXConnectionCollection Masks
		{
			get
			{
				return this.m_masks;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000055E0 File Offset: 0x000045E0
		public DXConnection[] Query(Server server, out ResultID[] errors)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			return server.QueryDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000561C File Offset: 0x0000461C
		public GeneralResponse Update(Server server, DXConnection connectionDefinition, out ResultID[] errors)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			return server.UpdateDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, connectionDefinition, out errors);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005658 File Offset: 0x00004658
		public GeneralResponse Delete(Server server, out ResultID[] errors)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			return server.DeleteDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005694 File Offset: 0x00004694
		public GeneralResponse CopyDefaultAttributes(Server server, bool configToStatus, out ResultID[] errors)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			return server.CopyDXConnectionDefaultAttributes(configToStatus, this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000056D0 File Offset: 0x000046D0
		public DXConnectionQuery()
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000056E4 File Offset: 0x000046E4
		public DXConnectionQuery(DXConnectionQuery query)
		{
			if (query != null)
			{
				this.Name = query.Name;
				this.BrowsePath = query.BrowsePath;
				this.Recursive = query.Recursive;
				this.m_masks = new DXConnectionCollection(query.Masks);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000573A File Offset: 0x0000473A
		public virtual object Clone()
		{
			return new DXConnectionQuery(this);
		}

		// Token: 0x040000C4 RID: 196
		private string m_name;

		// Token: 0x040000C5 RID: 197
		private string m_browsePath;

		// Token: 0x040000C6 RID: 198
		private DXConnectionCollection m_masks = new DXConnectionCollection();

		// Token: 0x040000C7 RID: 199
		private bool m_recursive;
	}
}
