using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008B RID: 139
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMHEADER2
	{
		// Token: 0x040003C6 RID: 966
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003C7 RID: 967
		[MarshalAs(UnmanagedType.I4)]
		public int dwValueOffset;

		// Token: 0x040003C8 RID: 968
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x040003C9 RID: 969
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;
	}
}
