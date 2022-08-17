using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000089 RID: 137
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCGROUPHEADER
	{
		// Token: 0x040003BC RID: 956
		[MarshalAs(UnmanagedType.I4)]
		public int dwSize;

		// Token: 0x040003BD RID: 957
		[MarshalAs(UnmanagedType.I4)]
		public int dwItemCount;

		// Token: 0x040003BE RID: 958
		[MarshalAs(UnmanagedType.I4)]
		public int hClientGroup;

		// Token: 0x040003BF RID: 959
		[MarshalAs(UnmanagedType.I4)]
		public int dwTransactionID;

		// Token: 0x040003C0 RID: 960
		[MarshalAs(UnmanagedType.I4)]
		public int hrStatus;
	}
}
