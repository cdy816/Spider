using System;

namespace Opc.Ae
{
	// Token: 0x020000C4 RID: 196
	[Flags]
	public enum FilterType
	{
		// Token: 0x040002E9 RID: 745
		Event = 1,
		// Token: 0x040002EA RID: 746
		Category = 2,
		// Token: 0x040002EB RID: 747
		Severity = 4,
		// Token: 0x040002EC RID: 748
		Area = 8,
		// Token: 0x040002ED RID: 749
		Source = 16,
		// Token: 0x040002EE RID: 750
		All = 65535
	}
}
