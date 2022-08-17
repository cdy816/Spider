using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Dx
{
	// Token: 0x02000015 RID: 21
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DXGeneralResponse
	{
		// Token: 0x040000F2 RID: 242
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szConfigurationVersion;

		// Token: 0x040000F3 RID: 243
		[MarshalAs(UnmanagedType.I4)]
		public int dwCount;

		// Token: 0x040000F4 RID: 244
		public IntPtr pIdentifiedResults;

		// Token: 0x040000F5 RID: 245
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
