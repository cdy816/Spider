using System;

namespace Opc.Ae
{
	// Token: 0x020000CA RID: 202
	[Flags]
	public enum ChangeMask
	{
		// Token: 0x04000302 RID: 770
		ActiveState = 1,
		// Token: 0x04000303 RID: 771
		AcknowledgeState = 2,
		// Token: 0x04000304 RID: 772
		EnableState = 4,
		// Token: 0x04000305 RID: 773
		Quality = 8,
		// Token: 0x04000306 RID: 774
		Severity = 16,
		// Token: 0x04000307 RID: 775
		SubCondition = 32,
		// Token: 0x04000308 RID: 776
		Message = 64,
		// Token: 0x04000309 RID: 777
		Attribute = 128
	}
}
