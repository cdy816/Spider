using System;

namespace Opc.Hda
{
	// Token: 0x020000A7 RID: 167
	[Flags]
	public enum Quality
	{
		// Token: 0x04000285 RID: 645
		ExtraData = 65536,
		// Token: 0x04000286 RID: 646
		Interpolated = 131072,
		// Token: 0x04000287 RID: 647
		Raw = 262144,
		// Token: 0x04000288 RID: 648
		Calculated = 524288,
		// Token: 0x04000289 RID: 649
		NoBound = 1048576,
		// Token: 0x0400028A RID: 650
		NoData = 2097152,
		// Token: 0x0400028B RID: 651
		DataLost = 4194304,
		// Token: 0x0400028C RID: 652
		Conversion = 8388608,
		// Token: 0x0400028D RID: 653
		Partial = 16777216
	}
}
