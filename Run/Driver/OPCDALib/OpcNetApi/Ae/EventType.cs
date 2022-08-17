using System;

namespace Opc.Ae
{
	// Token: 0x020000C3 RID: 195
	[Flags]
	public enum EventType
	{
		// Token: 0x040002E4 RID: 740
		Simple = 1,
		// Token: 0x040002E5 RID: 741
		Tracking = 2,
		// Token: 0x040002E6 RID: 742
		Condition = 4,
		// Token: 0x040002E7 RID: 743
		All = 65535
	}
}
