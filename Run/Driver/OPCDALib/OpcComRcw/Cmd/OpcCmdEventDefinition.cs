using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x0200005F RID: 95
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdEventDefinition
	{
		// Token: 0x040002F1 RID: 753
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x040002F2 RID: 754
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040002F3 RID: 755
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDataTypeDefinition;

		// Token: 0x040002F4 RID: 756
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
