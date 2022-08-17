using System;

namespace Opc.Da
{
	// Token: 0x020000BE RID: 190
	[Flags]
	public enum ResultFilter
	{
		// Token: 0x040002C4 RID: 708
		ItemName = 1,
		// Token: 0x040002C5 RID: 709
		ItemPath = 2,
		// Token: 0x040002C6 RID: 710
		ClientHandle = 4,
		// Token: 0x040002C7 RID: 711
		ItemTime = 8,
		// Token: 0x040002C8 RID: 712
		ErrorText = 16,
		// Token: 0x040002C9 RID: 713
		DiagnosticInfo = 32,
		// Token: 0x040002CA RID: 714
		Minimal = 9,
		// Token: 0x040002CB RID: 715
		All = 63
	}
}
