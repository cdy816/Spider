using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008A RID: 138
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMHEADER1
	{
		// Token: 0x040003C1 RID: 961
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003C2 RID: 962
		[MarshalAs(UnmanagedType.I4)]
		public int dwValueOffset;

		// Token: 0x040003C3 RID: 963
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x040003C4 RID: 964
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x040003C5 RID: 965
		public FILETIME ftTimeStampItem;
	}
}
