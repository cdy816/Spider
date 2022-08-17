using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000013 RID: 19
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ItemIdentifier
	{
		// Token: 0x040000EA RID: 234
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemPath;

		// Token: 0x040000EB RID: 235
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemName;

		// Token: 0x040000EC RID: 236
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVersion;

		// Token: 0x040000ED RID: 237
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
