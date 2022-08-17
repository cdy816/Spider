using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000093 RID: 147
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMPROPERTY
	{
		// Token: 0x040003FA RID: 1018
		[MarshalAs(UnmanagedType.I2)]
		public short vtDataType;

		// Token: 0x040003FB RID: 1019
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x040003FC RID: 1020
		[MarshalAs(UnmanagedType.I4)]
		public int dwPropertyID;

		// Token: 0x040003FD RID: 1021
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szItemID;

		// Token: 0x040003FE RID: 1022
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040003FF RID: 1023
		[MarshalAs(UnmanagedType.Struct)]
		public object vValue;

		// Token: 0x04000400 RID: 1024
		[MarshalAs(UnmanagedType.I4)]
		public int hrErrorID;

		// Token: 0x04000401 RID: 1025
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
