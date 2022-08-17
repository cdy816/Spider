using System;

namespace Opc.Ae
{
	// Token: 0x020000EE RID: 238
	[Serializable]
	public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x00012EF4 File Offset: 0x00011EF4
		public BrowsePosition(string areaID, BrowseType browseType, string browseFilter)
		{
			this.m_areaID = areaID;
			this.m_browseType = browseType;
			this.m_browseFilter = browseFilter;
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00012F11 File Offset: 0x00011F11
		public string AreaID
		{
			get
			{
				return this.m_areaID;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00012F19 File Offset: 0x00011F19
		public BrowseType BrowseType
		{
			get
			{
				return this.m_browseType;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00012F21 File Offset: 0x00011F21
		public string BrowseFilter
		{
			get
			{
				return this.m_browseFilter;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00012F2C File Offset: 0x00011F2C
		~BrowsePosition()
		{
			this.Dispose(false);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00012F5C File Offset: 0x00011F5C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00012F6B File Offset: 0x00011F6B
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00012F7E File Offset: 0x00011F7E
		public virtual object Clone()
		{
			return (BrowsePosition)base.MemberwiseClone();
		}

		// Token: 0x040003A2 RID: 930
		private bool m_disposed;

		// Token: 0x040003A3 RID: 931
		private string m_areaID;

		// Token: 0x040003A4 RID: 932
		private BrowseType m_browseType;

		// Token: 0x040003A5 RID: 933
		private string m_browseFilter;
	}
}
