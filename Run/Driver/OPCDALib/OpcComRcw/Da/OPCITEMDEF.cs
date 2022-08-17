using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000090 RID: 144
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMDEF
	{
		// Token: 0x040003E0 RID: 992
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szAccessPath;

		// Token: 0x040003E1 RID: 993
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemID;

		// Token: 0x040003E2 RID: 994
		[MarshalAs(UnmanagedType.I4)]
		public int bActive;

		// Token: 0x040003E3 RID: 995
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003E4 RID: 996
		[MarshalAs(UnmanagedType.I4)]
		public int dwBlobSize;

		// Token: 0x040003E5 RID: 997
		public IntPtr pBlob;

		// Token: 0x040003E6 RID: 998
		[MarshalAs(UnmanagedType.I2)]
		public short vtRequestedDataType;

		// Token: 0x040003E7 RID: 999
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;
	}
}
