using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000061 RID: 97
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdActionDefinition
	{
		// Token: 0x040002F9 RID: 761
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x040002FA RID: 762
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040002FB RID: 763
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEventName;

		// Token: 0x040002FC RID: 764
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szInArguments;

		// Token: 0x040002FD RID: 765
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szOutArguments;

		// Token: 0x040002FE RID: 766
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
