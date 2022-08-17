using System;

namespace Opc.Hda
{
	// Token: 0x0200004C RID: 76
	public class BrowseElement : ItemIdentifier
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000647E File Offset: 0x0000547E
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00006486 File Offset: 0x00005486
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000648F File Offset: 0x0000548F
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006497 File Offset: 0x00005497
		public bool IsItem
		{
			get
			{
				return this.m_isItem;
			}
			set
			{
				this.m_isItem = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000064A0 File Offset: 0x000054A0
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000064A8 File Offset: 0x000054A8
		public bool HasChildren
		{
			get
			{
				return this.m_hasChildren;
			}
			set
			{
				this.m_hasChildren = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000064B1 File Offset: 0x000054B1
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000064B9 File Offset: 0x000054B9
		public AttributeValueCollection Attributes
		{
			get
			{
				return this.m_attributes;
			}
			set
			{
				this.m_attributes = value;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000064C4 File Offset: 0x000054C4
		public override object Clone()
		{
			BrowseElement browseElement = (BrowseElement)base.MemberwiseClone();
			browseElement.Attributes = (AttributeValueCollection)this.m_attributes.Clone();
			return browseElement;
		}

		// Token: 0x040000EF RID: 239
		private string m_name;

		// Token: 0x040000F0 RID: 240
		private bool m_isItem;

		// Token: 0x040000F1 RID: 241
		private bool m_hasChildren;

		// Token: 0x040000F2 RID: 242
		private AttributeValueCollection m_attributes = new AttributeValueCollection();
	}
}
