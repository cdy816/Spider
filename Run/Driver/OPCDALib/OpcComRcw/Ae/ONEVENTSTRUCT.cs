using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000073 RID: 115
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ONEVENTSTRUCT
	{
		// Token: 0x04000354 RID: 852
		[MarshalAs(UnmanagedType.I2)]
		public short wChangeMask;

		// Token: 0x04000355 RID: 853
		[MarshalAs(UnmanagedType.I2)]
		public short wNewState;

		// Token: 0x04000356 RID: 854
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szSource;

		// Token: 0x04000357 RID: 855
		public FILETIME ftTime;

		// Token: 0x04000358 RID: 856
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szMessage;

		// Token: 0x04000359 RID: 857
		[MarshalAs(UnmanagedType.I4)]
		public int dwEventType;

		// Token: 0x0400035A RID: 858
		[MarshalAs(UnmanagedType.I4)]
		public int dwEventCategory;

		// Token: 0x0400035B RID: 859
		[MarshalAs(UnmanagedType.I4)]
		public int dwSeverity;

		// Token: 0x0400035C RID: 860
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szConditionName;

		// Token: 0x0400035D RID: 861
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szSubconditionName;

		// Token: 0x0400035E RID: 862
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x0400035F RID: 863
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved;

		// Token: 0x04000360 RID: 864
		[MarshalAs(UnmanagedType.I4)]
		public int bAckRequired;

		// Token: 0x04000361 RID: 865
		public FILETIME ftActiveTime;

		// Token: 0x04000362 RID: 866
		[MarshalAs(UnmanagedType.I4)]
		public int dwCookie;

		// Token: 0x04000363 RID: 867
		[MarshalAs(UnmanagedType.I4)]
		public int dwNumEventAttrs;

		// Token: 0x04000364 RID: 868
		public IntPtr pEventAttributes;

		// Token: 0x04000365 RID: 869
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szActorID;
	}
}
