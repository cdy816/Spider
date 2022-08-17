using System;

namespace Opc.Ae
{
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class BrowseElement
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00012EB1 File Offset: 0x00011EB1
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00012EB9 File Offset: 0x00011EB9
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

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00012EC2 File Offset: 0x00011EC2
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00012ECA File Offset: 0x00011ECA
		public string QualifiedName
		{
			get
			{
				return this.m_qualifiedName;
			}
			set
			{
				this.m_qualifiedName = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00012ED3 File Offset: 0x00011ED3
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00012EDB File Offset: 0x00011EDB
		public BrowseType NodeType
		{
			get
			{
				return this.m_nodeType;
			}
			set
			{
				this.m_nodeType = value;
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00012EE4 File Offset: 0x00011EE4
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0400039F RID: 927
		private string m_name;

		// Token: 0x040003A0 RID: 928
		private string m_qualifiedName;

		// Token: 0x040003A1 RID: 929
		private BrowseType m_nodeType;
	}
}
