using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000014 RID: 20
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IdentifiedResult
	{
		// Token: 0x040000EE RID: 238
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemPath;

		// Token: 0x040000EF RID: 239
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemName;

		// Token: 0x040000F0 RID: 240
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVersion;

		// Token: 0x040000F1 RID: 241
		[MarshalAs(UnmanagedType.I4)]
		public int hResultCode;
	}
}
