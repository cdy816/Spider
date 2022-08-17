using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000081 RID: 129
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct FILETIME
	{
		// Token: 0x0400039A RID: 922
		public int dwLowDateTime;

		// Token: 0x0400039B RID: 923
		public int dwHighDateTime;
	}
}
