using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000063 RID: 99
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdArgumentDefinition
	{
		// Token: 0x04000305 RID: 773
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x04000306 RID: 774
		[MarshalAs(UnmanagedType.I2)]
		public short vtValueType;

		// Token: 0x04000307 RID: 775
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x04000308 RID: 776
		[MarshalAs(UnmanagedType.I4)]
		public int bOptional;

		// Token: 0x04000309 RID: 777
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x0400030A RID: 778
		[MarshalAs(UnmanagedType.Struct)]
		public object vDefaultValue;

		// Token: 0x0400030B RID: 779
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szUnitType;

		// Token: 0x0400030C RID: 780
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;

		// Token: 0x0400030D RID: 781
		[MarshalAs(UnmanagedType.Struct)]
		public object vLowLimit;

		// Token: 0x0400030E RID: 782
		[MarshalAs(UnmanagedType.Struct)]
		public object vHighLimit;
	}
}
