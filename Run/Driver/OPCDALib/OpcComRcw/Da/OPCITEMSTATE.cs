using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008E RID: 142
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMSTATE
	{
		// Token: 0x040003D0 RID: 976
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x040003D1 RID: 977
		public FILETIME ftTimeStamp;

		// Token: 0x040003D2 RID: 978
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x040003D3 RID: 979
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x040003D4 RID: 980
		[MarshalAs(UnmanagedType.Struct)]
		public object vDataValue;
	}
}
