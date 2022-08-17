using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace OpcRcw.Batch
{
	// Token: 0x02000009 RID: 9
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCBATCHSUMMARY
	{
		// Token: 0x04000049 RID: 73
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szID;

		// Token: 0x0400004A RID: 74
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szDescription;

		// Token: 0x0400004B RID: 75
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szOPCItemID;

		// Token: 0x0400004C RID: 76
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szMasterRecipeID;

		// Token: 0x0400004D RID: 77
		[MarshalAs(UnmanagedType.R4)]
		public float fBatchSize;

		// Token: 0x0400004E RID: 78
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szEU;

		// Token: 0x0400004F RID: 79
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szExecutionState;

		// Token: 0x04000050 RID: 80
		[MarshalAs(UnmanagedType.LPWStr)]
		public string szExecutionMode;

		// Token: 0x04000051 RID: 81
		public System.Runtime.InteropServices.ComTypes.FILETIME ftActualStartTime;

		// Token: 0x04000052 RID: 82
		public System.Runtime.InteropServices.ComTypes.FILETIME ftActualEndTime;
	}
}
