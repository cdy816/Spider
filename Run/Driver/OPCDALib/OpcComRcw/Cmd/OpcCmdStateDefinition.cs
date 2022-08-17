using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000060 RID: 96
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdStateDefinition
	{
		// Token: 0x040002F5 RID: 757
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x040002F6 RID: 758
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040002F7 RID: 759
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDataTypeDefinition;

		// Token: 0x040002F8 RID: 760
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
