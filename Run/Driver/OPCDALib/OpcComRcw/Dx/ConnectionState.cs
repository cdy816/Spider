using System;

namespace OpcRcw.Dx
{
	// Token: 0x02000022 RID: 34
	public enum ConnectionState
	{
		// Token: 0x040001A8 RID: 424
		ConnectionState_INITIALIZING = 1,
		// Token: 0x040001A9 RID: 425
		ConnectionState_OPERATIONAL,
		// Token: 0x040001AA RID: 426
		ConnectionState_DEACTIVATED,
		// Token: 0x040001AB RID: 427
		ConnectionState_SOURCE_SERVER_NOT_CONNECTED,
		// Token: 0x040001AC RID: 428
		ConnectionState_SUBSCRIPTION_FAILED,
		// Token: 0x040001AD RID: 429
		ConnectionState_TARGET_ITEM_NOT_FOUND
	}
}
