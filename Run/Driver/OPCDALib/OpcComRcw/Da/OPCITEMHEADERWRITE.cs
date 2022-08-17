using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008D RID: 141
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMHEADERWRITE
	{
		// Token: 0x040003CE RID: 974
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003CF RID: 975
		[MarshalAs(UnmanagedType.I4)]
		public int dwError;
	}
}
