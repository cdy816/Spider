using System;

namespace Opc.Dx
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public class ItemIdentifier : Opc.ItemIdentifier
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000859A File Offset: 0x0000759A
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000085A2 File Offset: 0x000075A2
		public string Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				this.m_version = value;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000085AB File Offset: 0x000075AB
		public ItemIdentifier()
		{
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000085B3 File Offset: 0x000075B3
		public ItemIdentifier(string itemName) : base(itemName)
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000085BC File Offset: 0x000075BC
		public ItemIdentifier(string itemPath, string itemName) : base(itemPath, itemName)
		{
		}

		// Token: 0x06000324 RID: 804 RVA: 0x000085C6 File Offset: 0x000075C6
		public ItemIdentifier(Opc.ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000085CF File Offset: 0x000075CF
		public ItemIdentifier(ItemIdentifier item) : base(item)
		{
			if (item != null)
			{
				this.m_version = item.m_version;
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000085E8 File Offset: 0x000075E8
		public override bool Equals(object target)
		{
			if (typeof(ItemIdentifier).IsInstanceOfType(target))
			{
				ItemIdentifier itemIdentifier = (ItemIdentifier)target;
				return itemIdentifier.ItemName == base.ItemName && itemIdentifier.ItemPath == base.ItemPath && itemIdentifier.Version == this.Version;
			}
			return false;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00008649 File Offset: 0x00007649
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000187 RID: 391
		private string m_version;
	}
}
