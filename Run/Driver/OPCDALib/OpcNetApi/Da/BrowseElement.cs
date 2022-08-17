using System;

namespace Opc.Da
{
	// Token: 0x020000E8 RID: 232
	[Serializable]
	public class BrowseElement : ICloneable
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00012C42 File Offset: 0x00011C42
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00012C4A File Offset: 0x00011C4A
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00012C53 File Offset: 0x00011C53
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x00012C5B File Offset: 0x00011C5B
		public string ItemName
		{
			get
			{
				return this.m_itemName;
			}
			set
			{
				this.m_itemName = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00012C64 File Offset: 0x00011C64
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x00012C6C File Offset: 0x00011C6C
		public string ItemPath
		{
			get
			{
				return this.m_itemPath;
			}
			set
			{
				this.m_itemPath = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00012C75 File Offset: 0x00011C75
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00012C7D File Offset: 0x00011C7D
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

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00012C86 File Offset: 0x00011C86
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x00012C8E File Offset: 0x00011C8E
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

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00012C97 File Offset: 0x00011C97
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x00012C9F File Offset: 0x00011C9F
		public ItemProperty[] Properties
		{
			get
			{
				return this.m_properties;
			}
			set
			{
				this.m_properties = value;
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00012CA8 File Offset: 0x00011CA8
		public virtual object Clone()
		{
			BrowseElement browseElement = (BrowseElement)base.MemberwiseClone();
			browseElement.m_properties = (ItemProperty[])Convert.Clone(this.m_properties);
			return browseElement;
		}

		// Token: 0x0400038A RID: 906
		private string m_name;

		// Token: 0x0400038B RID: 907
		private string m_itemName;

		// Token: 0x0400038C RID: 908
		private string m_itemPath;

		// Token: 0x0400038D RID: 909
		private bool m_isItem;

		// Token: 0x0400038E RID: 910
		private bool m_hasChildren;

		// Token: 0x0400038F RID: 911
		private ItemProperty[] m_properties = new ItemProperty[0];
	}
}
