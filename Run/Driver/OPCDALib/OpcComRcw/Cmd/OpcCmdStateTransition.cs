using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000062 RID: 98
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdStateTransition
	{
		// Token: 0x040002FF RID: 767
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTransitionID;

		// Token: 0x04000300 RID: 768
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szStartState;

		// Token: 0x04000301 RID: 769
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEndState;

		// Token: 0x04000302 RID: 770
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTriggerEvent;

		// Token: 0x04000303 RID: 771
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szAction;

		// Token: 0x04000304 RID: 772
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;
	}
}
