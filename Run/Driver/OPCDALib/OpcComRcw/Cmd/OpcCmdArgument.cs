using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000064 RID: 100
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OpcCmdArgument
	{
		// Token: 0x0400030F RID: 783
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szName;

		// Token: 0x04000310 RID: 784
		[MarshalAs(UnmanagedType.Struct)]
		public object vValue;
	}
}
