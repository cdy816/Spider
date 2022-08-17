using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x02000094 RID: 148
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCITEMPROPERTIES
	{
		// Token: 0x04000402 RID: 1026
		[MarshalAs(UnmanagedType.I4)]
		public int hrErrorID;

		// Token: 0x04000403 RID: 1027
		[MarshalAs(UnmanagedType.I4)]
		public int dwNumProperties;

		// Token: 0x04000404 RID: 1028
		public IntPtr pItemProperties;

		// Token: 0x04000405 RID: 1029
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
