using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x0200003C RID: 60
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_ANNOTATION
	{
		// Token: 0x0400020F RID: 527
		[MarshalAs(UnmanagedType.I4)]
		public int hClient;

		// Token: 0x04000210 RID: 528
		[MarshalAs(UnmanagedType.I4)]
		public int dwNumValues;

		// Token: 0x04000211 RID: 529
		public IntPtr ftTimeStamps;

		// Token: 0x04000212 RID: 530
		public IntPtr szAnnotation;

		// Token: 0x04000213 RID: 531
		public IntPtr ftAnnotationTime;

		// Token: 0x04000214 RID: 532
		public IntPtr szUser;
	}
}
