using System;

namespace Opc.Hda
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class Item : ItemIdentifier
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00006754 File Offset: 0x00005754
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000675C File Offset: 0x0000575C
		public int AggregateID
		{
			get
			{
				return this.m_aggregateID;
			}
			set
			{
				this.m_aggregateID = value;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00006765 File Offset: 0x00005765
		public Item()
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000676D File Offset: 0x0000576D
		public Item(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00006776 File Offset: 0x00005776
		public Item(Item item) : base(item)
		{
			if (item != null)
			{
				this.AggregateID = item.AggregateID;
			}
		}

		// Token: 0x04000102 RID: 258
		private int m_aggregateID;
	}
}
