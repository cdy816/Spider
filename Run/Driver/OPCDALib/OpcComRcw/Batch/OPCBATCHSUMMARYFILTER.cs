using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace OpcRcw.Batch
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCBATCHSUMMARYFILTER
	{
		// Token: 0x04000053 RID: 83
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szID;

		// Token: 0x04000054 RID: 84
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x04000055 RID: 85
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szOPCItemID;

		// Token: 0x04000056 RID: 86
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szMasterRecipeID;

		// Token: 0x04000057 RID: 87
		[MarshalAs(UnmanagedType.R4)]
		public float fMinBatchSize;

		// Token: 0x04000058 RID: 88
		[MarshalAs(UnmanagedType.R4)]
		public float fMaxBatchSize;

		// Token: 0x04000059 RID: 89
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEU;

		// Token: 0x0400005A RID: 90
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szExecutionState;

		// Token: 0x0400005B RID: 91
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szExecutionMode;

		// Token: 0x0400005C RID: 92
		public System.Runtime.InteropServices.ComTypes.FILETIME ftMinStartTime;

		// Token: 0x0400005D RID: 93
		public System.Runtime.InteropServices.ComTypes.FILETIME ftMaxStartTime;

		// Token: 0x0400005E RID: 94
		public System.Runtime.InteropServices.ComTypes.FILETIME ftMinEndTime;

		// Token: 0x0400005F RID: 95
		public System.Runtime.InteropServices.ComTypes.FILETIME ftMaxEndTime;
	}
}
