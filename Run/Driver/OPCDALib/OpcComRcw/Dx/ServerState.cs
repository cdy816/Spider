using System;

namespace OpcRcw.Dx
{
	// Token: 0x02000021 RID: 33
	public enum ServerState
	{
		// Token: 0x040001A0 RID: 416
		ServerState_RUNNING = 1,
		// Token: 0x040001A1 RID: 417
		ServerState_FAILED,
		// Token: 0x040001A2 RID: 418
		ServerState_NOCONFIG,
		// Token: 0x040001A3 RID: 419
		ServerState_SUSPENDED,
		// Token: 0x040001A4 RID: 420
		ServerState_TEST,
		// Token: 0x040001A5 RID: 421
		ServerState_COMM_FAULT,
		// Token: 0x040001A6 RID: 422
		ServerState_UNKNOWN
	}
}
