using System;

namespace Opc.Da
{
	// Token: 0x020000BF RID: 191
	public enum serverState
	{
		// Token: 0x040002CD RID: 717
		unknown,
		// Token: 0x040002CE RID: 718
		running,
		// Token: 0x040002CF RID: 719
		failed,
		// Token: 0x040002D0 RID: 720
		noConfig,
		// Token: 0x040002D1 RID: 721
		suspended,
		// Token: 0x040002D2 RID: 722
		test,
		// Token: 0x040002D3 RID: 723
		commFault
	}
}
