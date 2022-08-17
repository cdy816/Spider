using System;

namespace Opc.Da
{
	// Token: 0x020000E6 RID: 230
	[Serializable]
	public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00012AA9 File Offset: 0x00011AA9
		public ItemIdentifier ItemID
		{
			get
			{
				return this.m_itemID;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00012AB1 File Offset: 0x00011AB1
		public BrowseFilters Filters
		{
			get
			{
				return (BrowseFilters)this.m_filters.Clone();
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00012AC3 File Offset: 0x00011AC3
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x00012AD0 File Offset: 0x00011AD0
		public int MaxElementsReturned
		{
			get
			{
				return this.m_filters.MaxElementsReturned;
			}
			set
			{
				this.m_filters.MaxElementsReturned = value;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00012ADE File Offset: 0x00011ADE
		public BrowsePosition(ItemIdentifier itemID, BrowseFilters filters)
		{
			if (filters == null)
			{
				throw new ArgumentNullException("filters");
			}
			this.m_itemID = ((itemID != null) ? ((ItemIdentifier)itemID.Clone()) : null);
			this.m_filters = (BrowseFilters)filters.Clone();
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00012B1C File Offset: 0x00011B1C
		~BrowsePosition()
		{
			this.Dispose(false);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00012B4C File Offset: 0x00011B4C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00012B5B File Offset: 0x00011B5B
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00012B6E File Offset: 0x00011B6E
		public virtual object Clone()
		{
			return (BrowsePosition)base.MemberwiseClone();
		}

		// Token: 0x0400037F RID: 895
		private bool m_disposed;

		// Token: 0x04000380 RID: 896
		private BrowseFilters m_filters;

		// Token: 0x04000381 RID: 897
		private ItemIdentifier m_itemID;
	}
}
