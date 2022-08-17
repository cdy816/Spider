using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000092 RID: 146
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMRESULT
	{
		// Token: 0x040003F4 RID: 1012
		[MarshalAs(UnmanagedType.I4)]
		public int hServer;

		// Token: 0x040003F5 RID: 1013
		[MarshalAs(UnmanagedType.I2)]
		public short vtCanonicalDataType;

		// Token: 0x040003F6 RID: 1014
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x040003F7 RID: 1015
		[MarshalAs(UnmanagedType.I4)]
		public int dwAccessRights;

		// Token: 0x040003F8 RID: 1016
		[MarshalAs(UnmanagedType.I4)]
		public int dwBlobSize;

		// Token: 0x040003F9 RID: 1017
		public IntPtr pBlob;
	}
}
