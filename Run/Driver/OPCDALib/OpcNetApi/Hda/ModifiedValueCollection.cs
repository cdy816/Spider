using System;

namespace Opc.Hda
{
	// Token: 0x020000AB RID: 171
	[Serializable]
	public class ModifiedValueCollection : ItemValueCollection
	{
		// Token: 0x17000140 RID: 320
		public ModifiedValue this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value;
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000100B1 File Offset: 0x0000F0B1
		public ModifiedValueCollection()
		{
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000100B9 File Offset: 0x0000F0B9
		public ModifiedValueCollection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000100C2 File Offset: 0x0000F0C2
		public ModifiedValueCollection(Item item) : base(item)
		{
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000100CB File Offset: 0x0000F0CB
		public ModifiedValueCollection(ItemValueCollection item) : base(item)
		{
		}
	}
}
