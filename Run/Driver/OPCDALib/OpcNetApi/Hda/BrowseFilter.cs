using System;

namespace Opc.Hda
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public class BrowseFilter : ICloneable
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00006507 File Offset: 0x00005507
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000650F File Offset: 0x0000550F
		public int AttributeID
		{
			get
			{
				return this.m_attributeID;
			}
			set
			{
				this.m_attributeID = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00006518 File Offset: 0x00005518
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00006520 File Offset: 0x00005520
		public Operator Operator
		{
			get
			{
				return this.m_operator;
			}
			set
			{
				this.m_operator = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00006529 File Offset: 0x00005529
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00006531 File Offset: 0x00005531
		public object FilterValue
		{
			get
			{
				return this.m_filterValue;
			}
			set
			{
				this.m_filterValue = value;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000653C File Offset: 0x0000553C
		public virtual object Clone()
		{
			BrowseFilter browseFilter = (BrowseFilter)base.MemberwiseClone();
			browseFilter.FilterValue = Convert.Clone(this.FilterValue);
			return browseFilter;
		}

		// Token: 0x040000FA RID: 250
		private int m_attributeID;

		// Token: 0x040000FB RID: 251
		private Operator m_operator = Operator.Equal;

		// Token: 0x040000FC RID: 252
		private object m_filterValue;
	}
}
