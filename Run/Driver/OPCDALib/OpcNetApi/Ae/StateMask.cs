using System;

namespace Opc.Ae
{
	// Token: 0x020000C2 RID: 194
	[Flags]
	public enum StateMask
	{
		// Token: 0x040002DC RID: 732
		Name = 1,
		// Token: 0x040002DD RID: 733
		ClientHandle = 2,
		// Token: 0x040002DE RID: 734
		Active = 4,
		// Token: 0x040002DF RID: 735
		BufferTime = 8,
		// Token: 0x040002E0 RID: 736
		MaxSize = 16,
		// Token: 0x040002E1 RID: 737
		KeepAlive = 32,
		// Token: 0x040002E2 RID: 738
		All = 65535
	}
}
