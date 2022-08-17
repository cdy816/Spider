using System;

namespace Opc.Da
{
	// Token: 0x020000E4 RID: 228
	[Flags]
	public enum StateMask
	{
		// Token: 0x0400036C RID: 876
		Name = 1,
		// Token: 0x0400036D RID: 877
		ClientHandle = 2,
		// Token: 0x0400036E RID: 878
		Locale = 4,
		// Token: 0x0400036F RID: 879
		Active = 8,
		// Token: 0x04000370 RID: 880
		UpdateRate = 16,
		// Token: 0x04000371 RID: 881
		KeepAlive = 32,
		// Token: 0x04000372 RID: 882
		ReqType = 64,
		// Token: 0x04000373 RID: 883
		Deadband = 128,
		// Token: 0x04000374 RID: 884
		SamplingRate = 256,
		// Token: 0x04000375 RID: 885
		EnableBuffering = 512,
		// Token: 0x04000376 RID: 886
		All = 65535
	}
}
