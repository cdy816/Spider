using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x0200003F RID: 63
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_TIME
	{
		// Token: 0x04000222 RID: 546
		[MarshalAs(UnmanagedType.I4)]
		public int bString;

		// Token: 0x04000223 RID: 547
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szTime;

		// Token: 0x04000224 RID: 548
		public OPCHDA_FILETIME ftTime;
	}
}
