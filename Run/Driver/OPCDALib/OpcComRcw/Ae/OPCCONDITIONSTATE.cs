using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000075 RID: 117
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCCONDITIONSTATE
	{
		// Token: 0x0400036F RID: 879
		[MarshalAs(UnmanagedType.I2)]
		public short wState;

		// Token: 0x04000370 RID: 880
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved1;

		// Token: 0x04000371 RID: 881
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szActiveSubCondition;

		// Token: 0x04000372 RID: 882
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szASCDefinition;

		// Token: 0x04000373 RID: 883
		[MarshalAs(UnmanagedType.I4)]
		public int dwASCSeverity;

		// Token: 0x04000374 RID: 884
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szASCDescription;

		// Token: 0x04000375 RID: 885
		[MarshalAs(UnmanagedType.I2)]
		public short wQuality;

		// Token: 0x04000376 RID: 886
		[MarshalAs(UnmanagedType.I2)]
		public short wReserved2;

		// Token: 0x04000377 RID: 887
		public FILETIME ftLastAckTime;

		// Token: 0x04000378 RID: 888
		public FILETIME ftSubCondLastActive;

		// Token: 0x04000379 RID: 889
		public FILETIME ftCondLastActive;

		// Token: 0x0400037A RID: 890
		public FILETIME ftCondLastInactive;

		// Token: 0x0400037B RID: 891
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szAcknowledgerID;

		// Token: 0x0400037C RID: 892
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szComment;

		// Token: 0x0400037D RID: 893
		[MarshalAs(UnmanagedType.I4)]
		public int dwNumSCs;

		// Token: 0x0400037E RID: 894
		public IntPtr pszSCNames;

		// Token: 0x0400037F RID: 895
		public IntPtr pszSCDefinitions;

		// Token: 0x04000380 RID: 896
		public IntPtr pdwSCSeverities;

		// Token: 0x04000381 RID: 897
		public IntPtr pszSCDescriptions;

		// Token: 0x04000382 RID: 898
		public int dwNumEventAttrs;

		// Token: 0x04000383 RID: 899
		public IntPtr pEventAttributes;

		// Token: 0x04000384 RID: 900
		public IntPtr pErrors;
	}
}
