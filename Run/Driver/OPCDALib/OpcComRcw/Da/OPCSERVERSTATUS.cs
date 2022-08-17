using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008F RID: 143
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCSERVERSTATUS
	{
		// Token: 0x040003D5 RID: 981
		public FILETIME ftStartTime;

		// Token: 0x040003D6 RID: 982
		public FILETIME ftCurrentTime;

		// Token: 0x040003D7 RID: 983
		public FILETIME ftLastUpdateTime;

		// Token: 0x040003D8 RID: 984
		public OPCSERVERSTATE dwServerState;

		// Token: 0x040003D9 RID: 985
		[MarshalAs(UnmanagedType.I4)]
		public int dwGroupCount;

		// Token: 0x040003DA RID: 986
		[MarshalAs(UnmanagedType.I4)]
		public int dwBandWidth;

		// Token: 0x040003DB RID: 987
		[MarshalAs(UnmanagedType.I2)]
		public short wMajorVersion;

		// Token: 0x040003DC RID: 988
		[MarshalAs(UnmanagedType.I2)]
		public short wMinorVersion;

		// Token: 0x040003DD RID: 989
		[MarshalAs(UnmanagedType.I2)]
		public short wBuildNumber;

		// Token: 0x040003DE RID: 990
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x040003DF RID: 991
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szVendorInfo;
	}
}
