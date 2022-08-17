using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000040 RID: 64
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_ITEM
	{
		// Token: 0x04000225 RID: 549
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x04000226 RID: 550
		[MarshalAs(UnmanagedType.I4)]
		public int haAggregate;

		// Token: 0x04000227 RID: 551
		[MarshalAs(UnmanagedType.I4)]
		public int dwCount;

		// Token: 0x04000228 RID: 552
		public IntPtr pftTimeStamps;

		// Token: 0x04000229 RID: 553
		public IntPtr pdwQualities;

		// Token: 0x0400022A RID: 554
		public IntPtr pvDataValues;
	}
}
