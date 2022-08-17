using System;

namespace Opc.Ae
{
	// Token: 0x0200008A RID: 138
	[Serializable]
	public class ItemUrl : ItemIdentifier
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000BD8A File Offset: 0x0000AD8A
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000BD92 File Offset: 0x0000AD92
		public URL Url
		{
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000BD9B File Offset: 0x0000AD9B
		public ItemUrl()
		{
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000BDAE File Offset: 0x0000ADAE
		public ItemUrl(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000BDC2 File Offset: 0x0000ADC2
		public ItemUrl(ItemIdentifier item, URL url) : base(item)
		{
			this.Url = url;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000BDDD File Offset: 0x0000ADDD
		public ItemUrl(ItemUrl item) : base(item)
		{
			if (item != null)
			{
				this.Url = item.Url;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000BE00 File Offset: 0x0000AE00
		public override object Clone()
		{
			return new ItemUrl(this);
		}

		// Token: 0x040001CC RID: 460
		private URL m_url = new URL();
	}
}
