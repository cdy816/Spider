using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x0200005E RID: 94
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdTargetElement
	{
		// Token: 0x040002EB RID: 747
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szLabel;

		// Token: 0x040002EC RID: 748
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTargetID;

		// Token: 0x040002ED RID: 749
		[MarshalAs(UnmanagedType.I4)]
		public int bIsTarget;

		// Token: 0x040002EE RID: 750
		[MarshalAs(UnmanagedType.I4)]
		public int bHasChildren;

		// Token: 0x040002EF RID: 751
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfNamespaceUris;

		// Token: 0x040002F0 RID: 752
		public IntPtr pszNamespaceUris;
	}
}
