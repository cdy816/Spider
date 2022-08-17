using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000091 RID: 145
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMATTRIBUTES
	{
		// Token: 0x040003E8 RID: 1000
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szAccessPath;

		// Token: 0x040003E9 RID: 1001
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemID;

		// Token: 0x040003EA RID: 1002
		[MarshalAs(UnmanagedType.I4)]
		public int bActive;

		// Token: 0x040003EB RID: 1003
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003EC RID: 1004
		[MarshalAs(UnmanagedType.I4)]
		public int hServer;

		// Token: 0x040003ED RID: 1005
		[MarshalAs(UnmanagedType.I4)]
		public int dwAccessRights;

		// Token: 0x040003EE RID: 1006
		[MarshalAs(UnmanagedType.I4)]
		public int dwBlobSize;

		// Token: 0x040003EF RID: 1007
		public IntPtr pBlob;

		// Token: 0x040003F0 RID: 1008
		[MarshalAs(UnmanagedType.I2)]
		public short vtRequestedDataType;

		// Token: 0x040003F1 RID: 1009
		[MarshalAs(UnmanagedType.I2)]
		public short vtCanonicalDataType;

		// Token: 0x040003F2 RID: 1010
		public OPCEUTYPE dwEUType;

		// Token: 0x040003F3 RID: 1011
		[MarshalAs(UnmanagedType.Struct)]
		public object vEUInfo;
	}
}
