using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000074 RID: 116
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCEVENTSERVERSTATUS
	{
		// Token: 0x04000366 RID: 870
		public FILETIME ftStartTime;

		// Token: 0x04000367 RID: 871
		public FILETIME ftCurrentTime;

		// Token: 0x04000368 RID: 872
		public FILETIME ftLastUpdateTime;

		// Token: 0x04000369 RID: 873
		public OPCEVENTSERVERSTATE dwServerState;

		// Token: 0x0400036A RID: 874
		[MarshalAs(UnmanagedType.I2)]
		public short wMajorVersion;

		// Token: 0x0400036B RID: 875
		[MarshalAs(UnmanagedType.I2)]
		public short wMinorVersion;

		// Token: 0x0400036C RID: 876
		[MarshalAs(UnmanagedType.I2)]
		public short wBuildNumber;

		// Token: 0x0400036D RID: 877
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x0400036E RID: 878
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVendorInfo;
	}
}
