using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x0200003E RID: 62
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_ATTRIBUTE
	{
		// Token: 0x0400021D RID: 541
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x0400021E RID: 542
		[MarshalAs(UnmanagedType.I4)]
		public int dwNumValues;

		// Token: 0x0400021F RID: 543
		[MarshalAs(UnmanagedType.I4)]
		public int dwAttributeID;

		// Token: 0x04000220 RID: 544
		public IntPtr ftTimeStamps;

		// Token: 0x04000221 RID: 545
		public IntPtr vAttributeValues;
	}
}
