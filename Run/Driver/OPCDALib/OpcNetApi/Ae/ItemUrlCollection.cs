using System;

namespace Opc.Ae
{
	// Token: 0x0200008B RID: 139
	public class ItemUrlCollection : ReadOnlyCollection
	{
		// Token: 0x170000E5 RID: 229
		public ItemUrl this[int index]
		{
			get
			{
				return (ItemUrl)this.Array.GetValue(index);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000BE1B File Offset: 0x0000AE1B
		public new ItemUrl[] ToArray()
		{
			return (ItemUrl[])Convert.Clone(this.Array);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000BE2D File Offset: 0x0000AE2D
		public ItemUrlCollection() : base(new ItemUrl[0])
		{
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000BE3B File Offset: 0x0000AE3B
		public ItemUrlCollection(ItemUrl[] itemUrls) : base(itemUrls)
		{
		}
	}
}
