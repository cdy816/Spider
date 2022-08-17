using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x0200005C RID: 92
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdNamespaceDefinition
	{
		// Token: 0x040002E3 RID: 739
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szUri;

		// Token: 0x040002E4 RID: 740
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x040002E5 RID: 741
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfCommandNames;

		// Token: 0x040002E6 RID: 742
		public IntPtr pszCommandNames;
	}
}
