using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace OpcRcw.Cmd
{
	// Token: 0x02000066 RID: 102
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdStateChangeEvent
	{
		// Token: 0x04000328 RID: 808
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEventName;

		// Token: 0x04000329 RID: 809
		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved;

		// Token: 0x0400032A RID: 810
		public System.Runtime.InteropServices.ComTypes.FILETIME ftEventTime;

		// Token: 0x0400032B RID: 811
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEventData;

		// Token: 0x0400032C RID: 812
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szOldState;

		// Token: 0x0400032D RID: 813
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szNewState;

		// Token: 0x0400032E RID: 814
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szStateData;

		// Token: 0x0400032F RID: 815
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfInArguments;

		// Token: 0x04000330 RID: 816
		public IntPtr pInArguments;

		// Token: 0x04000331 RID: 817
		[MarshalAs(UnmanagedType.I4)]
		public int dwNoOfOutArguments;

		// Token: 0x04000332 RID: 818
		public IntPtr pOutArguments;
	}
}
