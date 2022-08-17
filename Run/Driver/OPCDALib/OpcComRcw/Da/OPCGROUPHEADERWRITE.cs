using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200008C RID: 140
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCGROUPHEADERWRITE
	{
		// Token: 0x040003CA RID: 970
		[MarshalAs(UnmanagedType.I4)]
		public int dwItemCount;

		// Token: 0x040003CB RID: 971
		[MarshalAs(UnmanagedType.I4)]
		public int hClientGroup;

		// Token: 0x040003CC RID: 972
		[MarshalAs(UnmanagedType.I4)]
		public int dwTransactionID;

		// Token: 0x040003CD RID: 973
		[MarshalAs(UnmanagedType.I4)]
		public int hrStatus;
	}
}
