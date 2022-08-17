using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x0200003D RID: 61
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_MODIFIEDITEM
	{
		// Token: 0x04000215 RID: 533
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x04000216 RID: 534
		[MarshalAs(UnmanagedType.I4)]
		public int dwCount;

		// Token: 0x04000217 RID: 535
		public IntPtr pftTimeStamps;

		// Token: 0x04000218 RID: 536
		public IntPtr pdwQualities;

		// Token: 0x04000219 RID: 537
		public IntPtr pvDataValues;

		// Token: 0x0400021A RID: 538
		public IntPtr pftModificationTime;

		// Token: 0x0400021B RID: 539
		public IntPtr pEditType;

		// Token: 0x0400021C RID: 540
		public IntPtr szUser;
	}
}
