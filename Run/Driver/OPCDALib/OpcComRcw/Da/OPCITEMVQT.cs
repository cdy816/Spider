using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000096 RID: 150
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMVQT
	{
		// Token: 0x0400040B RID: 1035
		[MarshalAs(UnmanagedType.Struct)]
		public object vDataValue;

		// Token: 0x0400040C RID: 1036
		[MarshalAs(UnmanagedType.I4)]
		public int bQualitySpecified;

		// Token: 0x0400040D RID: 1037
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x0400040E RID: 1038
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x0400040F RID: 1039
		[MarshalAs(UnmanagedType.I4)]
		public int bTimeStampSpecified;

		// Token: 0x04000410 RID: 1040
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;

		// Token: 0x04000411 RID: 1041
		public FILETIME ftTimeStamp;
	}
}
