using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000095 RID: 149
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCBROWSEELEMENT
	{
		// Token: 0x04000406 RID: 1030
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x04000407 RID: 1031
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemID;

		// Token: 0x04000408 RID: 1032
		[MarshalAs(UnmanagedType.I4)]
		public int dwFlagValue;

		// Token: 0x04000409 RID: 1033
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;

		// Token: 0x0400040A RID: 1034
		public OPCITEMPROPERTIES ItemProperties;
	}
}
