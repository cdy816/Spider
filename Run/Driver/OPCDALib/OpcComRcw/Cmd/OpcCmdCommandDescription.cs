using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000065 RID: 101
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdCommandDescription
	{
		// Token: 0x04000311 RID: 785
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x04000312 RID: 786
		[MarshalAs(UnmanagedType.I4)]
		public int bIsGlobal;

		// Token: 0x04000313 RID: 787
		[MarshalAs(UnmanagedType.R8)]
		public double dblExecutionTime;

		// Token: 0x04000314 RID: 788
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfEventDefinitions;

		// Token: 0x04000315 RID: 789
		public IntPtr pEventDefinitions;

		// Token: 0x04000316 RID: 790
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfStateDefinitions;

		// Token: 0x04000317 RID: 791
		public IntPtr pStateDefinitions;

		// Token: 0x04000318 RID: 792
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfActionDefinitions;

		// Token: 0x04000319 RID: 793
		public IntPtr pActionDefinitions;

		// Token: 0x0400031A RID: 794
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfTransitions;

		// Token: 0x0400031B RID: 795
		public IntPtr pTransitions;

		// Token: 0x0400031C RID: 796
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfInArguments;

		// Token: 0x0400031D RID: 797
		public IntPtr pInArguments;

		// Token: 0x0400031E RID: 798
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfOutArguments;

		// Token: 0x0400031F RID: 799
		public IntPtr pOutArguments;

		// Token: 0x04000320 RID: 800
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfSupportedControls;

		// Token: 0x04000321 RID: 801
		public IntPtr pszSupportedControls;

		// Token: 0x04000322 RID: 802
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfAndDependencies;

		// Token: 0x04000323 RID: 803
		public IntPtr pszAndDependencies;

		// Token: 0x04000324 RID: 804
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfOrDependencies;

		// Token: 0x04000325 RID: 805
		public IntPtr pszOrDependencies;

		// Token: 0x04000326 RID: 806
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfNotDependencies;

		// Token: 0x04000327 RID: 807
		public IntPtr pszNotDependencies;
	}
}
