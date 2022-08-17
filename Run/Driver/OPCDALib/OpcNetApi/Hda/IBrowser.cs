using System;

namespace Opc.Hda
{
	// Token: 0x02000048 RID: 72
	public interface IBrowser : IDisposable
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001B0 RID: 432
		BrowseFilterCollection Filters { get; }

		// Token: 0x060001B1 RID: 433
		BrowseElement[] Browse(ItemIdentifier itemID);

		// Token: 0x060001B2 RID: 434
		BrowseElement[] Browse(ItemIdentifier itemID, int maxElements, out IBrowsePosition position);

		// Token: 0x060001B3 RID: 435
		BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position);
	}
}
