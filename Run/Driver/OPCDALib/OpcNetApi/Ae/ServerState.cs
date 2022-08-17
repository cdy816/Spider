using System;

namespace Opc.Ae
{
	// Token: 0x020000CD RID: 205
	public enum ServerState
	{
		// Token: 0x0400031C RID: 796
		Unknown,
		// Token: 0x0400031D RID: 797
		Running,
		// Token: 0x0400031E RID: 798
		Failed,
		// Token: 0x0400031F RID: 799
		NoConfig,
		// Token: 0x04000320 RID: 800
		Suspended,
		// Token: 0x04000321 RID: 801
		Test,
		// Token: 0x04000322 RID: 802
		CommFault
	}
}
